using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PezeshkPlus.Models.CustomAttribute;
using System.Web.Mvc;

namespace PezeshkPlus.Models.CustomModel
{
    public class DoctorProfile
    {
        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[(a-z)|(A-Z)|(\u0600-\u06FF\s)]{3,20}$", ErrorMessage = "بايد حداقل 3 و حداکثر 20 کارکتر از حروف باشد")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[(a-z)|(A-Z)|(\u0600-\u06FF\s)]{3,20}$", ErrorMessage = "بايد حداقل 3 و حداکثر 20 کارکتر از حروف باشد")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"\d{10}", ErrorMessage = "کد ملي بايد 10 رقم باشد")]
        public string NationalID { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[+]?\d{11,12}$", ErrorMessage = "شماره موبايل بايد عددي بين 11 الي 13 رقم باشد")]
        public string PhoneNumber { get; set; }

        [Remote("PasswordValidation", "Doctor", HttpMethod = "POST")]
        [RegularExpression(@"^(\w|-|_|\(|\)){8,20}$", ErrorMessage = "پسورد بايد 8 الي 20 کارکتر شامل اعداد يا حروف انگليسي يا کارکتر هاي -و_و(و) باشد")]
        public string Password { get; set; }

        [RegularExpression(@"^(\w|-|_|\(|\)){8,20}$", ErrorMessage = "پسورد بايد 8 الي 20 کارکتر شامل اعداد يا حروف انگليسي يا کارکتر هاي -و_و(و) باشد")]
        public string NewPassword { get; set; }

        [RegularExpression(@"^(\w|-|_|\(|\)){8,20}$", ErrorMessage = "پسورد بايد 8 الي 20 کارکتر شامل اعداد يا حروف انگليسي يا کارکتر هاي -و_و(و) باشد")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "اين پسورد با پسورد بالا همخواني ندارد!")]
        public string RepeatNewPassword { get; set; }

        [RegularExpression(@"^[+]?\d{4,12}$", ErrorMessage = "شماره تلفن بايد عددي بين 4 الي 13 رقم باشد")]
        public string ClinicPhone { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[\u0600-\u06FF\s]{3,30}$", ErrorMessage = "لطفا فارسي تايپ کنيد")]
        public string Province { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[(\u0600-\u06FF)]+[\u0600-\u06FF\s]{3,30}$", ErrorMessage = "لطفا فارسي تايپ کنيد")]
        public string City { get; set; }

        [RegularExpression(@"^[(\u0600-\u06FF)]+[\u0600-\u06FF\s]{3,100}$", ErrorMessage = "لطفا فارسي تايپ کنيد")]
        public string Address { get; set; }

        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[(\u0600-\u06FF\s)|(\-)|(\d)|(,)|(،)]{3,100}$", ErrorMessage = "لطفا فارسي تايپ کنيد - کارکتر های مجاز => - ، , اعداد و حروف فارسی")]
        public string WorkTime { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        public string MedicalField { get; set; }

        public HttpPostedFileBase Picture { get; set; }

        public Search SearchModel { get; set; }
    }
}