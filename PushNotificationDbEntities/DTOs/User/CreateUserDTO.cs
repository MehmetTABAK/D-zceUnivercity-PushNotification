using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities.DTOs.User
{
    public class CreateUserDTO //Burada gerçekleştirmiş olduğumuz olay iki farklı DbEntities sınıfındaki verileri tek sınıfta kullanmak.Yani oluşturacağımız sayfada bizim User sınıfındaki propertylerin yanında Role sınıfındaki RoleId propertysine ihtiyacımız olduğu için bu sınıfı oluşturuyoruz.Herhangi bir view içinde iki farklı model kullanamadığımız için bu yöntemi kullanarak tek modelde iki farklı tablodaki ihtiyacımız olan verilere ulaşabiliyoruz.
    {
        public string FirstName { get; set; } //User sınıfından
        public string LastName { get; set; } //User sınıfından
        public string Email { get; set; } //User sınıfından
        public long PhoneNumber { get; set; } //User sınıfından
        public string DeviceToken { get; set; } //User sınıfından
        public int RoleId { get; set; } //Role sınıfından
    }
}
