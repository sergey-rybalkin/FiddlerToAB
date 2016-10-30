using System.Diagnostics;
using System.IO;

namespace FiddlerToWcat
{
    internal class WcatController
    {
        private const int ClientsNumber = 1;

        private const int WarmupSeconds = 2;

        private readonly string _wcatPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="WcatController" /> class.
        /// </summary>
        /// <param name="wcatPath">Full path to the wcat installation directory.</param>
        internal WcatController(string wcatPath)
        {
            _wcatPath = wcatPath;
        }

        internal void RunScenario(
            string scenarioFile,
            string outputFile,
            string server,
            int port,
            int clients,
            int duration)
        {
            // Prepares batch file that attaches console tab to the ConEmu and runs wcat controller.
            string batchFile =
                PrepareControllerBatchFile(scenarioFile, outputFile, server, port, clients, duration);

            ProcessStartInfo controllerStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = string.Format("/k {0}", batchFile),
                UseShellExecute = true,
                CreateNoWindow = false,
                ErrorDialog = true
            };
            Process.Start(controllerStartInfo);

            ProcessStartInfo clientStartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(_wcatPath, "wcclient.exe"),
                Arguments = "localhost",
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                ErrorDialog = true
            };
            Process.Start(clientStartInfo);
        }

        internal string PrepareControllerBatchFile(
            string scenarioFile,
            string outputFile,
            string host,
            int port,
            int clients,
            int duration)
        {
            string batchFile = scenarioFile + ".cmd";
            FileStream batchStream = File.OpenWrite(batchFile);
            using (StreamWriter writer = new StreamWriter(batchStream))
            {
                writer.WriteLine(@"call ""C:\Program Files (x86)\Far\ConEmu\Attach.cmd""");
                writer.Write('"');
                writer.Write(Path.Combine(_wcatPath, "wcctl.exe"));
                writer.Write("\" ");

                string args = string.Format("-t {0} -c {1} -s {2} -v {3} -u {4} -w {5} -p {6} -o {7}",
                                            scenarioFile,
                                            ClientsNumber,
                                            host,
                                            clients,
                                            duration,
                                            WarmupSeconds,
                                            port,
                                            outputFile);
                writer.Write(args);
            }

            return batchFile;
        }
    }
}