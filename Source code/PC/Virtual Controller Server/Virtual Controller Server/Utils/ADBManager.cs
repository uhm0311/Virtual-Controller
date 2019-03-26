using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualController.Utils
{
    public class ADBManager : IDisposable
    {
        public ADBManager()
        {
            bool adb = false;
            System.Diagnostics.Process[] processlist = System.Diagnostics.Process.GetProcesses();

            foreach (System.Diagnostics.Process process in processlist)
            {
                if (process.ProcessName.StartsWith("adb"))
                    adb = true;
            }

            if (adb)
            {
                adb = false;
                killADB();
            }
            startADB();
        }

        public void Dispose()
        {
            killADB();
        }

        private System.Diagnostics.ProcessStartInfo getStartInfo(string fileName)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.FileName = "\"" + System.Environment.CurrentDirectory + "\\adb\\" + fileName + "\"";

            info.CreateNoWindow = true;
            info.UseShellExecute = false;

            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            return info;
        }

        private void startADB()
        {
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(getStartInfo("adb_start.bat"));
            process.WaitForExit();
        }

        private void killADB()
        {
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(getStartInfo("adb_kill.bat"));
            process.WaitForExit();
        }

        public int getConnectedDevices()
        {
            string read = "";
            int devices = 0;

            System.Diagnostics.Process process = System.Diagnostics.Process.Start(getStartInfo("adb_devices.bat"));
            process.WaitForExit();

            read = process.StandardOutput.ReadLine();
            if (read != null && read.ToLower().StartsWith("list of"))
            {
                while (true)
                {
                    read = process.StandardOutput.ReadLine();

                    if (read != null && read.Length > 0)
                        devices++;
                    else break;
                }
            }
            process.Close();

            return devices;
        }

        public void portForward(int port)
        {
            string read = " ";

            while (read != null && read.Length > 0)
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(getStartInfo("adb_forward.bat"));
                process.WaitForExit();

                read = process.StandardOutput.ReadLine() + process.StandardError.ReadLine();
                process.Close();
            }
        }
    }
}
