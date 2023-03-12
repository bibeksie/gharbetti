using Gharbetti.Data;
using Gharbetti.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gharbetti.Controllers
{
    [Route("[controller]")]
    public class ExpenseTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ExpenseTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var expenseTypeList = _db.ExpenseTypes.ToList();
            ViewData["ExpenseType"] = expenseTypeList;
            return View();
        }  

        

    }
}
