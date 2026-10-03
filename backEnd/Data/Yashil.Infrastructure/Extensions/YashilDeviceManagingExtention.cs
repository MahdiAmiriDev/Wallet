using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
namespace Yashil.Infrastructure.Extensions
{
    public static class YashilDeviceManagingExtention
    {
        public static bool ValidateMacAddress(string macAddress)
        {
            if (string.IsNullOrEmpty(macAddress))
                return false;

            var cleaned = CleanMacAddressRegex(macAddress);

            var allMacs = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up &&
                              nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => CleanMacAddressRegex(nic.GetPhysicalAddress().ToString()));

            return allMacs.Any(mac => mac == cleaned);
        }

        private static string CleanMacAddressRegex(string macAddress)
        {
            if (string.IsNullOrEmpty(macAddress))
                return macAddress;
            return Regex.Replace(macAddress, "[^a-fA-F0-9]", "").ToUpper();
        }

        //public static List<string> GetMacAddress()
        //{
        //    return NetworkInterface
        //        .GetAllNetworkInterfaces()
        //        .Where(nic => nic.OperationalStatus == OperationalStatus.Up &&
        //                      nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
        //        .Select(nic => nic.GetPhysicalAddress().ToString()).ToList();
        //}
    }
}
