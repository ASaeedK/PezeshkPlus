using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PezeshkPlus.Models.CustomAttribute;
using System.Web.Mvc;

namespace PezeshkPlus.Models.CustomModel
{
    public class DoctorRegister
    {
        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[(a-z)|(A-Z)|(\u0600-\u06FF\s)]{2,20}$", ErrorMessage = "بايد حداقل 3 و حداکثر 20 کارکتر از حروف باشد")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[(a-z)|(A-Z)|(\u0600-\u06FF\s)]{3,20}$", ErrorMessage = "بايد حداقل 3 و حداکثر 20 کارکتر از حروف باشد")]
        public string LastName { get; set; }

        [Remote("NationalIDValidation", "Doctor", HttpMethod = "POST")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"\d{10}", ErrorMessage = "کد ملي بايد 10 رقم باشد")]
        public string NationalID { get; set; }

        [Remote("PhoneNumberValidation", "Doctor", HttpMethod = "POST")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[+]?\d{11,12}$", ErrorMessage = "شماره موبايل بايد عددي بين 11 الي 13 رقم باشد")]
        public string PhoneNumber { get; set; }

        [Remote("EmailValidation", "Doctor", HttpMethod = "POST")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[_A-Za-z0-9-\+]+(\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,4})$", ErrorMessage = "الگوي ايميل صحيح نيست => example@gmail.com")]
        [StringLength(70, ErrorMessage = "لطفا ايميل حداکثر 70 کارکتر باشد")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^(\w|-|_|\(|\)){8,20}$", ErrorMessage = "پسورد بايد 8 الي 20 کارکتر شامل اعداد يا حروف انگليسي يا کارکتر هاي -و_و(و) باشد")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^(\w|-|_|\(|\)){8,20}$", ErrorMessage = "پسورد بايد 8 الي 20 کارکتر شامل اعداد يا حروف انگليسي يا کارکتر هاي -و_و(و) باشد")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "اين پسورد با پسورد بالا همخواني ندارد!")]
        public string RepeatPassword { get; set; }
        public System.DateTime RegistrationDate { get; set; }

        [Remote("ClinicPhoneValidation", "Doctor", HttpMethod = "POST")]
        [RegularExpression(@"^[+]?\d{4,12}$", ErrorMessage = "شماره تلفن بايد عددي بين 4 الي 13 رقم باشد")]
        public string ClinicPhone { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[(\u0600-\u06FF)]+[\u0600-\u06FF\s]{3,30}$", ErrorMessage = "لطفا فارسي تايپ کنيد")]
        public string Province { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[(\u0600-\u06FF)]+[\u0600-\u06FF\s]{3,30}$", ErrorMessage = "لطفا فارسي تايپ کنيد")]
        public string City { get; set; }

        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[\u0600-\u06FF\s]{3,100}$", ErrorMessage = "لطفا فارسي تايپ کنيد")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        public string MedicalField { get; set; }

        [RequiredBool(ErrorMessage = "تيک اين قسمت را بايد بزنيد")]
        public bool Agree { get; set; }
    }
}