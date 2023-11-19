using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushNotificationDataAccess;
using PushNotificationDbEntities;
using PushNotificationDbEntities.DTOs;
using PushNotificationEntites;
using System.Text;

namespace PushNotificationWeb.Controllers
{
    public class AnnouncementController : Controller
    {
        PushNotificationContext context = new();

        [HttpGet]
        public IActionResult Index()
        {
            List<PushNotificationDbEntities.Announcement> announcements = context.Announcements.ToList();
            return View(announcements);
        }

        [HttpGet]
        public IActionResult AddNotification()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNotification(PushNotificationDbEntities.Announcement model)
        {
            PushNotificationContext context = new();

            if (string.IsNullOrEmpty(model.PhotoUrl))
            {
                model.PhotoUrl = "https://i.hizliresim.com/b5t2vap.jpg";
            }

            PushNotificationDbEntities.Announcement anno = new()
            {
                Id = model.Id,
                Title = model.Title,
                Body = model.Body,
                PhotoUrl = model.PhotoUrl,
            };

            context.Add(anno);

            try
            {
                context.SaveChanges();
                ViewBag.Message = "Duyuru paylaşımı başarılı.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Duyuru paylaşım hatası: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var anno = await context.Announcements.FirstOrDefaultAsync(w => w.Id == id);
            context.Announcements.Remove(anno);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var anno = await context.Announcements.FirstOrDefaultAsync(w => w.Id == id);
            return View(anno);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PushNotificationDbEntities.Announcement us)
        {
            if (string.IsNullOrEmpty(us.PhotoUrl))
            {
                us.PhotoUrl = "https://i.hizliresim.com/b5t2vap.jpg";
            }

            var annoupdate = await context.Announcements.FirstOrDefaultAsync(w => w.Id == us.Id);
            annoupdate.Title = us.Title;
            annoupdate.Body = us.Body;
            annoupdate.PhotoUrl = us.PhotoUrl;

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ShareToGroup(int id)
        {
            var anno = context.Announcements.FirstOrDefault(w => w.Id == id);
            return View(anno);
        }

        [HttpGet]
        public async Task<IActionResult> DropdownGroup(string name)
        {
            ViewData["Name"] = name;
            List<Role> roles = context.Roles.ToList();
            roles = roles.Where(r => r.Name != "Admin").ToList();
            return PartialView("Partials/_RoleDropdown", roles);
        }

        [HttpPost]
        public async Task<IActionResult> ShareToGroup(FcmRequestModel model, int roleId, SentAnnouncement sentAnno)
        {
            try
            {
                string apiUrl = "https://localhost:7051/api/Api/sendNotification";

                var usersInRole = context.UserRoleRelation.Where(ur => ur.RoleId == roleId).Select(ur => ur.User.DeviceToken).ToList();

                if (usersInRole.Any())
                {
                    HttpClient httpClient = new HttpClient();

                    foreach (var deviceToken in usersInRole)
                    {
                        var requestData = new FcmRequestModel
                        {
                            To = deviceToken,
                            Notification = new NotificationDataModel
                            {
                                Title = model.Notification.Title,
                                Body = model.Notification.Body
                            }
                        };

                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync(apiUrl, content);

                        if (!response.IsSuccessStatusCode)
                        {
                            ViewBag.ResponseMessage = "Veri gönderme başarısız. HTTP kodu: " + response.StatusCode;
                            return RedirectToAction("Index", "SentAnnouncement");
                        }
                    }

                    ViewBag.ResponseMessage = "Veri başarıyla gönderildi.";

                    var sentAnnouncement = new SentAnnouncement
                    {
                        AnnouncementId = sentAnno.AnnouncementId,
                        RoleId = sentAnno.RoleId,
                    };

                    context.SentAnnouncement.Add(sentAnnouncement);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResponseMessage = "Hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Index", "Announcement");
        }
    }
}
