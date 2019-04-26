using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PezeshkPlus.Models.CustomModel
{
    public class RegisterPack
    {
        public DoctorRegister RegisterModel { get; set; }
        public DoctorLogin LoginModel { get; set; }
        public Search SearchModel { get; set; }
    }
}