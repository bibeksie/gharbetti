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
                                     join ro in _db.Rooms on au.RoomId equals ro.Id
                                     join hr in _db.HouseRooms on ro.Id equals hr.RoomId
                                     join h in _db.Houses on hr.HouseId equals h.Id
                                     where r.Name == StaticDetail.Role_PendingTenant
                                     select new { 
                                         au.FirstName,
                                         au.MiddleName,
                                         au.LastName,
                                         au.PhoneNumber,
                                         au.MobileNumber, 
                                         au.Email,
                                         au.Dob,
                                         au.AddressLine1,
                                         au.AddressLine2,
                                         au.AddressLine3,
                                         au.PostalCode,
                                         au.Country,
                                         au.County,
                                         RoomName = h.Name + "-> " + ro.RoomNo,
                                         au.StayLength,
                                         au.Identification,
                                         au.PhotoId,
                                         au.Id
                                     }).ToList();


            ViewData["Approve"] = pendingTenantList;
            return View();
        }  

        

    }
}
