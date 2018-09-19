# 《AutoTool》

![项目进度](http://progressed.io/bar/50?title=项目进度) 

![master](https://img.shields.io/travis/SixGodZhang/AutoTool.svg) ![Unity版本](https://img.shields.io/badge/platform-2017.3.1f1-green.svg)

## 什么是AutoTool？
AutoTool是一个基于Unity，建立任务流水线的工具

### 工具预览
![工具界面预览](https://github.com/SixGodZhang/AutoTool/blob/master/Images/autotool1.png)
### 工具源码
![工具源码预览](https://github.com/SixGodZhang/AutoTool/blob/master/Images/autotool2.png)
## 功能描述
该工具建立一套简单的任务流水线,添加任务实现BuildTask抽象类即可.

## 优势
1. **可视化界面**.  可以监控到每个任务的执行状态、执行时间
2. **动态任务配置**.  根据界面提供的选项，可自定义任务流水线
3. **可扩展性高**.  继承实现BuildTask抽象类，即可扩展任务流水线
4. **使用多线程优化了批处理调用**.  解决了Bat缓冲池满Bat阻塞的情况和调用Bat不会卡死Unity的问题
5. **封装了Log日志和文件操作**.
6. **任务回滚**. 当流水线失败时，自动将可回滚的任务进行回滚.(比如svn update为不可回滚任务,文件移动为可回滚任务)

## 使用样例
工具栏->Editor->BuildAndroidPipline

## 关于任务回滚说明
只有标记了可回滚的任务才可以回滚,在实现可回滚的任务,只需实现:
```
        bool IsCanReverse { get; }//任务是否可回滚
        void OnReverse();//回滚(当任务失败时执行回滚)
```
测试样例请见 TestReverseTask.cs
测试效果图:
![回滚测试效果](https://github.com/SixGodZhang/AutoTool/blob/master/Images/ReverseTask.png)


## 待完善
模块名称 | 状态
--------|--------
任务动态配置 | :x:
多条流水线切换 | :x:
测试用例 | :o:

## API
接口名称 | 作用 | 示例
--------|--------|--------
ATLog.Log(string content) | 自定义Log内容，输出到%rootproject%\autoToolLogPath.log | ATLog.Log("Hello,World!");
ATLog.Log(ErrorCode errorCode) | 输出预定义Log到%rootproject%\autoToolLogPath.log | ATLog.Log(ErrorCode.NONE);
ATLog.ClearLog() | 清除Log | ATLog.ClearLog();
BatTool.CallBatByThread<T>(string batPath, BuildTask<T> task) | 多线程执行批处理 | BatTool.CallBatByThread(AutoToolConstants.BatDic["svnOP_Update"],this);
BatTool.CallBat(string batPath) | 执行批处理 | BatTool.CallBat("C:\\hello.bat");
FileHelper.ShowExplorerWindow(string filePath) | 根据文件路径打开Windows文件浏览器 | ...
FileHelper.WriteToFile(string filePath, string content, FileMode mode = FileMode.OpenOrCreate) | 将内容写入文件 | ...
FileHelper.MoveFolder(string sPath, string tPath) | 剪切文件夹 | ...
FileHelper.RemoveFullFolder(string path) | 移除文件夹及其子文件 | ...
FileHelper.CopyFile(string sourcePath, string destPath, bool isrewrite = true) | 复制文件 | ...
FileHelper.CopyFolder(string sourcePath, string destPath) | 复制文件夹及其子文件 | ...
SysProgressBar.ShowProgressBar(float current, float total = 100.0f, string taskName = "请稍后...") | 显示Unity内置进度条 | ...
SysProgressBar.ClearProgressBar() | 清除进度条 | ...