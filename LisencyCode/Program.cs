using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace LisencyCode
{
    class Program
    {
        public static string GetCpuId()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }
        public static string GetHddId(string wmiClass, string wmiProperty)
        //Return a hardware identifier
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }
        public static String GetMacAddressOfComputer()
        {
            String firstMacAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();
            return firstMacAddress;
        }
        public static void AddDataToRegistry()
        {

        }
        public static string GetDataFromRegistry()
        {
            return "";
        }
        static void Main(string[] args)
        {


            var macAddress = GetMacAddressOfComputer();
            var cpuId = GetCpuId();
            string modelNo = GetHddId("Win32_DiskDrive", "Model");
            int currentYear = DateTime.Now.Year;
            StringBuilder uniquecode = new StringBuilder();
            uniquecode.Append(macAddress);
            uniquecode.Append(cpuId);
            uniquecode.Append(currentYear.ToString());
            uniquecode.Append(modelNo);
            //Console.WriteLine(uniquecode);
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);
            key.CreateSubKey("LisencyCode");
            key = key.OpenSubKey("AppName", true);
            key.CreateSubKey("AppVersion");
            key = key.OpenSubKey("AppVersion", true);

            key.SetValue("yourkey", uniquecode);

        }
    }
}
