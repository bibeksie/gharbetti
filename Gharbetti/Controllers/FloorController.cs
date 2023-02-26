using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class FloorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FloorController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var floorList = _db.Floors.ToList();
            ViewData["Floor"] = floorList;
            return View();
        }  

        

    }
}
