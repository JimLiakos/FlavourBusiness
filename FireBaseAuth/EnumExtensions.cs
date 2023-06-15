namespace Firebase.Auth
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <MetaDataID>{a14e05f8-5b28-4db7-8a41-d925b607ece4}</MetaDataID>
    public static class EnumExtensions
    {
        /// <summary>
        /// Finds the <see cref="EnumMemberAttribute"/> on given enum and returns its value.
        /// </summary>
        public static string ToEnumString<T>(this T type)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetTypeInfo().DeclaredFields.First(f => f.Name == name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();

            return enumMemberAttribute.Value;
        }
    }
}
