using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    public class UserPassword: Entity
    {
        public string Password { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
