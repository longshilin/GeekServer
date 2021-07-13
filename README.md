
__QQ交流群 : 228688193__  

# GeekServer介绍：
GeekServer是一个开源的单服结构的游戏服务器框架，采用C# .Netcore开发，开发效率高，性能强，跨平台，并内置不停服热更新机制，支持http tcp。可以满足绝大部分游戏类型的需求，特别是和Unity3D协同开发更佳。  
设计理念:大道至简，以简化繁

# GeekServer功能：

### 1.跨平台  
使用C# .Netcore开发（可以跨平台，可以跨平台，可以跨平台），.Netcore现在功能和性能都已经十分强大和稳健，不管是在windows还是linux上部署起来都很简便。
### 2.全面异步编程  
全部采用异步编程（async/await），让逻辑代码变得整洁优雅，清晰易懂，让代码写起来行如流水。
### 3.TPL(Task Parallel Library) Actor模型  
GeekServer的Actor模型构建于强大的TPL DataFlow之上，让Actor模型如虎添翼。（不了解Actor模型，可以搜一下相关资料，Akka，Orleans都是采用的Actor模型）
### 4.Actor死锁检测 
Actor模型本身是存在死锁的情况，且不容易被发现，~~GeekServer在debug模式下提供了检测机制，让死锁问题暴露在开发过程中~~。GeekServer内部可检测单路死锁和多路死锁，并采用调用链重入机制，最大程度上消除死锁。
### 5.支持不停服更新 
采用组件+状态的设计，状态只有属性，没有方法，组件只用方法，没有属性，并通过代理的方式全部放到热更dll中，运行时重新加载dll即可热更所有逻辑。
### 6.网络模块  
网络模块采用了DotNetty，DotNetty是微软Azure团队，使用C#实现的Netty的版本，性能强劲，功能强大。
### 7.持久化透明  
采用Mongodb作为数据存储，状态的持久化全透明，框架会自动序列化/反序列（并在编译期间动态注入INotifyPropertyChanged，高效过滤非变化的状态）让开发人员更加专注于业务逻辑，无需操心数据库。 
### 8.Timer/Scheduler/Event  
内置线程安全的Timer，Scheduler，Event系统，轻松应对游戏服务器常见的定时，任务计划，事件触发等业务需求。
### 9.定期释放不活跃内存数据  
以功能系统级别的粒度，定期剔除内存中不活跃的玩家数据，尽最大可能减少服务器内存开销。
### 10.高效的通信协议  
通信协议，以扁平数据结构的xbuffer为基础（flatbuffer的简化版），序列化和反序列化效率极高，同时序列化之后的数据极小，数据传输效率很高。(已验证支持ILRunTime热更)
### 11.一键导表工具  
GeekServer包含一个一键导表工具，将策划配置表，转化为二进制数据，并提供了方便快捷的API供游戏调用   

# 运行
1. 安装[.NetCore3.1](https://dotnet.microsoft.com/download/dotnet/3.1)
2. 安装[mongodb4.x](https://www.mongodb.com/try/download/community)
3. 打开git clone本项目https://github.com/leeveel/GeekServer.git
4. 运行Tools/ExcelGen/ExcelConverter.exe 点击[服务器-ALL]导出配置表
5. 用VisualStudio2019打开GeekServer.sln 启动GeekServer.App

# 文档&例子&Demo
[文档](https://github.com/leeveel/GeekServer/tree/master/Docs)  
[Sample](https://github.com/leeveel/GeekServer/tree/master/GeekServer.Hotfix/Sample)  
[UnityDemo_Server](https://github.com/leeveel/GeekServer/tree/master/GeekServer.Hotfix/Demo)  
[UnityDemo_Client](https://github.com/leeveel/GeekServer/tree/master/UnityDemo)[测试版本Unity2019.x，测试平台Android]Client代码未做任何优化和设计，仅用于演示如何与服务器交互  

# TODO  
Actor调用接口化，并自动生成代码，使Actor入队透明化   

# 推荐项目  
[xbuffer](https://github.com/CodeZeg/xbuffer) 一种简化版本的 flatbuffer 序列化库  
[ExcelToCode](https://github.com/leeveel/ExcelToCode) 一键从Excel中导出模板代码和二进制数据  
[cshotfix](https://github.com/qq576067421/cshotfix) Unity基于(ILRunTime)[https://github.com/Ourpalm/ILRuntime]的补丁试热更新架构