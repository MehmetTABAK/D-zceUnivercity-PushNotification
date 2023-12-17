using Microsoft.EntityFrameworkCore;
using PushNotificationDbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Veritabanı bağlantısı kurarak tabloları oluşturmamızı sağlayan katman. 

namespace PushNotificationDataAccess
{
    public class PushNotificationContext : DbContext //DbContext sınıfından türetiyoruz. İçeriğine yazdığımız her bir DbSet veritabanındaki bir tabloya karşılık gelir.
    {
        public DbSet<User> Users { get; set; } //Veritabanındaki "Users" tablosunu temsil eder.
        public DbSet<UserPassword> UserPasswords { get; set; } //Veritabanındaki "UserPasswords" tablosunu temsil eder.
        public DbSet<DirectAnnouncement> DirectAnnouncements { get; set; } //Veritabanındaki "DirectAnnouncements" tablosunu temsil eder.
        public DbSet<Role> Roles { get; set; } //Veritabanındaki "Roles" tablosunu temsil eder.
        public DbSet<Announcement> Announcements { get; set; } //Veritabanındaki "Announcements" tablosunu temsil eder.
        public DbSet<UserRoleRelation> UserRoleRelation { get; set; } //Veritabanındaki "UserRoleRelation" tablosunu temsil eder.
        public DbSet<SentAnnouncement> SentAnnouncement { get; set; } //Veritabanındaki "SentAnnouncement" tablosunu temsil eder.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //DbContext sınıfının "OnConfiguring" metodu ile DbContext'in veritabanına bağlanma konfigürasyonunu ayarlıyoruz.
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=PushNotification"); //"UseSqlServer" metodu ile SQL Server veritabanına bağlanma konfigürasyonunu belirliyoruz.
        }
    }
}
