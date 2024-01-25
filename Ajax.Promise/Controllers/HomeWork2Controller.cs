using Ajax.Promise.Models;
using Ajax.Promise.Models.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Ajax.Promise.Controllers
{
    public class HomeWork2Controller : Controller
    {
        private readonly MyDBContext _dbContext;
        private readonly IWebHostEnvironment _host;

        public HomeWork2Controller(MyDBContext dbContext, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _host = host;
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

        public IActionResult EmailCompare(string email)
        {
            var isExist = _dbContext.Members.Any(m => m.Email == email);
            return Content(isExist.ToString());
        }

        //[HttpPost]
        //public IActionResult Register(RegisterVm vm)
        //{

        //    string uploadPath = $@"D:\Joey\test\Ajax.Core.Solution\Ajax.Promise\wwwroot\upLoad\{vm.imgForm?.Name}";

        //    using (var fileStream = new FileStream(uploadPath, FileMode.Create))
        //    {

        //    }
        //    var result = $"Hello {vm.name}, {vm.age} 歲了，電子郵件是 {vm.email}.";


        //    return Content(result);
        //}


        [HttpPost]
        public IActionResult Register(/*Member member,*/ RegisterVm vm)
        {
            if (!ModelState.IsValid) return Content("新增失敗");

            //todo 檔案存在的處理
            //todo 限制上傳的檔案類型
            //todo 限制上傳的檔案大小

            Member member = new Member();

            if (vm.imgForm == null) goto Skip;

            string fileName = vm.imgForm.FileName;

            //組出檔案上傳的實際路徑
            string uploadPath = Path.Combine(_host.WebRootPath, "upload", fileName);

            //檔案上傳
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                vm.imgForm.CopyTo(fileStream);
            }

            //轉成二進制
            byte[]? imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                vm.imgForm.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }

            member.FileName = fileName;
            member.FileData = imgByte;

        Skip:

            //密碼加鹽          
            // 設定 PBKDF2 參數
            int iterations = 10000; // 重複計算次數兩面刃
            int numBytesRequested = 32; // 產生 32 位元組
            //產生鹽
            byte[] salt = GenerateRandomSalt();
            // 使用 PBKDF2 加密
            byte[] hashedPassword = KeyDerivation.Pbkdf2(vm.password, salt, KeyDerivationPrf.HMACSHA512, iterations, numBytesRequested);

            member.Name = vm.name;
            member.Age = int.TryParse(vm.age, out int value) ? value : default;
            member.Email = vm.email;

            // salt 和 Password 可以被儲存為位元組陣列或轉換成十六進位字串
            member.Salt = Convert.ToBase64String(salt);
            member.Password = Convert.ToBase64String(hashedPassword);

            _dbContext.Members.Add(member);
            _dbContext.SaveChanges();

            return Content("新增成功");
        }

        // 產生鹽
        private static byte[] GenerateRandomSalt()
        {
            byte[] salt = new byte[16]; // 16位元組的加鹽
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
