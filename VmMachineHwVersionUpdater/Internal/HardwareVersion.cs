﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VmMachineHwVersionUpdater.Extensions;

namespace VmMachineHwVersionUpdater.Internal
{
    public class HardwareVersion
    {
        public void Update(string vmxPath, int newVersion)
        {
            var readAllLines = File.ReadAllLines(vmxPath);

            foreach(var line in readAllLines)
            {
                var lineToLower = line.ToLower();

                if(!lineToLower.Contains("virtualhw.version"))
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

        public IEnumerable<Machine> ReadFromPath(string machinePath)
        {
            var filePath = new FilePath();
            var machineList = new List<Machine>();
            var fileList = filePath.GetFileList(machinePath).Distinct().ToList();

            Parallel.ForEach(fileList, file =>
            {
                var readAllLines = File.ReadAllLines(file);
                var hwVersion = "";
                var displayName = "";
                var guestOs = "";

                Parallel.ForEach(readAllLines.Select(line => line.ToLower()), lineToLower =>
                {
                    if(lineToLower.Contains("virtualhw.version"))
                    {
                        hwVersion = lineToLower.Replace('"', ' ').Trim();
                        hwVersion = hwVersion.Replace("virtualhw.version = ", "");
                    }
                    if(lineToLower.Contains("displayname"))
                    {
                        displayName = lineToLower.Replace('"', ' ').Trim();
                        displayName = displayName.Replace("displayname = ", "");
                    }
                    if(lineToLower.Contains("guestos"))
                    {
                        guestOs = lineToLower.Replace('"', ' ').Trim();
                        guestOs = guestOs.Replace("guestos = ", "");
                    }
                });
                var directoryInfo = new FileInfo(file).Directory;
                var size = directoryInfo.GetDirectorySize();
                var machine = new Machine
                {
                    Id = Guid.NewGuid().ToString(),
                    HwVersion = Convert.ToInt32(hwVersion),
                    DisplayName = displayName.Trim(),
                    GuestOs = MappingExtensions.GetGuestOsFullName(guestOs.Trim()),
                    Path = file.Trim(),
                    ShortPath = file.Replace(machinePath, "").Trim(),
                    DirectorySizeGb = Math.Round(size/(1024*1024*1024), 2),
                    DirectorySize =
                        $"MB: {Math.Round(size/(1024*1024), 2)} | KB: {Math.Round(size/(1024), 2)}"
                };
                machineList.Add(machine);
            });

            return machineList;
        }
    }
}