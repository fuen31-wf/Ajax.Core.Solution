using Ajax.Promise.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ajax.Promise.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _dbContext;

        public ApiController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cities()
        {
            var cities = _dbContext.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }
    }
}
