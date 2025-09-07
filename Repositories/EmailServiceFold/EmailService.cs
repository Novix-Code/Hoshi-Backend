using Azure;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.Models.UserModels;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OtpNet;

namespace Hoshi.Repositories.EmailServiceFold
{
    /// <summary>
    /// Service for sending transactional emails (OTP, admin credentials) and managing OTP lifecycle.
    /// Uses MailKit SMTP and OtpNet TOTP generation.
    /// Note: SSL validation bypass is enabled for debugging; see commented fix below.
    /// </summary>
    public class EmailService : IEmailService
    {

        #region fields and ctor
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly HoshiDbContext _context;

        private byte[] _secretKey;
        private const int otpDefaultSteps = 1800; //1800 second (30 minutes)

        public EmailService(IConfiguration configuration, int noOfBytes = 16, UserManager<User> userManager = null, HoshiDbContext context = null)
        {
            _configuration = configuration;
            _secretKey = GenerateRandomSecretKey(noOfBytes);
            _userManager = userManager;
            _context = context;
        }
        public byte[] GenerateRandomSecretKey(int noOfBytes)
        {
            return KeyGeneration.GenerateRandomKey(noOfBytes);
        }
        #endregion
        #region template

        private readonly string Template = @"<!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Account Verification</title>
            <style>
            .email-container {
                font-family: 'Georgia', serif;
                max-width: 700px;
                margin: 0 auto;
                padding: 30px;
                border: 2px solid #7f8c8d;
                border-radius: 12px;
                background-color: #ecf0f1;
            }
            .email-body h1 {
                color: #2980b9;
                font-size: 28px;
                margin-bottom: 20px;
            }
            .email-body p {
                color: #333333;
                line-height: 1.7;
                margin-bottom: 25px;
            }
            .otp-code {
                font-size: 24px;
                color: #e74c3c;
                font-weight: bold;
            }
            .email-footer {
                text-align: center;
                margin-top: 30px;
            }
            .email-footer p {
                color: #666666;
                font-size: 16px;
                margin-bottom: 12px;
            }
        </style>
        </head>
        <body>
            <div class=""email-container"">
                <div class=""email-body"">
                    <h1>Hello, {Name}!</h1>
                    <p>To complete your registration, please use the following User Code and Password  for verification:</p>
                    <p> AdminCode : {AdminCode} </p>            
                    <p class=""otp-code"">Password:  {Password} </p>
                    <p> Please check login page : {PageLinke} </p>
                </div>
                <div class=""email-footer"">
                    <p>Thank you for choosing <strong>Hoshi</strong>. We’re here to support you every step of the way.</p>
                </div>
            </div>
        </body>
        </html>";
        private readonly string TemplateOTP2 = @"<!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Account Verification</title>
            <style>
            .email-container {
                font-family: 'Georgia', serif;
                max-width: 700px;
                margin: 0 auto;
                padding: 30px;
                border: 2px solid #7f8c8d;
                border-radius: 12px;
                background-color: #ecf0f1;
            }
            .email-body h1 {
                color: #2980b9;
                font-size: 28px;
                margin-bottom: 20px;
            }
            .email-body p {
                color: #333333;
                line-height: 1.7;
                margin-bottom: 25px;
            }
            .otp-code {
                font-size: 24px;
                color: #e74c3c;
                font-weight: bold;
            }
            .email-footer {
                text-align: center;
                margin-top: 30px;
            }
            .email-footer p {
                color: #666666;
                font-size: 16px;
                margin-bottom: 12px;
            }
        </style>
        </head>
        <body>
            <div class=""email-container"">
                <div class=""email-body"">
                    <p>To complete your registration, please use the following OTP to verification:</p>
                    <p class=""otp-code"">OTP:  {OTPCode} </p>
                    <p>This OTP code is valid for a limited time, so be sure to use it promptly to access your account.</p>
                </div>
                <div class=""email-footer"">
                    <p>Thank you for choosing <strong>Novix</strong>. We’re here to support you every step of the way.</p>
                </div>
            </div>
        </body>
        </html>";
        private readonly string Template3 = @"<!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Account Verification</title>
            <style>
            .email-container {
                font-family: 'Georgia', serif;
                max-width: 700px;
                margin: 0 auto;
                padding: 30px;
                border: 2px solid #7f8c8d;
                border-radius: 12px;
                background-color: #ecf0f1;
            }
            .email-body h1 {
                color: #2980b9;
                font-size: 28px;
                margin-bottom: 20px;
            }
            .email-body p {
                color: #333333;
                line-height: 1.7;
                margin-bottom: 25px;
            }
            .otp-code {
                font-size: 24px;
                color: #e74c3c;
                font-weight: bold;
            }
            .email-footer {
                text-align: center;
                margin-top: 30px;
            }
            .email-footer p {
                color: #666666;
                font-size: 16px;
                margin-bottom: 12px;
            }
        </style>
        </head>
        <body>
            <div class=""email-container"">
                <div class=""email-body"">
                    <h1>Hello,there!</h1>
                    <p> your verification Code : {AdminCode}</p>          
                </div>
                <div class=""email-footer"">
                    <p>Thank you for choosing <strong>Novix</strong>. We’re here to support you every step of the way.</p>
                </div>
            </div>
        </body>
        </html>";

