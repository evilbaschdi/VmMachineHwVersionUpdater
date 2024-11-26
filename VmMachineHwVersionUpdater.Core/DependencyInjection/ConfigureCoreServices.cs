using EvilBaschdi.Core.AppHelpers;
using EvilBaschdi.Core.Internal;

namespace VmMachineHwVersionUpdater.Core.DependencyInjection;

/// <summary />
public static class ConfigureCoreServices
{
    /// <summary />
    public static void AddCoreServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IAddEditAnnotation, AddEditAnnotation>();
        services.AddSingleton<IArchiveMachine, ArchiveMachine>();
        services.AddSingleton<IChangeDisplayName, ChangeDisplayName>();
        services.AddSingleton<IConvertAnnotationLineBreaks, ConvertAnnotationLineBreaks>();
        services.AddSingleton<ICopyDirectoryWithFilesWithProgress, CopyDirectoryWithFilesWithProgress>();
        services.AddSingleton<ICopyDirectoryWithProgress, CopyDirectoryWithProgress>();
        services.AddSingleton<ICopyMachine, CopyMachine>();
        services.AddSingleton<ICopyProgress, CopyProgress>();
        services.AddSingleton<ICurrentItem, CurrentItem>();
        services.AddSingleton<IDeleteMachine, DeleteMachine>();
        services.AddSingleton<IFileListFromPath, FileListFromPath>();
        services.AddSingleton<IFilterItemSource, FilterItemSource>();
        services.AddSingleton<IGuestOsesInUse, GuestOsesInUse>();
        services.AddSingleton<IGuestOsOutputStringMapping, GuestOsOutputStringMapping>();
        services.AddSingleton<IGuestOsStringMapping, GuestOsStringMapping>();
        services.AddSingleton<IHandleMachineFromPath, HandleMachineFromPath>();
        services.AddSingleton<ILineStartActions, LineStartActions>();
        services.AddSingleton<ILoad, Load>();
        services.AddSingleton<ILoadSearchOsItems, LoadSearchOsItems>();
        services.AddSingleton<IMachinesFromPath, MachinesFromPath>();
        services.AddSingleton<IParseVmxFile, ParseVmxFile>();
        services.AddSingleton<IPathSettings, PathSettings>();
        services.AddSingleton<IProcessByPath, ProcessByPath>();
        services.AddSingleton<IReadLogInformation, ReadLogInformation>();
        services.AddSingleton<IReturnValueFromVmxLine, ReturnValueFromVmxLine>();
        services.AddSingleton<ISetDisplayName, SetDisplayName>();
        services.AddSingleton<ISetMachineIsEnabledForEditing, SetMachineIsEnabledForEditing>();
        services.AddSingleton<ISettingsValid, SettingsValid>();
        services.AddSingleton<IToggleToolsSyncTime, ToggleToolsSyncTime>();
        services.AddSingleton<IToggleToolsUpgradePolicy, ToggleToolsUpgradePolicy>();
        services.AddSingleton<IUpdateAnnotation, UpdateAnnotation>();
        services.AddSingleton<IUpdateMachineVersion, UpdateMachineVersion>();
        services.AddSingleton<IVmPools, VmPools>();
        services.AddSingleton<IVmxLineStartsWith, VmxLineStartsWith>();
    }
}