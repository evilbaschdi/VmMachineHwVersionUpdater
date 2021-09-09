using EvilBaschdi.Core;
using Microsoft.Extensions.DependencyInjection;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public interface IConfigureDefaultCommandServices : IRunFor<IServiceCollection>
    {
    }
}