using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    public class Announcement: Entity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string PhotoUrl { get; set; }
        public virtual ICollection<SentAnnouncement> Roles { get; set; }
    }
}
