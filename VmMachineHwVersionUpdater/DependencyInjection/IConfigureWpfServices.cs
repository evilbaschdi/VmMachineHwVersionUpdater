using EvilBaschdi.Core;
using Microsoft.Extensions.DependencyInjection;

namespace VmMachineHwVersionUpdater.DependencyInjection
{
    /// <inheritdoc />
    public interface IConfigureWpfServices : IRunFor<IServiceCollection>
    {
    }
}