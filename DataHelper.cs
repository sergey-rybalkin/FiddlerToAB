using System;
using System.IO;
using System.Reflection;

namespace FiddlerToWCAT
{
    internal static class DataHelper
    {
        internal const string ScenarioTemplate = "scenario-template.txt";

        internal const string RequestTemplate = "request-template.txt";

        internal static string GetEmbeddedResource(string fileName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            Stream stream = assembly.GetManifestResourceStream("FiddlerToWCAT." + fileName);
            if (null == stream)
                throw new InvalidOperationException("Embedded resource not found: " + fileName);

            StreamReader reader = new StreamReader(stream);

            using (reader)
            {
                return reader.ReadToEnd();
            }
        }
    }
}