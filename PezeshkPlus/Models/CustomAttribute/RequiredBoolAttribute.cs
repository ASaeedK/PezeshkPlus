using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PezeshkPlus.Models.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = true)]
    public class RequiredBoolAttribute : ValidationAttribute
    {
        public RequiredBoolAttribute()
            : base()
        {

        }
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value.ToString() == "False") return false;
            return true;
        }
    }
}