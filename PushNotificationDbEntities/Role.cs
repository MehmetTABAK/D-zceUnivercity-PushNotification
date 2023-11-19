using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    public class Role : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<UserRoleRelation> Users { get; set; }
        public virtual ICollection<SentAnnouncement> Announcements { get; set; }
    }
}
