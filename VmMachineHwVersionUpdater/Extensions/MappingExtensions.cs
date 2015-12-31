using System.Collections.Generic;
using System.Linq;

namespace VmMachineHwVersionUpdater.Extensions
{
    /// <summary>
    /// </summary>
    public class MappingExtensions
    {
        private static IEnumerable<KeyValuePair<string, string>> GuestOsMappings()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                //Windows 9x.
                new KeyValuePair<string, string>("win31", "Windows 3.1"),
                new KeyValuePair<string, string>("win95", "Windows 95"),
                new KeyValuePair<string, string>("win98", "Windows 98"),
                new KeyValuePair<string, string>("winnt", "Windows NT"),
                new KeyValuePair<string, string>("winme", "Windows ME"),
                new KeyValuePair<string, string>("win2000pro", "Windows 2000 Professional"),
                new KeyValuePair<string, string>("win2000serv", "Windows 2000 Server"),
                new KeyValuePair<string, string>("win2000advserv", "Windows 2000 Advanced Server "),
                //Windows XP.
                new KeyValuePair<string, string>("winxphome", "Windows XP Home"),
                new KeyValuePair<string, string>("winxppro", "Windows XP Professional"),
                new KeyValuePair<string, string>("winxppro-64", "Windows XP Professional x64"),
                new KeyValuePair<string, string>("winnetstandard", "Windows Server 2003 Standard"),
                new KeyValuePair<string, string>("winnetstandard-64", "Windows Server 2003 Standard x64"),
                new KeyValuePair<string, string>("winnetenterprise", "Windows Server 2003 Enterprise"),
                new KeyValuePair<string, string>("winnetenterprise-64", "Windows Server 2003 Enterprise x64"),
                new KeyValuePair<string, string>("winnetbusiness", "Windows Server 2003 Small Business "),
                new KeyValuePair<string, string>("winnetweb", "Windows Server 2003 Web"),
                //Windows Vista.
                new KeyValuePair<string, string>("winvista", "Windows Vista"),
                new KeyValuePair<string, string>("winvista-64", "Windows Vista x64"),
                new KeyValuePair<string, string>("longhorn", "Windows Server 2008"),
                new KeyValuePair<string, string>("longhorn-64", "Windows Server 2008 x64"),
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
                new KeyValuePair<string, string>("windows9srv-64", "Windows Server 2016 x64"),
                //Linux.
                new KeyValuePair<string, string>("otherlinux", "Other Linux 2.2.x kernel"),
                new KeyValuePair<string, string>("otherlinux-64", "Other Linux 2.2.x kernel x64"),
                new KeyValuePair<string, string>("other24xlinux", "Other Linux 2.4.x kernel"),
                new KeyValuePair<string, string>("other24xlinux-64", "Other Linux 2.4.x kernel x64"),
                new KeyValuePair<string, string>("other26xlinux", "Other Linux 2.6.x kernel"),
                new KeyValuePair<string, string>("other26xlinux-64", "Other Linux 2.6.x kernel x64"),
                new KeyValuePair<string, string>("other3xlinux", "Other Linux 3.x kernel"),
                new KeyValuePair<string, string>("other3xlinux-64", "Other Linux 3.x kernel x64"),
                //Ubuntu.
                new KeyValuePair<string, string>("ubuntu", "Ubuntu"),
                new KeyValuePair<string, string>("ubuntu-64", "Ubuntu x64"),
                //Fedora.
                new KeyValuePair<string, string>("fedora", "Fedora"),
                new KeyValuePair<string, string>("fedora-64", "Fedora x64"),
                //OpenSuse.
                new KeyValuePair<string, string>("opensus", "OpenSuse"),
                new KeyValuePair<string, string>("opensuse-64", "OpenSuse x64"),
                //CentOS.
                new KeyValuePair<string, string>("centos", "Cent OS"),
                new KeyValuePair<string, string>("centos-64", "Cent OS x64"),
                //Debian.
                new KeyValuePair<string, string>("debian5", "Debian 5"),
                new KeyValuePair<string, string>("debian5-64", "Debian 5 x64"),
                new KeyValuePair<string, string>("debian6", "Debian 6"),
                new KeyValuePair<string, string>("debian6-64", "Debian 6 x64"),
                new KeyValuePair<string, string>("debian7", "Debian 7"),
                new KeyValuePair<string, string>("debian7-64", "Debian 7 x64"),
                //Other.
                new KeyValuePair<string, string>("winhyperv", "Hyper-V"),
                new KeyValuePair<string, string>("vmkernel6", "VMware vSphere 6")
            };

            return list;
        }

        /// <summary>
        /// </summary>
        public static string GetGuestOsFullName(string guestOs)
        {
            var fullName = GuestOsMappings().FirstOrDefault(mapping => mapping.Key == guestOs).Value;

            return !string.IsNullOrWhiteSpace(fullName) ? fullName : guestOs;
        }
    }
}