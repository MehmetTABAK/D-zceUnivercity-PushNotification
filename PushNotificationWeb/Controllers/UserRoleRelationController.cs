using Microsoft.AspNetCore.Mvc;
using PushNotificationWeb.Models;
using System.Net.Mail;
using PushNotificationDbEntities;
using PushNotificationDataAccess;
using System.Net.Http;
using PushNotificationDbEntities.DTOs.User;

namespace PushNotificationWeb.Controllers
{
    public class UserRoleRelationController : Controller
    {
        PushNotificationContext context = new();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(CreateUserDTO model)
        {
            PushNotificationContext context = new();

            User users = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DeviceToken = model.DeviceToken,
            };

            context.Add(users);
            context.SaveChanges();
            UserRoleRelation relation = new UserRoleRelation();
            relation.UserId = users.Id;
            relation.RoleId = model.RoleId;

            context.Add(relation);

            try
            {
                context.SaveChanges();
                ViewBag.Message = "Kayıt işlemi başarılı.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Kayıt işlemi hatası: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DropdownRole(string name)
        {
            ViewData["Name"] = name;
            List<Role> roles = context.Roles.ToList();
            roles = roles.Where(r => r.Name != "Admin").ToList();
            return PartialView("Partials/_RoleDropdown", roles);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserPassword us)
        {
            var admin = context.UserPasswords.FirstOrDefault(x => x.UserId == us.UserId && x.Password == us.Password);
            if (admin != null)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
