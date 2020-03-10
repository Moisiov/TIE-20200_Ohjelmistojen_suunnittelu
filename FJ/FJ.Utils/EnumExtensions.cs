using System;
using System.ComponentModel;
using System.Reflection;

namespace FJ.Utils
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the Description of enum's DescriptionAttribute
        /// </summary>
        /// <param name="enumValue">Enum of which description is looked for</param>
        /// <returns>Description of this enum</returns>
        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())[0]
                            .GetCustomAttribute<DescriptionAttribute>()
                            .Description;
        }

        /// <summary>
        /// Gets the first letter as upper from enum's DescriptionAttribute
        /// </summary>
        /// <param name="enumValue">Enum of which description is looked for</param>
        /// <returns>Short one-letter description of this enum</returns>
        public static string GetShortDescription(this Enum enumValue)
        {
            var desc = enumValue.GetDescription();
            if (desc.IsNullOrWhitespace())
            {
                return string.Empty;
            }

            return desc[0].ToString().ToUpper();
        }
    }
}
