using System.Text.RegularExpressions;

namespace StyleVaulAPI.Extensions
{
    public static class StringExtensions
    {
        public static bool EmailIsValid(this string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            var regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
