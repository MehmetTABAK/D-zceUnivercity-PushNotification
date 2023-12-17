using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    public class User: Entity //Entity sınıfından türettiğimiz için Entity sınıfı içerisindeki tüm propertyler bu sınıfa da dahildir.
    {
        public string FirstName { get; set; } //"FirstName" adında string bir property tanımlanıyoruz.Kullanıcının adını temsil eder.
        public string LastName { get; set; } //"LastName" adında string bir property tanımlanıyoruz.Kullanıcının soyadını temsil eder.
        public string Email { get; set; } //"Email" adında string bir property tanımlanıyoruz.Kullanıcının e-posta adresini temsil eder.
        public long PhoneNumber { get; set; } //"PhoneNumber" adında long bir property tanımlanıyoruz.Kullanıcının telefon numarasını temsil eder.
        public string DeviceToken { get; set; } //"DeviceToken" adında string bir property tanımlanıyoruz.Kullanıcının cihazına özgü bir belirteç (token) temsil eder.
        public virtual ICollection<UserPassword> UserPasswords { get; set; } //"UserPasswords" adında bir UserPassword koleksiyon property tanımlanıyoruz.Bu, kullanıcının şifre bilgilerini içeren bir koleksiyonu temsil eder.
        public virtual ICollection<DirectAnnouncement> DirectAnnouncements { get; set; } //"DirectAnnouncements" adında bir DirectAnnouncement koleksiyon property tanımlanıyoruz.Bu, kullanıcının aldığı doğrudan bildirimleri temsil eden bir koleksiyonu temsil eder.
        public virtual ICollection<UserRoleRelation> Roles { get; set; } //"Roles" adında bir UserRoleRelation koleksiyon property tanımlanıyoruz.Bu, kullanıcının sahip olduğu rolleri temsil eden bir koleksiyonu temsil eder.
        public virtual ICollection<SentAnnouncement> Announcements { get; set; } //"Announcements" adında bir SentAnnouncement koleksiyon property tanımlanıyoruz.Bu, kullanıcının gönderilmiş bildirimleri temsil eden bir koleksiyonu temsil eder.
    }
}
