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
        //增加注释 测试
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

            newProjectPath = CmdReader.ReadLine("请输入新项目路径：", input => Directory.Exists(input));

            newProjectName = CmdReader.ReadLine("请输入新项目名称：");

            Console.WriteLine("正在处理...");

            replaceRegex = new Regex(
                srcProjectName,
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

            //Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            newProjectPath = Path.Combine(newProjectPath, newProjectName);

            Replace(srcProjectPath);

            Console.WriteLine($"完成！\r\n新的项目保存在：{newProjectPath}");
            Console.Write("请按任意键退出！");
            Console.ReadKey();
        }

        static void Replace(string srcDirectory)
        {
            string name = Path.GetFileName(srcDirectory);
            if (config.IgnoreFolders.Contains(name))
                return;

            bool copyFolderFlag = config.CopyFolders.Contains(name);

            if (!copyFolderFlag)
            {
                var directories = Directory.GetDirectories(srcDirectory);
                if (directories.Length > 0)
                    Array.ForEach(directories, dir => Replace(dir));
            }

            var files = copyFolderFlag ? Directory.GetFiles(srcDirectory, "*.*", SearchOption.AllDirectories) : Directory.GetFiles(srcDirectory);
            foreach (var file in files)
            {
                if (config.IgnoreExtensions.Contains(Path.GetExtension(file)))
                    continue;

                var destFile = file.Replace(srcProjectPath, newProjectPath).Replace(srcProjectName, newProjectName);
                if (copyFolderFlag)
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
            var encoding = GetEncoding(srcFile);

            Directory.CreateDirectory(Path.GetDirectoryName(destFile));

            var srcText = File.ReadAllText(srcFile, encoding);
            var destText = replaceRegex.Replace(srcText, newProjectName);

            File.WriteAllText(destFile, destText, encoding);
        }

        static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open)) file.Read(bom, 0, 4);

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }
    }
}
