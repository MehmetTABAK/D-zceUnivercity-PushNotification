using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    public class UserPassword: Entity //Entity sınıfından türettiğimiz için Entity sınıfı içerisindeki tüm propertyler bu sınıfa da dahildir.
    {
        public string Password { get; set; } //"Password" adında string bir property tanımlanıyor.Kullanıcının şifresini temsil eder.
        public int UserId { get; set; } //"UserId" adında int bir property tanımlanıyor.Kullanıcının kimliğini temsil eder ve bu kimlik, UserPassword sınıfının User özelliği ile ilişkilidir.
        public virtual User User { get; set; } //"User" adında bir User property tanımlanıyor.Bu özellik, UserPassword sınıfının ilişkilendiği kullanıcıyı temsil eder.
    }
}
