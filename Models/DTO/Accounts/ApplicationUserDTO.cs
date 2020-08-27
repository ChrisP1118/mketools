using MkeTools.Web.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.Accounts
{
    public class ApplicationUserDTO
    {
        [Required]
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
