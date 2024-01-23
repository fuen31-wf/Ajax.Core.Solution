using Ajax.Promise.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ajax.Promise.Controllers
{
    public class HomeWork2Controller : Controller
    {
        private readonly MyDBContext _dbContext;

        public HomeWork2Controller(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NameCompare(string name)
        {
            var isExist = _dbContext.Members.Any(m => m.Name == name);
            return Content(isExist.ToString());
        }

        [HttpPost]
        public IActionResult Register(string name, string email, string age)
        {
            var result = $"Hello {name}, {age} 歲了，電子郵件是 {email}.";
            return Content(result);
        }
    }
}
