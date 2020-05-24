using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.Core.PerMachine
{
    /// <inheritdoc />
    public interface IUpsertVmxLine<in T> : IRunFor2<string, T>
    {
    }
}