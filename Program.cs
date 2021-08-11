using System;
using Microsoft.Win32;

namespace SharpInstallSoft
{
    class Program
    {
        public static bool CheckInList(string address, RegistryHive hive)
        {
            var view = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (var localKey = RegistryKey.OpenBaseKey(hive, view).OpenSubKey(address))
            {
                try
                {
                    foreach (var subKey in localKey.GetSubKeyNames())
                    {
                        using (RegistryKey keyName = localKey.OpenSubKey(subKey))
                        {
                            if (String.IsNullOrEmpty((string)keyName.GetValue("DisplayName")))
                            {
                                continue;
                            }
                            Console.WriteLine("DisplayName: {0}\nDisplayVersion: {1}\nInstallLocation: {2}\nInno Setup: App Path: {3}\nInno Setup: User: {4}\nInstallDate: {5}\nPublisher: {6}", keyName.GetValue("DisplayName"), keyName.GetValue("DisplayVersion"), keyName.GetValue("InstallLocation"), keyName.GetValue("Inno Setup: App Path"), keyName.GetValue("Inno Setup: User"), keyName.GetValue("InstallDate"), keyName.GetValue("Publisher"));
                            Console.WriteLine();
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }

            }
            return true;
        }
        public static void GetInstalledApps()
        {

            string uninstallKey1 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            string uninstallKey2 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            CheckInList(uninstallKey1, RegistryHive.LocalMachine);
            CheckInList(uninstallKey1, RegistryHive.CurrentUser);
            CheckInList(uninstallKey2, RegistryHive.LocalMachine);

        }
        static void Main(string[] args)
        {
            GetInstalledApps();
        }
    }
}
