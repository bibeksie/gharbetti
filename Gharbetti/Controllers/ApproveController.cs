using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class ApproveController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApproveController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var pendingTenantList = (from au in _db.ApplicationUsers
                            join ur in _db.UserRoles on au.Id equals ur.UserId
                            join r in _db.Roles on ur.RoleId equals r.Id
                            where r.Name == StaticDetail.Role_PendingTenant
                            select au).ToList();


            ViewData["Approve"] = pendingTenantList;
            return View();
        }  

        

    }
}
