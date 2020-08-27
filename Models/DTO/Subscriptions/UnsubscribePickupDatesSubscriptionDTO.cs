using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.Subscriptions
{
    public class UnsubscribePickupDatesSubscriptionDTO
    {
        public Guid SubscriptionId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string Hash { get; set; }
    }
}
