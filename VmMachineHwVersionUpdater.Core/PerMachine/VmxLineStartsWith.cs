using System;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.Core.PerMachine
{
    /// <inheritdoc />
    public class VmxLineStartsWith : IVmxLineStartsWith
    {
        /// <inheritdoc />
        public bool ValueFor([NotNull] string line, [NotNull] string key)
        {
            if (line == null)
            {
                throw new ArgumentNullException(nameof(line));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return line.StartsWith(key, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}