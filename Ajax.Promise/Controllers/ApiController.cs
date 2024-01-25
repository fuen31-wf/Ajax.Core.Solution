using Ajax.Promise.Models;
using Ajax.Promise.Models.JsonModels;
using Ajax.Promise.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Linq;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public IActionResult RegisterResult(RegisterVm vm)
        {
            if (string.IsNullOrEmpty(vm.name)) vm.name = "Guest";
            return Content($"Hello {vm.name}, You are {vm.age} years old, email: {vm.email}");
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

        public IActionResult Spots()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Spots([FromBody]SpotsCriteriaVm cr)
        {
            var query = _dbContext.SpotImagesSpots.AsNoTracking();

            query = cr.categoryId == 0 ?
                query : query.Where(s => s.CategoryId == cr.categoryId);

            query = string.IsNullOrEmpty(cr.keyword) ?
                query : query.Where(s => s.SpotDescription.Contains(cr.keyword) ||
                                         s.Address.Contains(cr.keyword));

            switch (cr.sortBy)
            {
                default:
                    query = query.OrderBy(s => s.SpotId);
                    break;
                case "Address":
                    query = query.OrderBy(s => s.Address);
                    break;
                case "DateCreated":
                    query = query.OrderBy(s => s.DateCreated);
                    break;
            }

            if (cr.sortType == "desc")
            {
                query = query.Reverse();
            }

            int count = query.Count();

            int page = cr.page ?? 1;
            int pageSize = cr.pageSize ?? 9;

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            query = query.Skip((page - 1) * (pageSize)).Take(pageSize);

            var data = query.ToList();

            var spotsJm = new SpotsJm
            {
                totalPages = totalPages,
                currentPage = page,
                spots = data,
                totalCount = count
            };


            //if(cr.sortType == "desc")

            return Json(spotsJm);
        }

        public IActionResult AutoComplete()
        {
            return View();
        }

        public IActionResult SpotTitle(string keyword)
        {
            var spts = _dbContext.Spots.Where(s => s.SpotTitle.Contains(keyword))
                                       .Select(s=>s.SpotTitle);
            return Json(spts);
        }

        public IActionResult ApiTest()
        {

            return View();
        }
    }
}
