using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class PaymentModeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PaymentModeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var paymentModeList = _db.PaymentModes.ToList();
            ViewData["PaymentMode"] = paymentModeList;
            return View();
        }  

        

    }
}
