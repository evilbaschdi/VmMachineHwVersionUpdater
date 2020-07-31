using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.Core.PerMachine
{
    /// <inheritdoc />
    public class ReturnValueFromVmxLine : IReturnValueFromVmxLine
    {
        /// <inheritdoc />
        public string ValueFor([NotNull] string line,[NotNull] string key)
        {
           
            if (line == null)
            {
                throw new ArgumentNullException(nameof(line));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            var value = line.Replace('"', ' ').Trim();
            value = Regex.Replace(value, $"{key} = ", "", RegexOptions.IgnoreCase).Trim();

            return value;
        }
    }
}