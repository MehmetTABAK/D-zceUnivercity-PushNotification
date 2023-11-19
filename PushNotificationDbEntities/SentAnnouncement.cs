using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    //Cross Table
    public class SentAnnouncement: Entity
    {
        public int AnnouncementId { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role{ get; set; }
        public virtual Announcement Announcement { get; set; }
    }
}