        private readonly string PageLinke = "https://www.google.com/";

        #endregion
        #region Functions
        public async Task<ResultDTO<string>> SendEmail(string email, string AdminCode, string DefaultPassword, string userName)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Ahmed Toba", _configuration["SmtpSettings:Username"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Complete Login";
                if (!string.IsNullOrEmpty(Template))
                {
                    // Replace placeholders with the customer's name and the generated OTP code
                    var newText = Template.Replace("{AdminCode}", AdminCode).Replace("{Password}", DefaultPassword).Replace("{PageLinke}", PageLinke).Replace("{Name}", userName);
                    emailMessage.Body = new TextPart("html")
                    {
                        Text = newText
                    };
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Bypass SSL validation for debugging only
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    // Suggested improvement (do not bypass SSL in production):
                    // client.ServerCertificateValidationCallback = null;

                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }



                return ResultDTO<string>.Success("Successfully Send Email");
            }
            catch (Exception ex)
            {

                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            }
        }
        public async Task<ResultDTO<string>> SendVerifivationCode(string email)
        {
            try
            {
                var OTP = GenerateOtp();
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Ahmed Toba", _configuration["SmtpSettings:Username"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Verification Code";
                if (!string.IsNullOrEmpty(Template))
                {
                    // Replace placeholders with the customer's name and the generated OTP code
                    var newText = Template.Replace("{AdminCode}", OTP.ComputeTotp());
                    emailMessage.Body = new TextPart("html")
                    {
                        Text = newText
                    };
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Bypass SSL validation for debugging only
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    // Suggested improvement: remove the bypass in production.

                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }



                return ResultDTO<string>.Success("Successfully Send Email");
            }
            catch (Exception ex)
            {

                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            }
        }
        public Totp GenerateOtp(byte[] secretKey = null, int otpExpirationTime = otpDefaultSteps, int otpSize = 8) //generate otp from provided secret key and otpExpirationTime 
        {
            secretKey ??= this._secretKey; //if user didn't provide a secret key it will be the same secretKey of the object
            var totp = new Totp(secretKey, step: otpExpirationTime, totpSize: otpSize);
            return totp;
        }
        public async Task<ResultDTO<string>> SendOTP(string email)
        {
            try
            {
                var OTP = GenerateOtp();

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Ahmed Toba", _configuration["SmtpSettings:Username"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Complete Login";
                if (!string.IsNullOrEmpty(TemplateOTP2))
                {
                    // Replace placeholders with the customer's name and the generated OTP code
                    var newText = TemplateOTP2.Replace("{OTPCode}", OTP.ComputeTotp());
                    emailMessage.Body = new TextPart("html")
                    {
                        Text = newText
                    };
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Bypass SSL validation for debugging only
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    // Suggested improvement: remove the bypass in production.

                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
                var _user = await _userManager.FindByEmailAsync(email);
                var userOTPDTO = new UserOTP
                {
                    Code = OTP.ComputeTotp(),
                    CreatedAt = DateTime.Now,
                    IsRevoked = false,
                    SecreteKey = this._secretKey,
                    UserId = _user.Id

                };

                await _context.UserOTPs.AddAsync(userOTPDTO);
                await _context.SaveChangesAsync();

                return ResultDTO<string>.Success("Successfully Send OTP");
            }
            catch (Exception ex)
            {
                return ResultDTO<string>.Failure(
                    new ErrorDTO
                    {
                        ErrorAr = "فشل في ارسال ال OTP.",
                        ErrorEn = ex.InnerException == null ? ex.Message : ex.InnerException.Message
                    },
                    ResponseStatusCodes.BadRequest
                );
            }

        }
        public async Task<ResultDTO<object>> checkOTPVerfication(string otp, string userId)
        {
            var targetuserOtp = await _context.UserOTPs.Where(p => p.UserId == int.Parse(userId)).FirstOrDefaultAsync();

            if (targetuserOtp == null || targetuserOtp.Code != otp || targetuserOtp.IsRevoked)
            {
                return ResultDTO<object>.Failure(
                    new ErrorDTO
                    {
                        ErrorAr = "هذا ال OTP غير صحيح.",
                        ErrorEn = "This OTO is not Valied"
                    },
                    ResponseStatusCodes.BadRequest
                );
            }

            var totp = new Totp(targetuserOtp.SecreteKey, step: otpDefaultSteps, totpSize: 8);
            bool isValid = totp.VerifyTotp(otp, out long timeStepMatched, new VerificationWindow(previous: 1, future: 0));
            if (!isValid)
            {
                return ResultDTO<object>.Failure(
                    new ErrorDTO { 
                        ErrorAr = "هذا ال OTP منتهي، برجاء استخدام اعادة الارسال.",
                        ErrorEn = "This Otp is expired please use resend OTP" 
                    }, 
                    ResponseStatusCodes.NotFound
                );

            }
            var targetUser = await _userManager.FindByIdAsync(userId);
            targetUser.PhoneNumberConfirmed = true;
            targetUser.EmailConfirmed = true;
            targetuserOtp.IsRevoked = true;
            await _userManager.UpdateAsync(targetUser);
            await _context.SaveChangesAsync();
            return ResultDTO<object>.Success(true);
        }
        public async Task<ResultDTO<object>> ReSetOtp(string email)
        {
            try
            {
                // prepare the email contant
                var OTP = GenerateOtp();
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Ahmed Toba", _configuration["SmtpSettings:Username"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Complete Login";
                if (!string.IsNullOrEmpty(TemplateOTP2))
                {
                    // Replace placeholders with the customer's name and the generated OTP code
                    var newText = TemplateOTP2.Replace("{OTPCode}", OTP.ComputeTotp());
                    emailMessage.Body = new TextPart("html")
                    {
                        Text = newText
                    };
                }
                // check email, User and UserOTP
                var _user = await _userManager.FindByEmailAsync(email);
                if (_user == null)
                {
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = "this user not exist" }, ResponseStatusCodes.NotFound);
                }
                var targetUserOtp = await _context.UserOTPs.Where(p => p.UserId == _user.Id).FirstOrDefaultAsync();
                targetUserOtp.Code = OTP.ComputeTotp();
                targetUserOtp.IsRevoked = false;
                targetUserOtp.CreatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                // send Email
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Bypass SSL validation for debugging only
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    // Suggested improvement: remove the bypass in production.

                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }


                return ResultDTO<object>.Success("Successfully Send OTP");
            }
            catch (Exception ex)
            {
                return ResultDTO<object>.Failure(
                    new ErrorDTO
                    {
                        ErrorAr = "فشل في ارسال ال OTP.",
                        ErrorEn = ex.InnerException == null ? ex.Message : ex.InnerException.Message
                    },
                    ResponseStatusCodes.BadRequest
                );
            }
        }

        #endregion
    }
}
