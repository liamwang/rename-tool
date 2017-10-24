using System.Collections.Generic;
using System.Linq;

namespace RenameTool
{
    public static class Extensions
    {
        public static IEnumerable<string> TrimSplit(this string text, params char[] separator)
        {
            return text.Split(separator).Select(s => s.Trim());
        }
    }
}
