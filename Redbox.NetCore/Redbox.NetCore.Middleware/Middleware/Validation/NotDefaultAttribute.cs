using System;
using System.ComponentModel.DataAnnotations;

namespace Redbox.NetCore.Middleware.Validation
{
    public class NotDefaultAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not have the default value";

        public NotDefaultAttribute()
            : base("The {0} field must not have the default value")
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            var type = value.GetType();
            if (!type.IsValueType)
                return true;
            var instance = Activator.CreateInstance(type);
            return !value.Equals(instance);
        }
    }
}