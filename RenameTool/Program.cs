using System;
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

            newProjectPath = CmdReader.ReadLine("请输入新项目保存位置：", input => Directory.Exists(input));
            newProjectName = CmdReader.ReadLine("请输入新项目名称：");

            newProjectPath = Path.Combine(newProjectPath, newProjectName);

            Console.WriteLine("正在处理...");

            replaceRegex = new Regex(
                srcProjectName,
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

            // 保存到桌面
            // Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

            Rename(srcProjectPath);

            Console.WriteLine($"完成！\r\n新的项目保存在：{newProjectPath}");
            Console.Write("请按任意键退出！");
            Console.ReadKey();
        }

        static void Rename(string srcDirectory, bool isCopyFolder = false)
        {
            string folderName = Path.GetFileName(srcDirectory);

            if (config.IgnoreFolders.Contains(folderName))
                return;

            isCopyFolder = isCopyFolder || config.CopyFolders.Contains(folderName);

            var directories = Directory.GetDirectories(srcDirectory);
            if (directories.Length > 0)
                Array.ForEach(directories, dir => Rename(dir, isCopyFolder));

            var files = Directory.GetFiles(srcDirectory);
            foreach (var file in files)
            {
                if (config.IgnoreExtensions.Contains(Path.GetExtension(file)))
                    continue;

                var destFile = file
                    .Replace(srcProjectPath, newProjectPath)
                    .Replace(srcProjectName, newProjectName);

                if (isCopyFolder)
                    CopyFile(file, destFile);
                else
                    ReplaceFile(file, destFile);
            }
        }

        static void CopyFile(string srcFile, string destFile)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destFile));
            File.Copy(srcFile, destFile, true);
        }

        static void ReplaceFile(string srcFile, string destFile)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destFile));

            var encoding = Util.GetEncoding(srcFile);
            var srcText = File.ReadAllText(srcFile, encoding);
            var destText = replaceRegex.Replace(srcText, newProjectName);

            File.WriteAllText(destFile, destText, encoding);
        }
    }
}
