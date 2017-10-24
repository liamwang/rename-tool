using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace RenameTool
{
    public class Configuration
    {
        public IEnumerable<string> IgnoreFolders { get; set; } = new HashSet<string>();
        public IEnumerable<string> IgnoreExtensions { get; set; } = new HashSet<string>();

        public static Configuration Build()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            return new Configuration
            {
                IgnoreFolders = config[nameof(IgnoreFolders)].TrimSplit(','),
                IgnoreExtensions = config[nameof(IgnoreExtensions)].TrimSplit(',')
            };
        }
    }
}
