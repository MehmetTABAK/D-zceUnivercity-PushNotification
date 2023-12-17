using Microsoft.AspNetCore.Mvc;
using PushNotificationWeb.Models;
using System.Net.Mail;
using PushNotificationDbEntities;
using PushNotificationDataAccess;
using System.Net.Http;
using PushNotificationDbEntities.DTOs.User;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;

namespace PushNotificationWeb.Controllers
{
    public class UserRoleRelationController : Controller
    {
        PushNotificationContext context = new(); //PushNotificationContext, veritabanı işlemleri için kullanılacak olup "context" adında bir değişken oluşturulur ve bu değişken, PushNotificationContext sınıfının bir örneğiyle doldurulur.Yani, PushNotificationContext sınıfından bir nesne oluşturularak, veritabanı işlemleri için gerekli bağlam hazırlanmış olur.


        [HttpGet]
        public IActionResult Index() //Kayıt olma veya giriş yapma panelindeki kayıt olma işlemini yapabilmemiz için gereken boş sayfayı açmamızı sağlar.
        {
            return View(); //Kayıt olma sayfasını gösteren bir sayfa için kullanılır.
        }

        [HttpPost]
        public IActionResult Index(CreateUserDTO model) //Kayıt olma veya giriş yapma panelindeki kayıt olma işlemini yapabilmemiz için gereken boş sayfayı doldurduğumuzda kaydetmemizi sağlar.
        {
            User users = new() //Yeni kullanıcı oluşturulur.
            {
                FirstName = model.FirstName, //Kullanıcının ismini alır.
                LastName = model.LastName, //Kullanıcının soyismini alır.
                Email = model.Email, //Kullanıcının e-posta adresini alır.
                PhoneNumber = model.PhoneNumber, //Kullanıcının telefon numarasını alır.
                DeviceToken = model.DeviceToken, //Kullanıcının cihaz tokenını alır.
            };

            context.Add(users); //Kullanıcıyı veritabanına ekler.
            context.SaveChanges(); //Değişiklikleri (kullanıcıyı) veritabanına kaydeder.

            UserRoleRelation relation = new UserRoleRelation(); //Kullanıcının rol ilişkisi oluşturulur.
            relation.UserId = users.Id; //Yeni oluşturulan kullanıcının Id'si atanır.
            relation.RoleId = model.RoleId; //Kullanıcının atanacak rolün Id'si atanır.

            context.Add(relation); //Kullanıcının rol ilişkisi veritabanına eklenir.
            context.SaveChanges(); //Değişiklikler (rol ilişkisi) veritabanına kaydedilir.

            return RedirectToAction("Index"); //Index sayfasına yönlendirme yapılır.
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DropdownRole(string name) //Kayıt olma veya giriş yapma panelindeki kayıt olma işlemini yapabilmemiz için gereken formun içine görev seçebilmemiz için partial view eklememizi sağlar.
        {
            ViewData["Name"] = name; //ViewBag aracılığıyla View'da kullanılacak olan "Name" değerini atarız.
            List<Role> roles = context.Roles.ToList(); //Tüm rolleri içeren Roles tablosu veritabanından çekilir ve bir Role listesi oluşturulur.
            roles = roles.Where(r => r.Name != "Admin").ToList(); //"Admin" rolü hariç tutularak filtrelenen rollerin yeni bir listesi oluşturulur.
            return PartialView("Partials/_RoleDropdown", roles); //Partial View olan "_RoleDropdown.cshtml" dosyasına, rol listesiyle birlikte yönlendirme yapılır.
        }

        [HttpGet]
        public IActionResult Login() //Kayıt olma veya giriş yapma panelindeki giriş yapma işlemini yapabilmemiz için gereken boş sayfayı açmamızı sağlar.
        {
            ClaimsPrincipal claimUser = HttpContext.User; //Adminin daha önce giriş yapıp yapmadığını kontrol etmek için nesne türetiyoruz.
            if (claimUser.Identity.IsAuthenticated) //Admin daha önce giriş yapmışsa
            {
                return RedirectToAction("Index", "User"); //Çıkış yapmadığı sürece tekrar giriş yapmasına gerek kalmadan admin paneline User controllerın Index actionu yolu ile girebilir.
            }
            return View(); //Giriş yapma sayfasını gösteren bir sayfa için kullanılır.
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserPassword us) //Kayıt olma veya giriş yapma panelindeki giriş yapma işlemini yapabilmemiz için gereken boş sayfayı doldurduğumuzda giriş yapabilmemizi sağlar.
        {       
            var admin = context.UserPasswords.FirstOrDefault(x => x.UserId == us.UserId && x.Password == us.Password); //Veritabanında kullanıcı parolasını kontrol etmek için girilen kullanıcı Id'si ve şifresi ile eşleşen bir kayıt aranır.
            if (admin != null) //Eğer girilen kullanıcı bilgileri ile eşleşen bir kayıt bulunursa, giriş başarılı kabul edilir.
            {
                List<Claim> claims = new List<Claim>() //Adminin kimlik bilgilerini temsil eden bir liste oluşturuluyor.
                {
                    new Claim(ClaimTypes.NameIdentifier, us.UserId.ToString()), //Adminin benzersiz kimlik numarası yani UserId'si
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme); //Adminin kimlik bilgilerini içeren bir ClaimsIdentity oluşturuluyoruz.
                AuthenticationProperties properties = new AuthenticationProperties() //Oturum açma işlemi sırasında kullanılacak özellikleri içeren bir AuthenticationProperties nesnesi oluşturuluyoruz.
                {
                    AllowRefresh = true, //Sayfanın yenilenebilir olup olmadığını belirliyoruz.
                    IsPersistent = true //Adminin tarayıcıyı kapatsa bile oturumun kalıcı olup olmayacağını belirler. Bu değeri true verdiğimiz için oturum açık kalacak.
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties); //Oturum açma işlemi gerçekleştiriliyoruz.

                return RedirectToAction("Index", "User"); //Başarılı giriş durumunda, User controllerdaki Index sayfasına yönlendirilir.Yani artık Admin panelindedir.
            
            }
            else //Eğer eşleşen bir kayıt bulunamazsa
            {
                return RedirectToAction("Index"); //Kullanıcı tekrar giriş sayfasına yönlendirilir.
            }
        }

        public async Task<IActionResult> LogOut() //Adminin çıkış yap butonuna bastıktan sonra çıkış yapabilmesi gerekli işlemi yapıyoruz.
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //Adminin oturumunu sonlandırmak için HttpContext.SignOutAsync metodu kullanılıyoruz. CookieAuthenticationDefaults.AuthenticationScheme, kullanılan kimlik doğrulama şemasını belirtir.

            return RedirectToAction("Index","UserRoleRelation"); //Oturumu kapatınca UserRoleRelation controllerdaki Index actiona gidecek.
        }
    }
}
