using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class RoomTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoomTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var roomList = _db.RoomTypes.ToList();
            ViewData["Room"] = roomList;
            return View();
        }  

        

    }
}
