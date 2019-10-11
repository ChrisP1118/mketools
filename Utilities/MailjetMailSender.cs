using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Utilities
{
    public class MailjetMailSender : IMailSender
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<MailjetMailSender> _logger;

        public MailjetMailSender(IConfiguration configuration, ILogger<MailjetMailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmail(string to, string subject, string text, string html)
        {
            MailjetClient client = new MailjetClient(_configuration["MailjetPublicKey"], _configuration["MailjetPrivateKey"])
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray
            {
                new JObject
                {
                    {
                        "From", new JObject
                        {
                            {"Email", _configuration["MailSenderFromEmail"]},
                            {"Name", _configuration["MailSenderFromName"]}
                        }
                    },
                    {
                        "To", new JArray
                        {
                            new JObject
                            {
                                {"Email", to}
                            }
                        }
                    },
                    {
                        "Subject", subject
                    },
                    {
                        "TextPart", text
                    },
                    {
                        "HTMLPart", html
                    }
                }
            });

            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
                _logger.LogInformation("Email sent to " + to + " (" + subject + ")");
            else
                _logger.LogError("Email failed to send to " + to + " (" + subject + "): " + response.StatusCode + ", " + response.GetErrorInfo() + ", " + response.GetErrorMessage());
        }
    }
}
