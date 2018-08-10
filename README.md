# ColaFrameWork  

### ColaFrameWork 自研Unity客户端框架  
架构阶段，暂时没有项目使用过。  

### 框架简介  
ColaFramework 一款简洁的MVC结构的Unity客户端框架，尤其支持快速开发3D战斗卡牌和mmo类型的游戏。目前暂时还不支持lua开发和热更，下一步准备完善这方面的内容，希望有兴趣的可以一起来维护。  

### 项目手册  

#### 项目结构  
![](./Doc/image/ColaFramework目录结构.png)  

#### 游戏启动运行流程与机制  

目前框架根据平台的不同，设计了不同的启动场景，分别命名为：  
>* GameLauncher(Windows平台)  
>* GameLauncherAndroid(Android平台)  

无论启动场景是哪一个，游戏都由GameLauncher启动器脚本启动。GameLauncher为一个单例类，在游戏初始化的过程中，启动器类负责初始化GameManager、InputMgr、FpsHelper、DebuglogHelper、U3DAutomation、StreamingAssetHelper、多线程工具类等一些基础的操作。  
在一些必要的资源准备妥当以后，会执行InitGameCore()方法，该方法会调用GameManager的InitGameCore()方法，进一步地进行初始化一些次要的管理器与资源。
在GameLauncher类中，会主动调用GameManager的Update、LateUpdate、FixedUpdate、ApplicationQuit等类似于MonoBehaivor中的方法，这让您依然可以自由地在这些方法中进行处理自己的相应逻辑。

#### 数据节点管理   

* CSV&C#版数据节点管理  
策划使用Excel表进行数据的配置，然后通过[转表工具](https://github.com/XINCGer/Unity3DTraining/tree/master/XlsxTools/xls2csv)(位于ColaFrameWork\Tools\XlsxTools\xls2csv目录下，内有详细的使用指南)将Excel表转为csv结构的数据结构供客户端读取。  
每一份csv数据表，都应对应一个数据解析类以及一个数据集合类，其中数据解析类用于解析并存储csv中的单条数据（一行），数据集合类将存储多个数据解析类的对象。  
数据解析类都应继承自 **LocalDataBase** (本地数据表中的单条数据的抽象基类)：  
```C#
/// <summary>
/// 本地数据表中的单条数据的抽象基类
/// </summary>
public abstract class LocalDataBase
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="strData"></param>
    public abstract void InitWithStr(string strData, char splitChar = ',');

    /// <summary>
    /// 得到Int数据
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    protected int GetInt(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            return int.Parse(str);
        }

        return 0;
    }

    /// <summary>
    /// 得到float数据
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    protected float GetFloat(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            return float.Parse(str);
        }

        return 0;
    }

    /// <summary>
    /// 得到bool数据
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    protected bool GetBool(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            return bool.Parse(str);
        }

        return false;
    }
}

```
**LocalDataBase** 该抽象类提供了一些基本的数据类型转换方法，id字段用来存储索引id，InitWithStr接口用来解析数据，将数据表中的单行文本传入到该接口以后，通过splitChar标识将数据进行拆分、解析。框架中已经包含了**I18NData**解析类，编写其他数据解析类时可以参考此类。  
数据集合类都应实现 **ILocalDataMapBase** 接口(DataMap，数据集合的接口，实现该接口用于管理LocalDataBase)  
```C#
/// <summary>
/// DataMap，数据集合的接口，实现该接口用于管理LocalDataBase
/// </summary>
public interface ILocalDataMapBase
{
    /// <summary>
    /// 对多行字符串进行处理，保存成LocalDataBase集合
    /// </summary>
    /// <param name="rows"></param>
    void SetMapCsv(string[] rows);
}
```  
**ILocalDataMapBase** 接口内只有一个 **void SetMapCsv(string[] rows)** 抽象方法，该方法用来对多行字符串进行处理，保存成LocalDataBase集合。
框架中已经包含了**I18NDataMap**数据集合类，编写其他数据集合类时可以参考此类。  

* LuaConfig&Lua版数据结构管理  

同样本框架也支持将Excel表格数据转为lua形式的配置文件，这样便于接入Lua开发以后数据的访问，提高访问效率。  
Lua格式的转表工具位于[Lua转表工具](./Tools/XlsxTools/xls2lua),其中config配置文件用来配置原始数据的目录和导出的Lua配置文件的目录
如果没有特殊需求的话，无需变更配置文件，直接执行同级目录下的Xls2Lua.exe工具即可进行转表操作。  
#### 事件/消息处理中心(Controller层)    

待补充  

#### UI框架(View层)   
* UI工作流程  
![](./Doc/image/ColaFramework框架UI工作流程.png)  
* UI主要方法  
![](./Doc/image/UIBase基类.png)  

#### SubSys系统框架(Module层)  
系统基类为ModuleBase，游戏内所有的系统的业务逻辑层均继承自该类。框架采用MVC结构，弱化了C层的作用，强化了M层。游戏中的业务逻辑大部分都放在M层处理

#### Execl策划表格规范与转表  

待补充  

#### 资源管理  

待补充  

#### 编辑器拓展与工具  
待补充  

#### 游戏自动化测试  
框架中集成了WETEST_SDK，通过使用[GAutomator](https://github.com/Tencent/GAutomator)实现Unity手游的UI自动化测试。  

#### DebugLog与日志  
待补充  

#### 网络通信与协议  
待补充  

#### 插值动画/缓动动画  
ColaFramework框架集成了DoTween、iTween两款比较常见的插值动画/缓动动画插件，并且自己拓展封装了适合UGUI的UGUITween组件，可以像NGUI中的Tween一样使用，非常方便，用来解决一些简单的Tween动画效果再合适不过了。  
其中拓展UGUITween组件包含以下几个常用的Tween组件：  
>* UGUITweenScale  
>* UGUITweenPosition  
>* UGUITweenAlpha  
>* UGUITweenRotation  





