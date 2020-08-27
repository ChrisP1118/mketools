using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.Accounts
{
    public class ResetPasswordResultsDTO
    {
        /// <summary>
        /// The username of the authenticated user
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// The ID of the authenticated user
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// A list of roles to which the authenticated user belongs
        /// </summary>
        [Required]
        public List<string> Roles { get; set; }

        /// <summary>
        /// A JWT authentication token for the user
        /// </summary>
        [Required]
        public string JwtToken { get; set; }
    }
}
