using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.Data.Accounts
{
    public class ExternalCredential : IHasId<Guid>
    {
        public Guid GetId() => this.Id;
        public Guid Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Guid ApplicationUserId { get; set; }

        public string Provider { get; set; }

        public string ExternalId { get; set; }
    }
}
