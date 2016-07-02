using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace JC.DataAccess.IntegrationTests
{
    internal static class SqlScript
    {
        public static IEnumerable<string> Parse(string scriptText)
        {
            var buffer = new StringBuilder();
            using (TextReader scriptReader = new StringReader(scriptText))
            {
                string line = null;
                while ((line = scriptReader.ReadLine()) != null)
                {
                    if (Regex.IsMatch(line, @"^\s*GO\s*$"))
                    {
                        yield return buffer.ToString();
                        buffer.Length = 0;
                    }
                    else
                    {
                        buffer.AppendLine(line);
                    }
                }
            }
            if (buffer.Length > 0)
            {
                yield return buffer.ToString();
            }
        }
    }
}
