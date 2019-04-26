using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PezeshkPlus.Models.CustomModel
{
    public class EmailContact
    {
        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[(a-z)|(A-Z)|(\u0600-\u06FF\s)]{1,40}$", ErrorMessage = "حداکثر 40 کارکتر از حروف انگلیسی و فارسی")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[_A-Za-z0-9-\+]+(\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,4})$", ErrorMessage = "الگوي ايميل صحيح نيست => example@gmail.com")]
        [StringLength(70, ErrorMessage = "لطفا ايميل حداکثر 70 کارکتر باشد")]
        public string Email { get; set; }

        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[(a-z)|(A-Z)|(\u0600-\u06FF\s)|(0-9)]{1,40}$", ErrorMessage = "حداکثر 40 کارکتر از حروف انگلیسی و فارسی و اعداد")]
        public string Topic { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [StringLength(4000, ErrorMessage = "حداکثر 4000 کارکتر مجاز است")]
        public string Text { get; set; }
        public Search SearchModel { get; set; }
    }
}