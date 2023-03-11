using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Gharbetti.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db
            )
        {
            _db = db;
        }

        [HttpGet]
        [Route("gettenant")]
        public async Task<IActionResult> GetTenant()
        {
            var dbTran = _db.Database.BeginTransaction();
            try
            {
                var tenantList = (from au in _db.ApplicationUsers
                                  join ur in _db.UserRoles on au.Id equals ur.UserId
                                  join r in _db.Roles on ur.RoleId equals r.Id
                                  where r.Name == StaticDetail.Role_Tenant
                                  select new
                                  {
                                      au.Id,
                                      Name = au.FirstName + " " + au.LastName,
                                  }).ToList();

                return Ok(new { Status = true, Message = "Sucessfully", Data = tenantList });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = "Error while Changing role" });
            }
        }

    }
}
