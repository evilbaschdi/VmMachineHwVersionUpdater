using EvilBaschdi.Core;
using Microsoft.Extensions.DependencyInjection;

namespace VmMachineHwVersionUpdater.Core.DependencyInjection
{
    /// <inheritdoc />
    public interface IConfigureCoreServices : IRunFor<IServiceCollection>
    {
    }
}