using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ReactiveUI;

namespace FJ.Client.Core
{
    public class FJNotificationObject : ReactiveObject
    {
        public void RaisePropertiesChanged()
        {
            RaisePropertyChanged(null);
        }
        
        protected bool SetAndRaise<T>(ref T field, T value, [CallerMemberName] string caller = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            RaisePropertyChanged(caller);

            return true;
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            ((IReactiveObject)this).RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            RaisePropertyChanged(expression.Member.Name);
        }
    }
}
