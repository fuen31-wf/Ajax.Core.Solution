using Ajax.Promise.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Ajax.Promise.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _dbContext;

        public ApiController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //[HttpPost]
        public IActionResult Index()
        {
            System.Threading.Thread.Sleep(5000);
            //return Content("Allen, Bllen");
            //return Content("<h2>Allen, Bllen</h2>", "text/html");
            //return Content("<h2>Allen, Bllen, 王福</h2>", "text/html", Encoding.UTF8);
            return Content("Allen, Bllen, 王福");
        }

        //[HttpPost]
        public IActionResult First()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RegisterResult(string name, int age=26)
        {
            if (string.IsNullOrEmpty(name)) name = "Guest";
            return Content($"Hello {name}, You are {age} years old.");
        }

        public IActionResult Address() { 
            return View();
        }
        
        public IActionResult Cities()
        {
            var cities = _dbContext.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }

        public IActionResult Dists(string city)
        {
            var dists = _dbContext.Addresses.Where(a => a.City==city).Select(a=>a.SiteId).Distinct();
            return Json(dists);
        }

        public IActionResult Roads(string dist)
        {
            var roads = _dbContext.Addresses.Where(a => a.SiteId == dist).Select(a=>a.Road).Distinct();
            return Json(roads);
        }

        public IActionResult Avatar(int id= 1)
        {
            Member? member = _dbContext.Members.Find(id);
            if(member == null) return NotFound();

            byte[] img = member.FileData;
            return File(img, "image/jpeg");
        }
    }
}
