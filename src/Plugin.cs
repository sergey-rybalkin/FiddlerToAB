using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using Fiddler;

namespace FiddlerToAB
{
    /// <summary>
    /// Plugin entry point.
    /// </summary>
    [ProfferFormat(FormatName, FormatDescription)]
    public class Plugin : ISessionExporter
    {
        private const string FormatName = "AB batch";

        private const string FormatDescription = "Apache Bench batch file";

        private static readonly string[] HeadersToSkip = new[]
        {
            "Host",
            "Content-Length"
        };

        /// <summary>
        /// Export sessions.
        /// </summary>
        /// <param name="sExportFormat">Format to export into.</param>
        /// <param name="oSessions">The list of sessions to export.</param>
        /// <param name="dictOptions">Configuration options.</param>
        /// <param name="evtProgressNotifications">The event progress notifications.</param>
        public bool ExportSessions(
            string sExportFormat,
            Session[] oSessions,
            Dictionary<string, object> dictOptions,
            EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            string targetFile = GetTargetFileName(dictOptions);
            if (string.IsNullOrEmpty(targetFile))
                return false;

            StreamWriter output = null;

            try
            {
                int counter = 0;
                Encoding utf8WithoutBom = new UTF8Encoding(false);
                output = new StreamWriter(targetFile, false, utf8WithoutBom);

                foreach (Session session in oSessions)
                {
                    WriteSessionToFile(output, session, Path.GetDirectoryName(targetFile));

                    var eventArgs = new ProgressCallbackEventArgs(
                        ++counter / (float)oSessions.Length,
                        $"Exported {counter} session(s).");
                    evtProgressNotifications(null, eventArgs);

                    output.Flush(); // Try to be in consistent state if next iteration fails
                }
            }
            catch (Exception ex) when (IsIOException(ex))
            {
                Log("Failed writing output file:" + ex.Message);
                return false;
            }
            finally
            {
                output.Dispose();
            }

            return true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        public void Dispose() { }

        private static string GetTargetFileName(Dictionary<string, object> dictOptions)
        {
            string targetFile = null;
            if (null != dictOptions && dictOptions.TryGetValue("Filename", out object oTargetFile))
                targetFile = (string)oTargetFile;

            if (string.IsNullOrEmpty(targetFile))
            {
                targetFile = Utilities.ObtainSaveFilename(
                    $"Export As {FormatName}", "Batch Files (*.bat)|*.bat");
            }

            if (string.IsNullOrEmpty(targetFile))
            {
                Log("Target file not selected.");
                return null;
            }

            return targetFile;
        }

        private static bool IsIOException(Exception ex)
        {
            return ex is SecurityException ||
                   ex is UnauthorizedAccessException ||
                   ex is DirectoryNotFoundException ||
                   ex is IOException;
        }

        private static void WriteSessionToFile(StreamWriter output, Session session, string basePath)
        {
            var cmd = new StringBuilder("abs.exe -n 100 -c 5 ", 1024);

            if (session.RequestBody?.Length > 0)
            {
                string payloadFile = $"{session.id}.req";
                File.WriteAllBytes(Path.Combine(basePath, payloadFile), session.RequestBody);

                if (0 == string.Compare("PUT", session.RequestMethod, true))
                    cmd.Append($"-u {payloadFile} ");
                else
                    cmd.Append($"-p {payloadFile} ");
            }

            cmd.Append("-m ").Append(session.RequestMethod).Append(' ');

            foreach (HTTPHeaderItem header in session.RequestHeaders)
            {
                if (HeadersToSkip.Any(h => 0 == string.Compare(h, header.Name, true)))
                    continue;

                if (0 == string.Compare("Content-Type", header.Name, true))
                    cmd.Append($"-T {header.Value} ");
                else
                    cmd.Append($"-H \"{header.Name}: {header.Value}\" ");
            }

            output.WriteLine(cmd.Append(session.fullUrl));
        }

        private static void Log(string message)
        {
            FiddlerApplication.Log.LogString($"[{FormatName}] {message}");
        }
    }
}
