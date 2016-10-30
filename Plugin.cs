using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Fiddler;

namespace FiddlerToWcat
{
    /// <summary>
    /// Plugin entry point.
    /// </summary>
    public class Plugin : IFiddlerExtension
    {
        private const string WcatDefaultLocation = @"%ProgramFiles%\wcat";

        private string _wcatPath;

        public void OnLoad()
        {
            if (!LookupWcat())
            {
                ShowWarning("WCAT cannot be found at " + WcatDefaultLocation);
                return;
            }

            MenuItem menuItem = new MenuItem("Run WCAT Script for selected sessions", OnRunScript);

            var menu = FiddlerApplication.UI.mnuSessionContext.GetContextMenu();
            if (null != menu)
                menu.MenuItems.Add(menuItem);
        }

        public void OnBeforeUnload()
        {
        }

        internal static void ShowWarning(string message)
        {
            FiddlerApplication.AlertUser("FiddlerToWCAT", message);
        }

        private bool LookupWcat()
        {
            string path = Environment.ExpandEnvironmentVariables(WcatDefaultLocation);
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
            if (0 == selectedSessions.Length)
            {
                ShowWarning("No sessions selected");
                return;
            }

            StartDialog dlg = new StartDialog();
            if (DialogResult.OK != dlg.ShowDialog())
                return;

            int targetPort = selectedSessions[0].port;
            string targetServer = selectedSessions[0].host;

            string scenarioFile =
                ExportSessionsAsWcatScript(selectedSessions, dlg.SelectedPath, targetServer);
            string outputFile = scenarioFile + ".xml";
            WcatController controller = new WcatController(_wcatPath);

            if (!dlg.SkipRun)
            {
                controller.RunScenario(
                    scenarioFile,
                    outputFile,
                    targetServer,
                    targetPort,
                    dlg.Clients,
                    dlg.DurationSeconds);
            }
            else
            {
                controller.PrepareControllerBatchFile(
                    scenarioFile,
                    outputFile,
                    targetServer,
                    targetPort,
                    dlg.Clients,
                    dlg.DurationSeconds);
            }            
        }

        private string ExportSessionsAsWcatScript(Session[] sessions, string location, string fileName)
        {
            string scenarioFileName = fileName + ".wcat";
            string scenarioFilePath = Path.Combine(location, scenarioFileName);

            Dictionary<string, object> options = new Dictionary<string, object>(1);
            options["Filename"] = scenarioFilePath;

            if (!FiddlerApplication.DoExport("WCAT Script", sessions, options, null))
                return null;

            return scenarioFilePath;
        }
    }
}