# RenameTool
这是一个基于 .NET Core 2.x 实现的项目重命名工具。

## 如何使用

Clone 此项目后直接运行，然后根据命令行提示按下图操作：

![效果图](https://user-images.githubusercontent.com/5000396/41758271-26985a08-761a-11e8-8881-1b0c1c9abe9f.png)

## 如何更好地使用

为了更好地重复使用，可以制作一个可执行脚本。以 Widows 平台为列，可以将此项目 publish 到本地某个文件夹，然后将下面的文本保存为 .bat 文件：

```bash
cd /d <your publish path>
dotnet RenameTool.dll
pause
```
