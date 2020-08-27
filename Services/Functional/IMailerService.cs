using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Functional
{
    public interface IMailerService
    {
        Task SendEmail(string to, string subject, string text);
        Task SendEmail(string to, string subject, string text, string html);
        Task SendAdminAlert(string alertName, string alertMessage);
    }
}
