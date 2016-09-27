using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EvilBaschdi.Core.DirectoryExtensions;
using EvilBaschdi.Core.Threading;

namespace VmMachineHwVersionUpdater.Internal
{
    /// <summary>
    /// </summary>
    public class HardwareVersion : IHardwareVersion
    {
        private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping;

        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        /// <exception cref="ArgumentNullException"><paramref name="guestOsOutputStringMapping" /> is <see langword="null" />.</exception>
        public HardwareVersion(IGuestOsOutputStringMapping guestOsOutputStringMapping)
        {
            if (guestOsOutputStringMapping == null)
            {
                throw new ArgumentNullException(nameof(guestOsOutputStringMapping));
            }
            _guestOsOutputStringMapping = guestOsOutputStringMapping;
        }

        /// <summary>
        /// </summary>
        /// <param name="vmxPath"></param>
        /// <param name="newVersion"></param>
        public void Update(string vmxPath, int newVersion)
        {
            var readAllLines = File.ReadAllLines(vmxPath);

            foreach (var line in readAllLines)
            {
                var lineToLower = line.ToLower();

                if (!lineToLower.Contains("virtualhw.version"))
                {
                    continue;
                }
                var inputStreamReader = File.OpenText(vmxPath);
                var text = inputStreamReader.ReadToEnd();
                inputStreamReader.Close();

                var newLine = $"virtualhw.version = \"{newVersion}\"";

                text = text.Replace(line, newLine);

                var outputStreamWriter = File.CreateText(vmxPath);
                outputStreamWriter.Write(text);
                outputStreamWriter.Close();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="machinePath"></param>
        /// <returns></returns>
        public IEnumerable<Machine> ReadFromPath(string machinePath)
        {
            var multiThreadingHelper = new MultiThreadingHelper();
            var filePath = new FilePath(multiThreadingHelper);
            var machineList = new ConcurrentBag<Machine>();
            var includeExtensionList = new List<string>
                                       {
                                           "vmx"
                                       };

            var fileList = filePath.GetFileList(machinePath, includeExtensionList).Distinct().ToList();

            Parallel.ForEach(fileList,
                file =>
                {
                    var readAllLines = File.ReadAllLines(file);
                    var hwVersion = "";
                    var displayName = "";
                    var guestOs = "";

                    Parallel.ForEach(readAllLines,
                        line =>
                        {
                            var lineToLower = line.ToLower();
                            if (lineToLower.Contains("virtualhw.version"))
                            {
                                hwVersion = lineToLower.Replace('"', ' ').Trim();
                                hwVersion = hwVersion.Replace("virtualhw.version = ", "");
                            }
                            if (lineToLower.Contains("displayname"))
                            {
                                displayName = line.Replace('"', ' ').Trim();
                                displayName = Regex.Replace(displayName, "displayname = ", "", RegexOptions.IgnoreCase);
                            }
                            if (lineToLower.Contains("guestos"))
                            {
                                guestOs = lineToLower.Replace('"', ' ').Trim();
                                guestOs = guestOs.Replace("guestos = ", "");
                            }
                        });

                    var fileInfo = new FileInfo(file);
                    var directoryInfo = fileInfo.Directory;
                    var log = File.Exists($@"{directoryInfo?.FullName}\vmware.log") ? $@"{directoryInfo?.FullName}\vmware.log" : null;
                    var logLastDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(log) && !log.IsFileLocked())
                    {
                        var logLastLine = File.ReadAllLines(log).Last();
                        logLastDate = logLastLine.Split('|').First().Replace("T", " ").Substring(0, 16);
                    }

                    var size = directoryInfo.GetDirectorySize();
                    var machine = new Machine
                                  {
                                      Id = Guid.NewGuid().ToString(),
                                      HwVersion = Convert.ToInt32(hwVersion),
                                      DisplayName = displayName.Trim(),
                                      GuestOs = _guestOsOutputStringMapping.GetGuestOsFullName(guestOs.Trim()),
                                      Path = fileInfo.FullName,
                                      ShortPath = fileInfo.FullName.Replace(machinePath.ToLower(), ""),
                                      DirectorySizeGb = Math.Round(size/(1024*1024*1024), 2),
                                      DirectorySize = $"MB: {Math.Round(size/(1024*1024), 2)} | KB: {Math.Round(size/(1024), 2)}",
                                      LogLastDate = logLastDate
                                  };
                    machineList.Add(machine);
                });

            return machineList;
        }
    }
}