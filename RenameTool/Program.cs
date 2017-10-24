using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RenameTool
{
    class Program
    {
        static string srcProjectPath; // 原项目路径
        static string srcProjectName; // 原项目名称
        static string newProjectPath; // 新项目路径
        static string newProjectName; // 新项目名称

        static Configuration config;
        static Regex replaceRegex;

        static void Main(string[] args)
        {
            Console.Title = "项目重命名工具";

            config = Configuration.Build();

            srcProjectPath = CmdReader.ReadLine("请输入原项目路径：", input => Directory.Exists(input));
            srcProjectName = CmdReader.ReadLine("请输入原项目名称：");
            newProjectName = CmdReader.ReadLine("请输入新项目名称：");

            replaceRegex = new Regex(
                srcProjectName,
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

            newProjectPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                newProjectName);

            Replace(srcProjectPath);

            Console.WriteLine($"操作完成！\r\n新的项目保存在：{newProjectPath}");
            Console.Write($"请按任意键退出！");
            Console.ReadKey();
        }

        static void Replace(string srcDirectory)
        {
            if (config.IgnoreFolders.Contains(Path.GetFileName(srcDirectory)))
                return;

            var directories = Directory.GetDirectories(srcDirectory);
            if (directories.Length > 0)
                Array.ForEach(directories, dir => Replace(dir));

            var files = Directory.GetFiles(srcDirectory);
            foreach (var file in files)
            {
                if (config.IgnoreExtensions.Contains(Path.GetExtension(file)))
                    continue;

                var destFile = file
                    .Replace(srcProjectPath, newProjectPath)
                    .Replace(srcProjectName, newProjectName);

                ReplaceFile(file, destFile);
            }
        }

        static void ReplaceFile(string srcFile, string destFile)
        {
            Console.WriteLine($"From：{srcFile}\r\nTo：{destFile}");

            Directory.CreateDirectory(Path.GetDirectoryName(destFile));

            var srcText = File.ReadAllText(srcFile, Encoding.UTF8);
            var destText = replaceRegex.Replace(srcText, newProjectName);

            File.WriteAllText(destFile, destText, Encoding.UTF8);
        }
    }
}
