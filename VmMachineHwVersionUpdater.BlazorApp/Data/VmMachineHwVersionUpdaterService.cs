using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.CoreExtended.AppHelpers;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.BlazorApp.Data
{
    public class VmMachineHwVersionUpdaterService
    {
        private IPathSettings _pathSettings;
        private int _updateAllHwVersion;
        private IUpdateMachineVersion _updateMachineVersion;
        private IArchiveMachine _archiveMachine;
        
        private IDeleteMachine _deleteMachine;
        private IGuestOsesInUse _guestOsesInUse;
        private ListCollectionView _listCollectionView;
        private IMachinesFromPath _machinesFromPath;
        private string _prevSortHeader;
        private IProcessByPath _processByPath;
       // private SortDescription _sd = new SortDescription("DisplayName", ListSortDirection.Ascending);
        private Machine _selectedMachine;
        private string _sortHeader;
        private IToggleToolsSyncTime _toggleToolsSyncTime;
        private IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy;
        private List<Machine> _currentItemSource;

        public Task<List<Machine>> GetMachinesAsync()
        {
            IVmPools vmPools = new VmPools();
            _pathSettings = new PathSettings(vmPools);


            var vmPoolFromSettingExistingPaths = _pathSettings.VmPool.GetExistingDirectories();
            if (vmPoolFromSettingExistingPaths.Any())
            {
                Load();
            }
            else
            {
                //this.ShowMessageAsync("No virtual machines found", "Please verify settings and discs attached");
            }


            return Task.FromResult(_currentItemSource);
        }

         private void Load()
        {
            IFileListFromPath fileListFromPath = new FileListFromPath();
            IGuestOsStringMapping guestOsStringMapping = new GuestOsStringMapping();
            IGuestOsOutputStringMapping guestOsOutputStringMapping = new GuestOsOutputStringMapping(guestOsStringMapping);
            IReadLogInformation readLogInformation = new ReadLogInformation();
            _updateMachineVersion = new UpdateMachineVersion();

            IReturnValueFromVmxLine returnValueFromVmxLine = new ReturnValueFromVmxLine();
            IVmxLineStartsWith vmxLineStartsWith = new VmxLineStartsWith();
            IConvertAnnotationLineBreaks convertAnnotationLineBreaks = new ConvertAnnotationLineBreaks();
            IHandleMachineFromPath handleMachineFromPath =
                new HandleMachineFromPath(guestOsOutputStringMapping, _pathSettings, _updateMachineVersion, readLogInformation, returnValueFromVmxLine, vmxLineStartsWith,
                    convertAnnotationLineBreaks);
            _processByPath = new ProcessByPath();
            _toggleToolsSyncTime = new ToggleToolsSyncTime();
            _toggleToolsUpgradePolicy = new ToggleToolsUpgradePolicy();
            _guestOsesInUse = new GuestOsesInUse(guestOsStringMapping);
            _machinesFromPath = new MachinesFromPath(_pathSettings, handleMachineFromPath, fileListFromPath);
            _archiveMachine = new ArchiveMachine(_pathSettings);
            _deleteMachine = new DeleteMachine();

            ILoad load = new Load(_machinesFromPath);
            var loadValue = load.Value;


            _currentItemSource = loadValue.VmDataGridItemsSource;
            //DataContext = loadValue.VmDataGridItemsSource;
            //_listCollectionView = new ListCollectionView(loadValue.VmDataGridItemsSource);
            //_listCollectionView?.GroupDescriptions?.Add(new PropertyGroupDescription("Directory"));
            //_listCollectionView.SortDescriptions.Add(_sd);
            //VmDataGrid.ItemsSource = _listCollectionView;

            //UpdateAllTextBlock.Text = loadValue.UpdateAllTextBlocks;
            //UpdateAllHwVersion.Value = loadValue.UpdateAllHwVersion;

            //SearchOs filter
            //LoadSearchOsItems(loadValue);
        }
    }


}
