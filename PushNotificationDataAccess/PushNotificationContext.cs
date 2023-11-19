using Microsoft.EntityFrameworkCore;
using PushNotificationDbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationDataAccess
{
    public class PushNotificationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }
        public DbSet<DirectAnnouncement> DirectAnnouncements { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<UserRoleRelation> UserRoleRelation { get; set; }
        public DbSet<SentAnnouncement> SentAnnouncement { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=PushNotification");
        }
    }
}
