using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FJ.FinlandiaHiihtoAPI.Utils
{
    public static class LambdaExtensions
    {
        public static T SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLambda, TValue value)
        {
            if (!(memberLambda.Body is MemberExpression memberSelectorExpression))
            {
                return target;
            }
            
            var property = memberSelectorExpression.Member as PropertyInfo;
            if (property != null)
            {
                property.SetValue(target, value, null);
            }

            return target;
        }
    }
}
