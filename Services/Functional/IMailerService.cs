using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Functional
{
    public interface IMailerService
    {
        Task SendEmail(string to, string subject, string text, string html);
    }
}
