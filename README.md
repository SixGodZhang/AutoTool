# 《AutoTool》

![项目进度](http://progressed.io/bar/28?title=progress)

Unity 4.7.2f1(Window) | Unity 5.6.4p1 (Window)
----------------------|-----------------------
![master](https://img.shields.io/travis/SixGodZhang/AutoTool.svg) | ![master](https://img.shields.io/travis/:user/:repo/:branch.svg)


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
