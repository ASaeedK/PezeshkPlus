using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PezeshkPlus.Models.CustomModel
{
    public class Search
    {
        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[(a-z)|(A-Z)|(\u0600-\u06FF\s)]{1,40}$", ErrorMessage = "حداکثر 40 کارکتر از حروف انگلیسی و فارسی")]
        public string Name { get; set; }
        public string Province { get; set; }
        public string City { get; set; }

        [RegularExpression(@"^[(a-z)|(A-Z)|(\u0600-\u06FF)]+[(a-z)|(A-Z)|(\u0600-\u06FF\s)]{1,40}$")]
        public string MedicalFieldName { get; set; }
    }
}