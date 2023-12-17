using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushNotificationDataAccess;
using PushNotificationDbEntities;

// Bu katman siteden al�nan (sitede payla��lan) duyurular� mobil cihaz�m�zda listeleyerek g�r�nt�lememiz sa�lamaktad�r.

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly PushNotificationContext _context; //PushNotificationDataAccess katman�nda bulunan PushNotificationDataAccess tipinde bir alan tan�ml�yoruz.

    public ProductsController() //ProductsController s�n�f�n�n parametresiz yap�c� metodu.
    {
        _context = new PushNotificationContext(); //_context ad�nda yeni bir PushNotificationDataAccess nesnesi tan�ml�yoruz. Bu da bize veritaban�ndaki bilgilere eri�memiz sa�layacak.
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts() //Asenkron �al��an bir Task<IActionResult> d�nd�ren "GetProducts" ad�nda bir metot.
    {
        var products = await _context.Announcements.ToListAsync(); //_context i�indeki Announcements veritaban� tablosundaki t�m kay�tlar� liste olarak �ekiyoruz.

        return Ok(products); //Elde edilen listeyi HTTP 200 OK durum kodu ile birlikte yan�t olarak d�nd�r�l�yoruz.
    }
}