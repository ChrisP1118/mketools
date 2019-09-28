using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Accounts
{
    public class LoginExternalCredentialDTO
    {
        /// <summary>
        /// The external credential provider
        /// </summary>
        [Required]
        public string Provider { get; set; }

        /// <summary>
        /// The external credential issued ID
        /// </summary>
        [Required]
        public string ExternalId { get; set; }

        /// <summary>
        /// Email address for the user
        /// </summary>
        [Required]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
