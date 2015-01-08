using System.Collections.Generic;
using System.Linq;

namespace VmMachineHwVersionUpdater.Extensions
{
    public class MappingExtensions
    {
        private static IEnumerable<KeyValuePair<string, string>> GuestOsMappings()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                //Windows XP
                new KeyValuePair<string, string>("winxppro", "Windows XP Professional"),
                new KeyValuePair<string, string>("winnetstandard", "Windows Server 2003 Standard"),
                //Windows Vista.
                //Windows 7.
                new KeyValuePair<string, string>("windows7", "Windows 7"),
                new KeyValuePair<string, string>("windows7-64", "Windows 7 x64"),
                new KeyValuePair<string, string>("windows7srv-64", "Windows Server 2008 R2 x64"),
                //Windows 8.
                new KeyValuePair<string, string>("windows8", "Windows 8"),
                new KeyValuePair<string, string>("windows8-64", "Windows 8 x64"),
                new KeyValuePair<string, string>("windows8srv-64", "Windows Server 2012 x64"),
                //Windows 10.
                new KeyValuePair<string, string>("windows9", "Windows 10"),
                new KeyValuePair<string, string>("windows9-64", "Windows 10 x64"),
                new KeyValuePair<string, string>("windows9srv-64", "Windows Server 2015 x64")
            };

            return list;
        }

        public static string GetGuestOsFullName(string guestOs)
        {
            var fullName = string.Empty;
            foreach(var guestOsMapping in GuestOsMappings().Where(guestOsMapping => guestOsMapping.Key == guestOs))
            {
                fullName = guestOsMapping.Value;
            }

            return !string.IsNullOrWhiteSpace(fullName) ? fullName : guestOs;
        }
    }
}