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
            MessageBox.Show(message, "FiddlerToWCAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (selectedSessions.Length == 0)
            {
                ShowWarning("No sessions selected");
                return;
            }

            string scenarioFile = ExportSessionsAsWcatScript(selectedSessions);
            string outputFile = scenarioFile + ".xml";

            WcatController controller = new WcatController(_wcatPath);

            int targetPort = selectedSessions[0].port;

            if (80 == targetPort)
                controller.RunScenario(scenarioFile, outputFile);
            else
                controller.RunScenario(scenarioFile, outputFile, targetPort);
        }

        private string ExportSessionsAsWcatScript(Session[] sessions)
        {
            string tempFile = Path.GetTempFileName();
            string scenarioFileName = Path.GetFileNameWithoutExtension(tempFile) + ".wcat";
            string scenarioFilePath = Path.Combine(Path.GetTempPath(), scenarioFileName);

            Dictionary<string, object> options = new Dictionary<string, object>(1);
            options["Filename"] = scenarioFilePath;

            if (!FiddlerApplication.DoExport("WCAT Script", sessions, options, null))
                return null;

            return scenarioFilePath;
        }
    }
}