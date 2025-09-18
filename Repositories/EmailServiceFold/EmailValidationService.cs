using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DnsClient;

namespace Hoshi.Repositories.EmailServiceFold
{
    public class EmailValidationService
    {
        private readonly LookupClient _dnsClient;

        // Temp emails not allow
        private static readonly HashSet<string> DisposableEmailDomains = new HashSet<string>
        {
            "10minutemail.com", "guerrillamail.com", "mailinator.com", "tempmail.org",
            "yopmail.com", "temp-mail.org", "throwaway.email", "maildrop.cc",
            "sharklasers.com", "guerrillamail.info", "guerrillamail.biz", "guerrillamail.net",
            "guerrillamail.org", "guerrillamail.de", "grr.la", "guerrillamailblock.com",
            "pokemail.net", "spam4.me", "bccto.me", "chacuo.net", "027168.com"
        };

        // Whitelist commonly problematic domains
        private static readonly HashSet<string> WhitelistedDomains = new HashSet<string>
        {
            "yahoo.com", "yahoo.co.uk", "yahoo.fr", "yahoo.de",
            "hotmail.com", "outlook.com", "live.com",
            "gmail.com", "googlemail.com",
            "icloud.com", "me.com", "mac.com"
        };

        public EmailValidationService()
        {
            _dnsClient = new LookupClient();
        }

        public async Task<EmailValidationResult> ValidateEmailAsync(string email)
        {
            var result = new EmailValidationResult { Email = email };

            try
            {
                // Step 1: Basic format validation
                if (!IsValidEmailFormat(email))
                {
                    result.IsValid = false;
                    result.Reason = "صيغة البريد الإلكتروني غير صحيحة";
                    return result;
                }

                var domain = email.Split('@')[1].ToLowerInvariant();

                // Step 2: Check for disposable/temporary email
                //if (IsDisposableEmail(domain))
                //{
                //    result.IsValid = false;
                //    result.Reason = "البريد الإلكتروني المؤقت غير مسموح";
                //    return result;
                //}

                // Step 3: DNS A record check (domain exists)
                if (!await DomainExistsAsync(domain))
                {
                    result.IsValid = false;
                    result.Reason = "النطاق غير موجود";
                    return result;
                }

                // Step 4: MX record check (has mail servers)
                var mxRecords = await GetMxRecordsAsync(domain);
                if (!mxRecords.Any())
                {
                    result.IsValid = false;
                    result.Reason = "لا يحتوي النطاق على خوادم بريد";
                    return result;
                }

                // Step 5: SMTP server connectivity check
                var canConnect = await CanConnectToAnyMxServerAsync(mxRecords);
                if (!WhitelistedDomains.Contains(domain))
                    if (!canConnect)
                    {
                        result.IsValid = false;
                        result.Reason = "خوادم البريد غير متاحة";
                        return result;
                    }

                result.IsValid = true;
                result.Reason = "البريد الإلكتروني صالح";
                result.MxRecords = mxRecords;

            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Reason = "خطأ في التحقق من البريد الإلكتروني";
                result.ErrorDetails = ex.Message;
            }

            return result;
        }

        private bool IsValidEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // RFC 5322 compliant regex (simplified)
                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                if (!emailRegex.IsMatch(email))
                    return false;

                // Additional validation using MailAddress
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> DomainExistsAsync(string domain)
        {
            try
            {
                var result = await _dnsClient.QueryAsync(domain, QueryType.A);
                return result.Answers.ARecords().Any();
            }
            catch
            {
                // Fallback to AAAA record check
                try
                {
                    var result = await _dnsClient.QueryAsync(domain, QueryType.AAAA);
                    return result.Answers.AaaaRecords().Any();
                }
                catch
                {
                    return false;
                }
            }
        }

        private async Task<List<string>> GetMxRecordsAsync(string domain)
        {
            try
            {
                var result = await _dnsClient.QueryAsync(domain, QueryType.MX);
                return result.Answers
                    .MxRecords()
                    .OrderBy(mx => mx.Preference)
                    .Select(mx => mx.Exchange.Value.TrimEnd('.'))
                    .ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        private async Task<bool> CanConnectToAnyMxServerAsync(List<string> mxRecords)
        {
            var tasks = mxRecords.Take(3).Select(server => TestSmtpConnectionAsync(server));
            var results = await Task.WhenAll(tasks);
            return results.Any(result => result);
        }

        private async Task<bool> TestSmtpConnectionAsync(string server)
        {
            var ports = new[] { 25, 587, 465 };

            foreach (var port in ports)
            {
                try
                {
                    using var tcpClient = new TcpClient();

                    // Set connection timeout
                    var connectTask = tcpClient.ConnectAsync(server, port);
                    var timeoutTask = Task.Delay(5000); // 5 seconds timeout

                    var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                    if (completedTask == connectTask && tcpClient.Connected)
                    {
                        return true;
                    }
                }
                catch
                {
                    // Try next port
                    continue;
                }
            }

            return false;
        }

        // Batch validation for better performance
        public async Task<List<EmailValidationResult>> ValidateEmailsBatchAsync(List<string> emails)
        {
            var tasks = emails.Select(email => ValidateEmailAsync(email));
            return (await Task.WhenAll(tasks)).ToList();
        }

        // Quick validation (without SMTP connectivity test)
        public async Task<EmailValidationResult> QuickValidateEmailAsync(string email)
        {
            var result = new EmailValidationResult { Email = email };

            try
            {
                if (!IsValidEmailFormat(email))
                {
                    result.IsValid = false;
                    result.Reason = "صيغة البريد الإلكتروني غير صحيحة";
                    return result;
                }

                var domain = email.Split('@')[1].ToLowerInvariant();

                if (DisposableEmailDomains.Contains(domain))
                {
                    result.IsValid = false;
                    result.Reason = "البريد الإلكتروني المؤقت غير مسموح";
                    return result;
                }

                if (!await DomainExistsAsync(domain))
                {
                    result.IsValid = false;
                    result.Reason = "النطاق غير موجود";
                    return result;
                }

                var mxRecords = await GetMxRecordsAsync(domain);
                if (!mxRecords.Any())
                {
                    result.IsValid = false;
                    result.Reason = "لا يحتوي النطاق على خوادم بريد";
                    return result;
                }

                result.IsValid = true;
                result.Reason = "البريد الإلكتروني صالح (فحص سريع)";
                result.MxRecords = mxRecords;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Reason = "خطأ في التحقق من البريد الإلكتروني";
                result.ErrorDetails = ex.Message;
            }

            return result;
        }
    }

    public class EmailValidationResult
    {
        public string Email { get; set; }
        public bool IsValid { get; set; }
        public string Reason { get; set; }
        public List<string> MxRecords { get; set; } = new List<string>();
        public string ErrorDetails { get; set; }
    }
}
