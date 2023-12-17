using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushNotificationDataAccess;
using PushNotificationDbEntities;

// Bu katman siteden alýnan (sitede paylaþýlan) duyurularý mobil cihazýmýzda listeleyerek görüntülememiz saðlamaktadýr.

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly PushNotificationContext _context; //PushNotificationDataAccess katmanýnda bulunan PushNotificationDataAccess tipinde bir alan tanýmlýyoruz.

    public ProductsController() //ProductsController sýnýfýnýn parametresiz yapýcý metodu.
    {
        _context = new PushNotificationContext(); //_context adýnda yeni bir PushNotificationDataAccess nesnesi tanýmlýyoruz. Bu da bize veritabanýndaki bilgilere eriþmemiz saðlayacak.
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts() //Asenkron çalýþan bir Task<IActionResult> döndüren "GetProducts" adýnda bir metot.
    {
        var products = await _context.Announcements.ToListAsync(); //_context içindeki Announcements veritabaný tablosundaki tüm kayýtlarý liste olarak çekiyoruz.

        return Ok(products); //Elde edilen listeyi HTTP 200 OK durum kodu ile birlikte yanýt olarak döndürülüyoruz.
    }
}