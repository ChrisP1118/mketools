using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.DTO.Accounts
{
    public class LoginDTO
    {
        /// <summary>
        /// The username of the authenticating user
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The password of the authenticating user
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
