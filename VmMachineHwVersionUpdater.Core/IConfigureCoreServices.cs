using EvilBaschdi.Core;
using Microsoft.Extensions.DependencyInjection;

namespace VmMachineHwVersionUpdater.Core
{
    /// <inheritdoc />
    public interface IConfigureCoreServices : IRunFor<IServiceCollection>
    {
    }
}