using Gharbetti.Areas.Identity.Pages.Account;
using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Gharbetti.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string? _userId;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, ApplicationDbContext db,
            SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _logger = logger;
            _signInManager = signInManager;

            if (httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }



        }

        public async Task<IActionResult> Index()
        {
            //var user = await _userManager.GetUserAsync(User);
            //var userDetail = _db.ApplicationUsers.FirstOrDefault(x => x.Id == _userId);
            //if (userDetail != null)
            //{
            if (this.User.IsInRole("pendingtenant"))
            {
                //await _signInManager.SignOutAsync();
                //_logger.LogInformation("User logged out.");
                var userData = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == _userId);
                if (userData != null)
                {
                    ViewData["Remarks"] = userData.ApproveRemarks == null ? "Wait for LandLord to Respond" : userData.ApproveRemarks;
                }
                return View();
                //return Response.Redirect(Url.ToString());
            }

            //}
            ViewData["Remarks"] = "";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}