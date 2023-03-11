using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class CleanScheduleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string? _userId;

        public CleanScheduleController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var allCleanScheduleList = _db.CleanSchedules.Select(x=> new
            {
                x.Id,
                TenantId =  x.TenantId.ToString().ToLower(),
                CreatedBy = x.CreatedBy.ToString().ToLower(),
                x.StartDate,
                x.EndDate,
            }).ToList();
            var cleanScheduleList = (from cs in allCleanScheduleList
                                     join us in _db.ApplicationUsers on cs.TenantId equals us.Id
                                     join uss in _db.ApplicationUsers on cs.CreatedBy equals uss.Id
                                     select new
                                     {
                                         Id = cs.Id,
                                         StartDate = cs.StartDate.ToShortDateString(),
                                         EndDate = cs.EndDate.ToShortDateString(),
                                         Tenant = us.FirstName + " " + us.LastName,
                                         CreatedBy = uss.FirstName + " " + uss.LastName,
                                     }).ToList();

            ViewData["CleanSchedule"] = cleanScheduleList;
            return View();
        }



    }
}
