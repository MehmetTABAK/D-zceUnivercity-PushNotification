using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushNotificationDataAccess;
using PushNotificationDbEntities;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly PushNotificationContext _context; 

    public ProductsController()
    {
        _context = new PushNotificationContext();
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Announcements.ToListAsync();
        return Ok(products);
    }
}