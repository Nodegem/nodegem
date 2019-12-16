using System.Linq;
using System.Net.NetworkInformation;

namespace Nodegem.Common.Utilities {
    public class DeviceUtilities {
        public static string GetMacAddress()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where( nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback )
                .Select( nic => nic.GetPhysicalAddress().ToString() )
                .FirstOrDefault();
        }
    }
}