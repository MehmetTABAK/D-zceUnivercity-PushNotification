using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    public class User: Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set;}
        public long PhoneNumber { get; set;}
        public string DeviceToken { get; set;}
        public virtual ICollection<UserPassword> UserPasswords { get; set;}
        public virtual ICollection<DirectAnnouncement> DirectAnnouncements { get; set;}
        public virtual ICollection<UserRoleRelation> Roles { get; set;}
        public virtual ICollection<SentAnnouncement> Announcements { get; set;}
    }
}
