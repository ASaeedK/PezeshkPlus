using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PezeshkPlus.Models.CustomModel
{
    public class AdminLogin
    {
        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^[_A-Za-z0-9-\+]+(\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,4})$", ErrorMessage = "الگوي ايميل صحيح نيست => example@gmail.com")]
        [StringLength(70, ErrorMessage = "لطفا ايميل حداکثر 70 کارکتر باشد")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "فيلد اجباري است")]
        [RegularExpression(@"^(\w|-|_|\(|\)){8,20}$", ErrorMessage = "پسورد بايد 8 الي 20 کارکتر شامل اعداد يا حروف انگليسي يا کارکتر هاي -و_و(و) باشد")]
        public string Password { get; set; }
    }
}