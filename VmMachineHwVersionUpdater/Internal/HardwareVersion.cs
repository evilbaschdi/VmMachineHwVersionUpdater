﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

                if(lineToLower.Contains("virtualhw.version"))
                {
                    var inputStreamReader = File.OpenText(vmxPath);
                    var text = inputStreamReader.ReadToEnd();
                    inputStreamReader.Close();

                    var newLine = string.Format("virtualhw.version = \"{0}\"", newVersion);

                    text = text.Replace(line, newLine);

                    var outputStreamWriter = File.CreateText(vmxPath);
                    outputStreamWriter.Write(text);
                    outputStreamWriter.Close();
                }
            }
        }

        public IEnumerable<Machine> ReadFromPath(string machinePath)
        {
            var machineList = new List<Machine>();
            foreach(var dir in Directory.GetDirectories(machinePath))
            {
                foreach(var path in Directory.GetFiles(dir, "*.vmx"))
                {
                    if(!path.EndsWith("f"))
                    {
                        var readAllLines = File.ReadAllLines(path);
                        //virtualHW.version = "10"
                        //displayName = "Chromixium"
                        //guestOS = "ubuntu-64"

                        var hwVersion = "";
                        var displayName = "";
                        var guestOs = "";

                        foreach(var lineToLower in readAllLines.Select(line => line.ToLower()))
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
                        }
                        var machine = new Machine
                        {
                            Id = Guid.NewGuid().ToString(),
                            HwVersion = Convert.ToInt32(hwVersion),
                            DisplayName = displayName,
                            GuestOs = guestOs,
                            Path = path
                        };
                        machineList.Add(machine);
                    }
                }
            }
            return machineList;
        }
    }
}