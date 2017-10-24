# RenameTool
这是一个基于 .NET Core 2.0 实现的项目重命名工具。

## 如何使用

Clone 此项目后直接运行，然后按下图操作（超简单）：

![截图](https://user-images.githubusercontent.com/5000396/31938101-28e9fa08-b8e9-11e7-852a-1361c65fe28a.gif)

## 如何更好地使用

为了更好地重复使用，可以制作一个可执行脚本。以 Widows 平台为列，可以将此项目 publish 到本地某个文件夹，然后将下面的文本保成 .bat 文件：

```bash
title 项目重命名工具
cd /d <your publish path>
dotnet RenameTool.dll
pause
```
