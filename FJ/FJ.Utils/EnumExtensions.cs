using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FJ.Utils
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the Description of enum's DescriptionAttribute
        /// </summary>
        /// <param name="enumValue">Enum of which description is looked for</param>
        /// <returns>Description of this enum</returns>
        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())[0]
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetDescription();
        }
    }
}
