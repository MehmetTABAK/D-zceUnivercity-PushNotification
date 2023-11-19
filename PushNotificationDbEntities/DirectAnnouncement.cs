using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    public class DirectAnnouncement: Entity
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual User User { get; set; }
    }
}
