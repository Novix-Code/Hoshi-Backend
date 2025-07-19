using Azure;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using MimeKit;
using OtpNet;

namespace Hoshi.Repositories.EmailServiceFold
{
    public class EmailService: IEmailService
    {

        #region fields and ctor
        private readonly IConfiguration _configuration;
        private byte[] _secretKey;
        private const int otpDefaultSteps = 1800; //1800 second (30 minutes)

        public EmailService(IConfiguration configuration, int noOfBytes = 16)
        {
            _configuration = configuration;
            _secretKey = GenerateRandomSecretKey(noOfBytes);
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
                    <p>To complete your registration, please use the following Admin Code and Password  for verification:</p>
                    <p> AdminCode : {AdminCode} </p>            
                    <p class=""otp-code"">Password:  {Password} </p>
                    <p> Please check login page : {PageLinke} </p>
                </div>
                <div class=""email-footer"">
                    <p>Thank you for choosing <strong>Novix</strong>. We’re here to support you every step of the way.</p>
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

                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }



                return ResultDTO<string>.Success("Successfully Send Email");
            }
            catch (Exception ex)
            {

                return ResultDTO<string>.Failure(new ErrorDTO() , ResponseStatusCodes.BadRequest);

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
                    var newText = Template.Replace("{AdminCode}", OTP);
                    emailMessage.Body = new TextPart("html")
                    {
                        Text = newText
                    };
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Bypass SSL validation for debugging only
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

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
        public string GenerateOtp(byte[] secretKey = null, int otpExpirationTime = otpDefaultSteps, int otpSize = 8) //generate otp from provided secret key and otpExpirationTime 
        {
            secretKey ??= this._secretKey; //if user didn't provide a secret key it will be the same secretKey of the object
            var totp = new Totp(secretKey, step: otpExpirationTime, totpSize: otpSize);
            return totp.ComputeTotp();
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
                    var newText = TemplateOTP2.Replace("{OTPCode}", OTP);
                    emailMessage.Body = new TextPart("html")
                    {
                        Text = newText
                    };
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Bypass SSL validation for debugging only
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

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
        #endregion
    }
}
