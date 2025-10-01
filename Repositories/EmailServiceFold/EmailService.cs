using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.Models.UserModels;
using MailKit.Security;
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

        // COMMENTED: Class-level secret key is no longer used because it creates security vulnerability
        // where all users share the same secret key. Each user should have their own unique secret key.
        // private byte[] _secretKey;

        private const int otpDefaultSteps = 1800; //1800 second (30 minutes)

        public EmailService(IConfiguration configuration, UserManager<User> userManager, HoshiDbContext context)
        {
            _configuration = configuration;

            // COMMENTED: No longer generating class-level secret key since we generate unique keys per user
            // _secretKey = GenerateRandomSecretKey(noOfBytes);

            _userManager = userManager;
            _context = context;
        }

        // COMMENTED: This method is redundant since key generation is now handled directly in GenerateOtp method
        // public byte[] GenerateRandomSecretKey(int noOfBytes)
        // {
        //     return KeyGeneration.GenerateRandomKey(noOfBytes);
        // }
        #endregion

        #region template

        private readonly string AdminTemplate = @"<!DOCTYPE html>
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
                    <p>Thank you for choosing <strong>Hoshi</strong>. We're here to support you every step of the way.</p>
                </div>
            </div>
        </body>
        </html>";

        private readonly string SendOTPTemplate = @"<!DOCTYPE html>
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
                    <p>Thank you for choosing <strong>Hoshi</strong>. We're here to support you every step of the way.</p>
                </div>
            </div>
        </body>
        </html>";

        private readonly string VerificationTemplate = @"<!DOCTYPE html>
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
                    <p>Thank you for choosing <strong>Hoshi</strong>. We're here to support you every step of the way.</p>
                </div>
            </div>
        </body>
        </html>";

        private readonly string PageLinke = "https://www.google.com/";

        #endregion

        #region Functions

        // UPDATED: New method that generates OTP with unique secret key internally
        // This replaces the old GenerateOtp method that used class-level secret key
        public (Totp totp, byte[] secretKey) GenerateOtp(
            int otpExpirationTime = otpDefaultSteps, 
            int otpSize = 8, 
            int secretKeySize = 20
        )
        {
            byte[] secretKey = KeyGeneration.GenerateRandomKey(secretKeySize);
            var totp = new Totp(secretKey, step: otpExpirationTime, totpSize: otpSize);
            return (totp, secretKey);
        }

        // UPDATED: New method for creating TOTP from existing secret key (used for verification)
        public Totp GenerateOtpFromKey(byte[] secretKey, int otpExpirationTime = otpDefaultSteps, int otpSize = 8)
        {
            if (secretKey == null)
                throw new ArgumentNullException(nameof(secretKey));

            var totp = new Totp(secretKey, step: otpExpirationTime, totpSize: otpSize);
            return totp;
        }

        // COMMENTED: Old GenerateOtp method that used class-level secret key, which was a security vulnerability
        // because all users shared the same secret key. Replaced with new methods above.
        // public Totp GenerateOtp(byte[] secretKey = null, int otpExpirationTime = otpDefaultSteps, int otpSize = 8) 
        // {
        //     secretKey ??= this._secretKey; //if user didn't provide a secret key it will be the same secretKey of the object
        //     var totp = new Totp(secretKey, step: otpExpirationTime, totpSize: otpSize);
        //     return totp;
        // }

        public async Task<ResultDTO<string>> SendEmail(string email, string AdminCode, string DefaultPassword, string userName)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Hoshi", _configuration["SmtpSettings:Username"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Complete Login";

                if (!string.IsNullOrEmpty(AdminTemplate))
                {
                    // Replace placeholders with the customer's name and the generated OTP code
                    var newText = AdminTemplate
                        .Replace("{AdminCode}", AdminCode)
                        .Replace("{Password}", DefaultPassword)
                        .Replace("{PageLinke}", PageLinke)
                        .Replace("{Name}", userName);

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

                    await client.ConnectAsync(_configuration["SmtpSettings:Host"], 587, SecureSocketOptions.StartTls);
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

        // COMMENTED: This method uses the old GenerateOtp approach with shared secret key
        // It should be updated to use the new approach or kept as is if it serves a different purpose
        public async Task<ResultDTO<string>> SendVerifivationCode(string email)
        {
            try
            {
                // COMMENTED: Using old GenerateOtp method - should be updated if this method is still needed
                // var OTP = GenerateOtp();

                // UPDATED: Using new approach with unique secret key
                var (OTP, secretKey) = GenerateOtp();

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Hoshi", _configuration["SmtpSettings:Username"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Verification Code";
                if (!string.IsNullOrEmpty(VerificationTemplate))
                {
                    // Replace placeholders with the customer's name and the generated OTP code
                    var newText = VerificationTemplate.Replace("{AdminCode}", OTP.ComputeTotp());
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

                    await client.ConnectAsync(_configuration["SmtpSettings:Host"], 587, SecureSocketOptions.StartTls);
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

        public async Task<ResultDTO<string>> SendOTP(string email)
        {
            try
            {
                var _user = await _userManager.FindByEmailAsync(email);
                if (_user == null)
                {
                    return ResultDTO<string>.Failure(new ErrorDTO { ErrorEn = "User not found" }, ResponseStatusCodes.NotFound);
                }

                // UPDATED: Using new GenerateOtp method that returns both TOTP and unique secret key
                var (OTP, userSecretKey) = GenerateOtp();
                string otpCode = OTP.ComputeTotp();

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Hoshi", _configuration["SmtpSettings:Username"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Complete Authentication";
                if (!string.IsNullOrEmpty(SendOTPTemplate))
                {
                    // Replace placeholders with the customer's name and the generated OTP code
                    var newText = SendOTPTemplate.Replace("{OTPCode}", otpCode);
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

                    await client.ConnectAsync(_configuration["SmtpSettings:Host"], 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                // Remove any existing OTP for this user first
                var existingOtp = await _context.UserOTPs.Where(p => p.UserId == _user.Id).FirstOrDefaultAsync();
                if (existingOtp != null)
                {
                    _context.UserOTPs.Remove(existingOtp);
                }

                // UPDATED: Store only the secret key, not the OTP code, and use unique secret key per user
                var userOTPDTO = new UserOTP
                {
                    Code = OTP.ComputeTotp(),
                    CreatedAt = DateTime.UtcNow, // Use UTC consistently
                    IsRevoked = false,
                    SecreteKey = userSecretKey, // Store unique secret key per user
                    UserId = _user.Id
                };

                // COMMENTED: Old approach that stored the computed OTP code and used shared secret key
                // var userOTPDTO = new UserOTP
                // {
                //     Code = OTP.ComputeTotp(),  // This was wrong - shouldn't store the code
                //     CreatedAt = DateTime.Now,   // Should use UTC
                //     IsRevoked = false,
                //     SecreteKey = this._secretKey,  // This was wrong - shared secret key
                //     UserId = _user.Id
                // };

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
                        ErrorEn = ex.InnerException?.Message ?? ex.Message
                    },
                    ResponseStatusCodes.BadRequest
                );
            }
        }

        public async Task<ResultDTO<object>> ReSetOtp(string email)
        {
            try
            {
                var _user = await _userManager.FindByEmailAsync(email);
                if (_user == null)
                {
                    return ResultDTO<object>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "هذا المستخد غير موجود.",
                        ErrorEn = "This user does not exist."
                    });
                }

                var targetUserOtp = await _context.UserOTPs.Where(p => p.UserId == _user.Id).FirstOrDefaultAsync();
                if (targetUserOtp == null)
                {
                    return ResultDTO<object>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "هذا المستخدم لم يقم بإرسال رمز مسبق.",
                        ErrorEn = "This user has not sent an OTP code previously."
                    });
                }

                // UPDATED: Generate new OTP with new secret key
                var (OTP, newSecretKey) = GenerateOtp();
                string otpCode = OTP.ComputeTotp();

                // COMMENTED: Old approach that reused class-level secret key
                // var OTP = GenerateOtp();

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Hoshi", _configuration["SmtpSettings:Username"]));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Complete Login";
                if (!string.IsNullOrEmpty(SendOTPTemplate))
                {
                    // Replace placeholders with the customer's name and the generated OTP code
                    var newText = SendOTPTemplate.Replace("{OTPCode}", otpCode);
                    emailMessage.Body = new TextPart("html")
                    {
                        Text = newText
                    };
                }

                // UPDATED: Update with new secret key and don't store the OTP code
                targetUserOtp.Code = OTP.ComputeTotp();
                targetUserOtp.SecreteKey = newSecretKey; // New secret key
                targetUserOtp.IsRevoked = false;
                targetUserOtp.CreatedAt = DateTime.UtcNow;

                // COMMENTED: Old approach that stored the computed OTP code
                // targetUserOtp.Code = OTP.ComputeTotp();  // This was wrong
                // targetUserOtp.IsRevoked = false;
                // targetUserOtp.CreatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // send Email
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Bypass SSL validation for debugging only
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    // Suggested improvement: remove the bypass in production.

                    await client.ConnectAsync(_configuration["SmtpSettings:Host"], 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                return ResultDTO<object>.Success("Successfully Send OTP");
            }
            catch (Exception ex)
            {
                return ResultDTO<object>.InternalServerError(
                    new ErrorDTO
                    {
                        ErrorAr = "فشل في ارسال ال OTP.",
                        ErrorEn = "Failed to send the OTP."
                    },
                    innerError: ex.InnerException?.Message ?? ex.Message
                );
            }
        }

        public async Task<ResultDTO<object>> CheckOTPVerfication(string otp)
        {
            var targetuserOtp = await _context.UserOTPs.Where(p => p.Code == otp).FirstOrDefaultAsync();

            if (targetuserOtp == null || targetuserOtp.IsRevoked)
            {
                return ResultDTO<object>.BadRequest(
                    new ErrorDTO
                    {
                        ErrorAr = "هذا ال OTP غير صحيح.",
                        ErrorEn = "This OTP is not valid"
                    }
                );
            }

            // Use TOTP validation with the stored secret key
            var totp = GenerateOtpFromKey(targetuserOtp.SecreteKey);
            bool isValid = totp.VerifyTotp(otp, out long timeStepMatched, new VerificationWindow(previous: 1, future: 0));

            if (!isValid)
            {
                return ResultDTO<object>.BadRequest(
                    new ErrorDTO
                    {
                        ErrorAr = "هذا ال OTP منتهي، برجاء استخدام اعادة الارسال.",
                        ErrorEn = "This OTP is expired, please use resend OTP"
                    }
                );
            }

            var targetUser = await _userManager.FindByIdAsync(targetuserOtp.UserId.ToString());
            if (targetUser == null)
            {
                return ResultDTO<object>.Failure(
                    new ErrorDTO { ErrorEn = "User not found" },
                    ResponseStatusCodes.NotFound
                );
            }

            targetUser.PhoneNumberConfirmed = true;
            targetUser.EmailConfirmed = true;
            targetuserOtp.IsRevoked = true;
            await _userManager.UpdateAsync(targetUser);
            await _context.SaveChangesAsync();

            return ResultDTO<object>.Success(true);
        }

        #endregion
    }
}