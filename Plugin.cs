using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Fiddler;

namespace FiddlerToWCAT
{
    /// <summary>
    /// Plugin entry point.
    /// </summary>
    public class Plugin : IFiddlerExtension
    {
        private string _wcatPath;

        public void OnLoad()
        {
            if (!LookupWcat())
            {
                MessageBox.Show("WCAT not installed",
                                "FiddlerToWCAT",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            MenuItem menuItem = new MenuItem("Run WCAT Script for selected sessions", OnRunScript);

            FiddlerApplication.UI.mnuSessionContext.GetContextMenu().MenuItems.Add(menuItem);
        }

        public void OnBeforeUnload()
        {
        }

        private bool LookupWcat()
        {
            string path = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\wcat");
            if (Directory.Exists(path))
            {
                _wcatPath = path;
                return true;
            }

            path = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\wcat");
            if (Directory.Exists(path))
            {
                _wcatPath = path;
                return true;
            }

            return false;
        }

        private void OnRunScript(object sender, EventArgs eventArgs)
        {
            Session[] selectedSessions = FiddlerApplication.UI.GetSelectedSessions();
            if (selectedSessions.Length == 0)
            {
                MessageBox.Show("No sessions selected", "Run WCAT Script");
                return;
            }

            List<string> requests = new List<string>(selectedSessions.Length);
            string requestTemplate = DataHelper.GetEmbeddedResource(DataHelper.RequestTemplate);

            foreach (Session session in selectedSessions)
            {
                if (session.oRequest.headers.HTTPMethod != "GET")
                    continue;

                StringBuilder request = new StringBuilder(requestTemplate, 1024);
                string url = session.PathAndQuery;
                request.Replace(@"{url}", url);
                request.Replace(@"{port}", session.port.ToString());

                HTTPRequestHeaders headers = session.oRequest.headers;
                StringBuilder headersTemplate = new StringBuilder(1024);
                for (int i = 0; i < headers.Count(); i++)
                {
                    headersTemplate.AppendLine("      setheader");
                    headersTemplate.AppendLine("      {");
                    headersTemplate.AppendLine("        name=\"" + headers[i].Name + "\";");
                    headersTemplate.AppendLine(
                        "        value=\"" + headers[i].Value.Replace("\"", "\\\"") + "\";");
                    headersTemplate.AppendLine("      }");
                }

                request.Replace("{headers}", headersTemplate.ToString());
                requests.Add(request.ToString());
            }

            if (requests.Count < 1)
                return;

            string scenarioTemplate = DataHelper.GetEmbeddedResource(DataHelper.ScenarioTemplate);
            StringBuilder scenario = new StringBuilder(scenarioTemplate, 1024 * requests.Count);
            scenario.Replace("{requests}", string.Join(string.Empty, requests));

            string scenarioPath = Path.GetTempFileName() + ".wcat";
            string outputFile = Path.GetTempFileName() + ".wcat.xml";
            File.WriteAllText(scenarioPath, scenario.ToString());

            StartController(scenarioPath, outputFile).WaitForExit(0x7d0);
            StartClient("localhost").WaitForExit();

            if (File.Exists(outputFile))
                ShowResult(outputFile);
            else
                MessageBox.Show("WCAT Performance test didn't produce output file");
        }

        private Process ShowResult(string filepath)
        {
            string path = Path.Combine(Path.GetTempPath(), "report.xsl");
            if (!File.Exists(path))
                File.Copy(Path.Combine(_wcatPath, "report.xsl"), path);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "iexplore.exe",
                Arguments = filepath,
                UseShellExecute = true,
                CreateNoWindow = false,
                ErrorDialog = true
            };

            return Process.Start(startInfo);
        }

        private Process StartClient(string controller)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(_wcatPath, "wcclient.exe"),
                Arguments = controller,
                UseShellExecute = true,
                CreateNoWindow = false,
                ErrorDialog = true
            };
            return Process.Start(startInfo);
        }

        private Process StartController(string wcatFile, string outputFile)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(_wcatPath, "wcctl.exe"),
                Arguments = string.Format("-t {0} -c {1} -s {2} -v {3} -u {4} -w {5} -o {6} -new_console",
                                          wcatFile,
                                          1,
                                          "localhost",
                                          "10",
                                          20,
                                          2,
                                          outputFile),
                UseShellExecute = true,
                CreateNoWindow = false,
                ErrorDialog = true
            };
            return Process.Start(startInfo);
        }
    }
}