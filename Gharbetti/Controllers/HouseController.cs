using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class HouseController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HouseController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var houseList = _db.Houses.ToList();
            ViewData["House"] = houseList;
            return View();
        }  

        

    }
}
