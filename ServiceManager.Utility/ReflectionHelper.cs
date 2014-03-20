using System;
using System.Linq.Expressions;

namespace Celigo.ServiceManager.Utility
{
    static class ReflectionHelper
    {
        public static Action<TInstance, TProperty> GetSetter<TInstance, TProperty>(Expression<Func<TInstance, TProperty>> getter)
        {
            var member = (MemberExpression)getter.Body;
            var param = Expression.Parameter(typeof(string), "value");
#if CLR_2_0
            MethodInfo setMethod = ((PropertyInfo)member.Member).GetSetMethod();
            return (Action<TInstance, TProperty>)
                Delegate.CreateDelegate(typeof(Action<TInstance, TProperty>), setMethod);
#else
            // re-write in .NET 4.0 as a "set"
            var setter = Expression.Lambda<Action<TInstance, TProperty>>(
                Expression.Assign(member, param), getter.Parameters[0], param);

            // compile it
            return setter.Compile();
#endif
        }
    }
}
