using System.Xml.Linq;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ParseVboxFile : IParseVboxFile
{
    /// <inheritdoc />
    public RawMachine ValueFor(string file)
    {
        ArgumentNullException.ThrowIfNull(file);

        var rawMachine = new RawMachine
                         {
                             MachineType = MachineType.Vbox
                         };
        var xDocument = XDocument.Load(file);

        // Define the XML namespace to find the element correctly
        XNamespace ns = "http://www.virtualbox.org/";

        var machineNode = xDocument.Descendants(ns + "Machine").FirstOrDefault();
        if (machineNode == null)
        {
            return rawMachine;
        }

        rawMachine.VirtualBoxHwVersion =
            int.TryParse(machineNode.Attribute("version")?.Value, out var version) ? version : 0;

        rawMachine.DisplayName = machineNode.Attribute("name")?.Value ?? string.Empty;
        rawMachine.OSType = machineNode.Attribute("OSType")?.Value ?? string.Empty;

        var descriptionNode = machineNode.Descendants(ns + "Description").ToList();
        if (descriptionNode.Count != 0)
        {
            rawMachine.Annotation = descriptionNode.FirstOrDefault()?.Value;
        }

        var hardwareNode = machineNode.Descendants(ns + "Hardware").FirstOrDefault();
        if (hardwareNode == null)
        {
            return rawMachine;
        }

        var memoryNode = hardwareNode.Descendants(ns + "Memory").FirstOrDefault();
        if (memoryNode != null)
        {
            rawMachine.MemSize = int.TryParse(memoryNode.Attribute("RAMSize")?.Value, out var ramSize) ? ramSize : 0;
        }

        var platformNode = machineNode.Descendants(ns + "Platform").FirstOrDefault();
        if (platformNode == null)
        {
            return rawMachine;
        }

        var cpuNode = platformNode.Descendants(ns + "CPU").FirstOrDefault();
        if (cpuNode != null)
        {
            rawMachine.CpuCount = int.TryParse(cpuNode.Attribute("count")?.Value, out var count) ? count : 0;
        }

        return rawMachine;
    }
}