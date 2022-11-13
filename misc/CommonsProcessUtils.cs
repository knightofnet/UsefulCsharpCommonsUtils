using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace UsefulCsharpCommonsUtils.misc
{
    public static class CommonsProcessUtils
    {

        public static bool ExistsAppInstanceOf(string filepath)
        {
            ManagementClass mngmtClass = new ManagementClass("Win32_Process");
            return mngmtClass.GetInstances().Cast<ManagementBaseObject>().Any(o => o["ExecutablePath"] != null && o["ExecutablePath"].ToString().Equals(filepath));
        }

        public static int CountAppInstanceOf(string filepath)
        {
            ManagementClass mngmtClass = new ManagementClass("Win32_Process");
            return mngmtClass.GetInstances().Cast<ManagementBaseObject>().Count(o => o["ExecutablePath"] != null && o["ExecutablePath"].ToString().Equals(filepath));
        }
        public static bool DoCmd(string app, string argsStr = null, string workingDirectory = "")
        {
            //Log.Debug($"DoCmd: {app} {argsStr}");



            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = app,
                Arguments = argsStr ?? string.Empty,
#if DEBUG
                WindowStyle = ProcessWindowStyle.Normal,
#else
                WindowStyle = ProcessWindowStyle.Hidden,
#endif
                WorkingDirectory = workingDirectory,
            };

            try
            {


                Process p = Process.Start(psi);
               // Log.Debug($"DoCmd: processId: {p.Id}");
                p.WaitForExit();

                //Log.Debug($"DoCmd: ExitCode: {p.ExitCode}");

                return p.ExitCode == 0;
            }
            catch (Exception ex)
            {
                //Log.Warn(ex);
                return false;
            }

        }


    }
}
