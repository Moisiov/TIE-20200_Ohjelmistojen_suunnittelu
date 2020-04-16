using System;
using System.Reflection;

namespace FinlandiaHiihtoAPI.Utils
{
    public static class EnumExtensions
    {
        public static string GetRequestValue(this Enum enumValue)
        {
            return enumValue?.GetType()
                .GetMember(enumValue.ToString())[0]
                .GetCustomAttribute<RequestValue>()?
                .Value;
        }
    }
}
