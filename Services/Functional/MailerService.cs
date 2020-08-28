using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Functional
{
    public abstract class MailerService : IMailerService
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<MailerService> _logger;

        public MailerService(IConfiguration configuration, ILogger<MailerService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmail(string to, string subject, string text)
        {
            await SendEmail(to, subject, text, null);
        }

        public async Task SendEmail(string to, string subject, string text, string html)
        {
            await SendEmail(to, to, subject, text, html);
        }

        public async Task SendEmail(string toName, string toAddress, string subject, string text, string html)
        {
            string fromName = _configuration["MailSenderFromName"];
            string fromAddress = _configuration["MailSenderFromEmail"];

            if (!string.IsNullOrEmpty(_configuration["MailSenderOverride"]))
            {
                subject = "[Redirect: " + toAddress + "] " + subject;

                string[] overrideAddresses = _configuration["MailSenderOverride"].Split(';');
                foreach (string overrideAddress in overrideAddresses)
                {
                    await SendEmailInternal(overrideAddress, overrideAddress, fromName, fromAddress, subject, text, html);
                }
            }
            else
            {
                await SendEmailInternal(toName, toAddress, fromName, fromAddress, subject, text, html);
            }
        }

        protected abstract Task SendEmailInternal(string toName, string toAddress, string fromName, string fromAddress, string subject, string text, string html);
    }
}
