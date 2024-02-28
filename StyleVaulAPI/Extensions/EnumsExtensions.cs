using System.ComponentModel;
using System.Reflection;

namespace StyleVaulAPI.Extensions
{
    public static class EnumsExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            if (value == null)
                return string.Empty;

            return value
                    .GetType()
                    ?.GetMember(value.ToString())
                    ?.FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description ?? string.Empty;
        }
    }
}
