namespace Schedulee.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool ObjectsEqual(this object value1, object value2)
        {
            return value1 == null && value2 == null || value1 != null && value2 != null && value1.Equals(value2);
        }
    }
}
