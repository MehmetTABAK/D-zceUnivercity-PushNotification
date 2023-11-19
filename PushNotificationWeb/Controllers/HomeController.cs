using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushNotificationDataAccess;
using PushNotificationEntites;
using PushNotificationWeb.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;

namespace PushNotificationWeb.Controllers
{
    public class HomeController : Controller
    {
        PushNotificationContext context = new();
        public IActionResult Index() 
        {
            List<PushNotificationDbEntities.Announcement> announcements = context.Announcements.ToList();
            return View(announcements);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Announcement()
        {
            List<PushNotificationDbEntities.Announcement> announcements = context.Announcements.ToList();
            return View(announcements);
        }

        public IActionResult AnnouncementDetails(int id)
        {
            var anno = context.Announcements.Where(w => w.Id == id).ToList();
            return View(anno);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}