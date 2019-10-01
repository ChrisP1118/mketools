using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Accounts
{
    public class RegisterDTO
    {
        /// <summary>
        /// Email address for the registering user
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Password for the registering user
        /// </summary>
        [Required]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
