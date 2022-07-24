using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.misc
{
    public static class CommonsProcessUtils
    {


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
