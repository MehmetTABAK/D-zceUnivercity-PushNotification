using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    public class DirectAnnouncement: Entity //Entity sınıfından türettiğimiz için Entity sınıfı içerisindeki tüm propertyler bu sınıfa da dahildir.
    {
        public int UserId { get; set; } //"UserId" adında int bir property tanımlanıyoruz.Direkt bildirimin ilişkilendiği kullanıcının kimliğini temsil eder.
        public string Title { get; set; } //"Title" adında string bir property tanımlanıyoruz.Direkt bildirimin başlığını temsil eder.
        public string Body { get; set; } //"Body" adında string bir property tanımlanıyoruz.Direkt bildirimin içeriğini temsil eder.
        public virtual User User { get; set; } //"User" adında bir User property tanımlanıyoruz.Direkt bildirimin ilişkilendiği kullanıcıyı temsil eder.
    }
}
