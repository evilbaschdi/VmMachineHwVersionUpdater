using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine
{
    /// <inheritdoc cref="IUpdateMachineVersion" />
    public class UpdateMachineVersion : UpsertVmxLine<int>, IUpdateMachineVersion
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public UpdateMachineVersion()
            : base("virtualhw.version")
        {
        }

        /// <inheritdoc />
        public void RunFor(List<Machine> machines, int newVersion)
        {
            if (machines == null)
            {
                throw new ArgumentNullException(nameof(machines));
            }

            Parallel.ForEach(machines.Where(m => m.IsEnabledForEditing), machine => { RunFor(machine.Path, newVersion); });
        }
    }
}