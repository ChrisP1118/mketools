using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Functional
{
    public class SendGridMailerService : MailerService, IMailerService
    {
        public SendGridMailerService(IConfiguration configuration, ILogger<SendGridMailerService> logger) : base(configuration, logger)
        {
        }

        protected async override Task SendEmailInternal(string toName, string toAddress, string fromName, string fromAddress, string subject, string text, string html)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var message = MailHelper.CreateSingleEmail(new EmailAddress(fromAddress, fromName), new EmailAddress(toAddress, toName), subject, text, html);
            var response = await client.SendEmailAsync(message);

            if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
                _logger.LogInformation("Sending email {EmailAction} to {EmailAddress}: {EmaiSubject}", "succeeded", toAddress, subject);
            else
            {
                string errorText = await response.Body.ReadAsStringAsync();
                _logger.LogError("Sending email {EmailAction} to {EmailAddress}: {EmailSubect}: {EmailResponseStatusCode}, {EmailResponseBody}", "failed", toAddress, subject, response.StatusCode, errorText);
            }
        }
    }
}
