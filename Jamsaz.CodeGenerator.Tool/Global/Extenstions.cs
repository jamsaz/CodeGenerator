using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Reflection;

namespace Jamsaz.CodeGenerator.Tool.Global
{
    public static class Extentions
    {
        public static string Capitalize(this string word)
        {
            try
            {
                if (word.Length > 0)
                    return Inflector.Inflector.Capitalize(word);
                return word;
            }
            catch
            {
                return word;
            }
        }

        public static string UnCapitalize(this string word)
        {
            try
            {
                if (word.Length > 0)
                    return Inflector.Inflector.Uncapitalize(word);
                return word;
            }
            catch
            {
                return word;
            }
        }

        public static string Pluralize(this string word)
        {
            try
            {
                if (word.Length > 0)
                    return PluralizationService.CreateService(new System.Globalization.CultureInfo("en-US")).Pluralize(word);
                return word;
            }
            catch
            {
                return word;
            }
        }

        public static string Singularize(this string word)
        {
            try
            {
                if (word.Length > 0)
                    return PluralizationService.CreateService(new System.Globalization.CultureInfo("en-US")).Singularize(word);
                return word;
            }
            catch
            {
                return word;
            }
        }

        public static string UppercaseFirst(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string LowercaseFirst(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToLower(s[0]) + s.Substring(1);
        }
    }
}
