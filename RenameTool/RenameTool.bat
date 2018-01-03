@echo off
if not exist RenameTool.dll (echo 请在发布后的目录下运行此文件，按任意键退出！&pause>nul&exit)
dotnet RenameTool.dll