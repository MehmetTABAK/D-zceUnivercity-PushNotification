using PushNotificationDbEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDbEntities
{
    //Cross Table
    //Cross Table iki veya daha fazla tablonun birbirleriyle ilişkilendirildiği ve genellikle çoktan çok ilişkileri temsil ettiği bir tür veritabanı tablosudur.Biz burada Roles ve Users tablolarına çoktan çok ilişki kurduruyoruz.
    public class UserRoleRelation: Entity //Entity sınıfından türettiğimiz için Entity sınıfı içerisindeki tüm propertyler bu sınıfa da dahildir.
    {
        public int UserId { get; set; } //"UserId" adında int bir property tanımlanıyoruz.Kullanıcının kimliğini temsil eder ve UserRoleRelation sınıfının User özelliği ile ilişkilidir.
        public int RoleId { get; set; } //"RoleId" adında int bir property tanımlanıyoruz.Rolün kimliğini temsil eder ve UserRoleRelation sınıfının Role özelliği ile ilişkilidir.
        public virtual User User { get; set; } //"User" adında bir User property tanımlanıyoruz.Bu özellik, UserRoleRelation sınıfının ilişkilendiği kullanıcıyı temsil eder.
        public virtual Role Role { get; set; } //"Role" adında bir Role property tanımlanıyoruz.Bu özellik, UserRoleRelation sınıfının ilişkilendiği rolü temsil eder.
    }
}
