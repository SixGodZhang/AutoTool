# AutoTool

自动化工具是一个基于Unity，整合打包流程的工具

## 工具概览

### 工具界面
[https://github.com/SixGodZhang/AutoTool/blob/master/Images/autotool1.png](https://github.com/SixGodZhang/AutoTool/blob/master/Images/autotool1.png "工具界面")
### 工具源码
[https://github.com/SixGodZhang/AutoTool/blob/master/Images/autotool2.png](https://github.com/SixGodZhang/AutoTool/blob/master/Images/autotool2.png "工具源码")
## 功能描述
该工具建立一套简单的任务流水线,添加任务实现BuildTask抽象类即可.

## 优势
1. **可视化界面**.  可以监控到每个任务的执行状态、执行时间
2. **动态任务配置**.  根据界面提供的选项，可自定义任务流水线
3. **可扩展性高**.  继承实现BuildTask抽象类，即可扩展任务流水线
4. **使用多线程优化了批处理调用**.  解决了Bat缓冲池满Bat阻塞的情况和调用Bat不会卡死Unity的问题
5. **封装了Log日志和文件操作**.