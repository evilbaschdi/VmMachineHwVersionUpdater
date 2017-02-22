using System;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    ///     Some extensions for string
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Contains <see cref="System.StringComparison" /> statt.
        /// </summary>
        /// <param name="source">Die Zeichenfolge in der gesucht werden soll.</param>
        /// <param name="value">Die zu suchende Zeichenfogle.</param>
        /// <param name="comparisonType">Der Modus der beim Vergleichen angewendet werden soll.</param>
        /// <returns>
        ///     <c>true</c> wenn <paramref name="value" /> in <paramref name="source" /> gefunden wurde; andernfalls
        ///     <c>false</c>
        /// </returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        ///     Replace with <see cref="System.StringComparison" />
        /// </summary>
        /// <param name="source"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static string Replace(this string source, string oldValue, string newValue, StringComparison comparisonType)
        {
            var index = source.IndexOf(oldValue, comparisonType);

            // Determine if we found a match
            var matchFound = index >= 0;

            if (!matchFound)
            {
                return source;
            }
            // Remove the old text
            source = source.Remove(index, oldValue.Length);

            // Add the replacemenet text
            source = source.Insert(index, newValue);

            return source;
        }
    }
}