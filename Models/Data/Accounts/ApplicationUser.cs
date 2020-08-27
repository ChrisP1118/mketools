using Microsoft.AspNetCore.Identity;
using MkeTools.Web.Models.Data.Subscriptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Accounts
{
    public class ApplicationUser : IdentityUser<Guid>, IHasId<Guid>
    {
        public Guid GetId() => this.Id;

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        public List<ExternalCredential> ExternalCredentials { get; set; }

        public List<DispatchCallSubscription> DispatchCallSubscriptions { get; set; }
        public List<PickupDatesSubscription> PickupDateSubscriptions { get; set; }
    }
}
