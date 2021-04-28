using EvilBaschdi.Core;
using Microsoft.Extensions.DependencyInjection;

namespace VmMachineHwVersionUpdater
{
    /// <inheritdoc />
    public interface IConfigureWpfServices : IRunFor<IServiceCollection>
    {
    }
}