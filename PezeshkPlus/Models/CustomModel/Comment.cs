using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PezeshkPlus.Models.CustomModel
{
    public class Comment
    {
        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[(a-z)|(A-Z)|(\u0600-\u06FF\s)]{1,40}$", ErrorMessage = "حداکثر 40 کارکتر از حروف انگلیسی و فارسی")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فیلد اجباری است")]
        [StringLength(4000, ErrorMessage = "حداکثر 4000 کارکتر مجاز است")]
        public string Text { get; set; }

        public byte Rate { get; set; }
        public Search SearchModel { get; set; }
    }
}