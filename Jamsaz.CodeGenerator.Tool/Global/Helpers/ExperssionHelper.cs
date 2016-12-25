using System;
using System.Reflection;

namespace Jamsaz.CodeGenerator.Tool.Global.Helpers
{
    public class ExperssionHelper
    {
        public static Func<T, bool> GetLambdaFor<T>(MethodInfo method, object target)
        {
            return (Func<T, bool>)Delegate.CreateDelegate(typeof(Func<T, bool>),
                target, method.MakeGenericMethod(typeof(T)));
        }
    }
}
