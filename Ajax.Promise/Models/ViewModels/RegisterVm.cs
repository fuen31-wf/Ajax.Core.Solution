using System.ComponentModel.DataAnnotations;

namespace Ajax.Promise.Models.ViewModels
{
    public class RegisterVm
    {
        [Display(Name = "姓名:")]
        [Required]
        [StringLength(30)]
        public string? name { get; set; }

        [Display(Name = "電子郵件:")]
        [Required]
        [StringLength(256)]
        [EmailAddress]
        public string? email { get; set; }

        [Display(Name = "密碼:")]
        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string? password { get; set; }

        [Display(Name = "密碼確認:")]
        [Required]
        [StringLength(50)]
        [Compare(nameof(password))]
        [DataType(DataType.Password)]
        public string? confirmPassword { get; set; }

        [Display(Name = "年紀:")]
        [Required]
        public string? age { get; set; }

        [Display(Name = "頭像:")]
        public IFormFile? imgForm { get; set; }
    }
}
