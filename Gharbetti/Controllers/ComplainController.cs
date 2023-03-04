using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class ComplainController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;

        public ComplainController(ApplicationDbContext db,IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var complainList = _db.Complains.Where(x => x.TenantId ==  Guid.Parse(_userId)).ToList();
            ViewData["Complain"] = complainList;
            return View();
        }  

        

    }
}
