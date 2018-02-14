using System.Reflection;

namespace Schedulee.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj.GetType().GetRuntimeProperty(propertyName).GetValue(obj);
        }
    }
}
