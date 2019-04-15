using System;
using System.Diagnostics;

namespace codeCleanerConsole.BLL
{
    /// <summary>
    /// fsutil behavior query disablelastaccess
    /// fsutil behavior set disablelastaccess 0
    /// since Windows Vista, this property comes enabled by default, which means LastAccessDate filke property does not get updated.
    /// So, in order to this App works properly, it is necessary to disable by setting this in 0 
    /// </summary>
    public static class FSutil
    {
        public static void FSutilBehaviorCheck()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @Environment.SystemDirectory + @"\fsutil.exe",
                        Arguments = "behavior query disablelastaccess",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false
                    }
                };
                process.Start();

                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();
                    if (!line.Contains("DisableLastAccess = 0")) // expected value for this property is 0
                        throw new Exception("fsutil behavior disablelastaccess IS NOT 0 - (MUST BE SET ON 0)");
                }
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Program.logs.ErrorFSutilBehavior += "#200 - FSutil - " + ex.GetType().Name + " = " + ex.Message;
            }
        }

    }
}
