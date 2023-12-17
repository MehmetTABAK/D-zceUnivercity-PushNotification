using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using PushNotificationWeb.Models;
using PushNotificationDataAccess;
using PushNotificationDbEntities;
using Microsoft.EntityFrameworkCore;
using PushNotificationDbEntities.DTOs.User;
using Microsoft.AspNetCore.Authorization;

namespace PushNotificationWeb.Controllers
{
    public class UserController : Controller
    {
        PushNotificationContext context = new(); //PushNotificationContext, veritabanı işlemleri için kullanılacak olup "context" adında bir değişken oluşturulur ve bu değişken, PushNotificationContext sınıfının bir örneğiyle doldurulur.Yani, PushNotificationContext sınıfından bir nesne oluşturularak, veritabanı işlemleri için gerekli bağlam hazırlanmış olur.

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index() //Admin panelinde tüm kullanıcıları ve bilgilerini görmemizi sağlar.
        {
            List<GetUserDTO> users = context.Users.Include(w => w.Roles).ThenInclude(w => w.Role).ToList().Select(user => //Tüm kullanıcıları ve onlara ait rolleri çekmek için kullanılır.
            new GetUserDTO //Her bir kullanıcı için DTO (Data Transfer Object) oluşturulur.
            {
                Id = user.Id, // Kullanıcının ID'si
                FirstName = user.FirstName, // Kullanıcının Adı
                LastName = user.LastName, // Kullanıcının Soyadı
                Email = user.Email, // Kullanıcının e-posta adresi
                PhoneNumber = user.PhoneNumber, // Kullanıcının telefon numarası
                DeviceToken = user.DeviceToken, // Kullanıcının cihaz tokenı
                RoleNames = user.Roles != null && user.Roles.Any() ? string.Join(", ", user.Roles.Select(w => w.Role?.Name ?? "")) : string.Empty, //Kullanıcının rolleri varsa virgülle ayrılarak birleştirilir, yoksa boş bir string atanır.
            }
            ).ToList(); //DTO'lar bir listeye dönüştürülür.

            return View(users); //DTO'lar içeren liste, View'a geçirilir ve Index view'ına yönlendirilir.
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Update(int id) //Admin panelindeki seçilen kullanıcıyı güncellememiz için gereken boş formun açılmasını sağlar.
        {
            var user = await context.Users.FirstOrDefaultAsync(w => w.Id == id); //Belirli bir kullanıcı Id'sine sahip olan kullanıcıyı çekmek için kullanılır.
            return View(user); //Kullanıcıyı içeren view'e yönlendirilir.
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserDTO us) //Admin panelindeki seçilen kullanıcıyı güncellememiz için gereken boş formun doldurulduktan sonra kaydedilmesini sağlar.
        {
            var userupdate = await context.Users.FirstOrDefaultAsync(w => w.Id == us.Id); //Belirli bir kullanıcı ID'sine sahip olan kullanıcıyı çekmek için kullanılır.
            userupdate.FirstName = us.FirstName; //Yeni girilen ismi alır.
            userupdate.LastName = us.LastName; //Yeni girilen soyismi alır.
            userupdate.Email = us.Email; //Yeni girilen e-posta adresini alır.
            userupdate.PhoneNumber = us.PhoneNumber; //Yeni girilen telefon numarasını alır.
            userupdate.DeviceToken = us.DeviceToken; //Yeni girilen cihaz tokenını alır.

            var selectedRole = context.UserRoleRelation.FirstOrDefault(w => w.UserId == userupdate.Id)!; //Kullanıcının rol bilgisini çekiyoruz."context.UserRoleRelation" ile kullanıcı ve rol ilişkilerini içeren tabloya erişilir.FirstOrDefault metodu belirli bir koşulu sağlayan ilk öğeyi getirir. Burada, kullanıcının Id'si ile eşleşen rol bilgisini çeker.
            selectedRole.RoleId= us.RoleId; //Yeni seçilen rol bilgisini alır.
            context.Update(selectedRole); //Güncelleme veritabanına eklenir.

            context.SaveChanges(); //Değişiklikler veritabanına kaydedilir.
            return RedirectToAction("Index"); //Index sayfasına yönlendirilir.
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DropdownRole(string name) //Admin panelindeki seçilen kullanıcıyı güncellememiz için gereken formun içine görev seçebilmemiz için partial view eklememizi sağlar.
        {
            ViewData["Name"] = name; //ViewBag aracılığıyla View'da kullanılacak olan "Name" değerini atarız.
            List<Role> roles = context.Roles.ToList(); //Tüm rolleri içeren Roles tablosu veritabanından çekilir ve bir Role listesi oluşturulur.
            return PartialView("Partials/_RoleDropdown", roles); //Partial View olan "_RoleDropdown.cshtml" dosyasına, rol listesiyle birlikte yönlendirme yapılır.
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id) //Admin paneindeki seçilen kullanıcıyı silmemmizi sağlar.
        {
            var user = await context.Users.FirstOrDefaultAsync(w => w.Id == id); //Verilen Id'ye sahip kullanıcı veritabanından bulunur.
            context.Users.Remove(user); //Kullanıcı silinir.
            context.SaveChanges(); //Değişiklikler veritabanına kaydedilir.
            return RedirectToAction("Index"); //Index sayfasına yönlendirme yapılır.
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DropdownUsers(string name) //KullanıcıKayıt ve AdminGiriş panelindeki kullanıcı kayıt işlemi için gereken formun içine görev seçebilmemiz için partial view eklememizi sağlar.
        {
            ViewData["Name"] = name; //ViewBag aracılığıyla View'da kullanılacak olan "Name" değerini atarız.
            List<Role> roles = context.Roles.ToList(); //Tüm rolleri içeren Roles tablosu veritabanından çekilir ve bir Role listesi oluşturulur.
            roles = roles.Where(r => r.Name != "Admin").ToList(); //"Admin" rolü hariç tutularak filtrelenen rollerin yeni bir listesi oluşturulur.
            return PartialView("Partials/_Dropdown", roles); //Partial View olan "_Dropdown.cshtml" dosyasına, rol listesiyle birlikte yönlendirme yapılır.
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DropdownEmail(string name) //Admin panelindeki e-posta gönderme işlemini yaparken gereken formun içine kullanıcı seçebilmemiz için partial view eklememizi sağlar.
        {
            ViewData["Name"] = name; //ViewBag aracılığıyla View'da kullanılacak olan "Name" değerini atarız.
            List<User> users = context.Users.ToList(); //Tüm rolleri içeren Roles tablosu veritabanından çekilir ve bir Role listesi oluşturulur.
            return PartialView("Partials/_EmailDropdown", users); //Partial View olan "_EmailDropdown.cshtml" dosyasına, kullanıcı listesiyle birlikte yönlendirme yapılır.
        }
    }
}
