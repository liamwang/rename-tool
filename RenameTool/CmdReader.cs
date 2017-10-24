using System;

namespace RenameTool
{
    public class CmdReader
    {
        public static string ReadLine(string tipText, Func<string, bool> validate = null)
        {
            var input = string.Empty;
            while (string.IsNullOrWhiteSpace(input) || (validate != null && !validate(input)))
            {
                Console.WriteLine(tipText);
                input = Console.ReadLine();
            }
            return input;
        }
    }
}
