using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities.DTOs.User
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public string DeviceToken { get; set; }
        public int RoleId { get; set; }
    }
}
