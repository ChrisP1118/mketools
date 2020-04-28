using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Models.Data.Subscriptions
{
    public class PickupDatesSubscription : Subscription
    {
        /// <summary>
        /// The number of hours before 12:00am when the notification should be sent. For instance, a value of -6 will send a notification for Jan. 5 at 6:00pm on Jan. 4. A value of 6 would send
        /// a notification for Jan. 5 at 6:00am on Jan. 5.
        /// </summary>
        public int HoursBefore { get; set; }

        public DateTime? NextGarbagePickupDate { get; set; }
        public DateTime? NextRecyclingPickupDate { get; set; }

        /// <summary>
        /// This is NextGarbagePickupDate minus HoursBefore. It's nulled out when a notification is sent at that time.
        /// </summary>
        public DateTime? NextGarbagePickupNotification { get; set; }
        public DateTime? NextRecyclingPickupNotification { get; set; }

        [MaxLength(10)]
        public string LADDR { get; set; }

        [MaxLength(1)]
        public string SDIR { get; set; }

        [MaxLength(18)]
        public string SNAME { get; set; }

        [MaxLength(2)]
        public string STYPE { get; set; }
    }
}
