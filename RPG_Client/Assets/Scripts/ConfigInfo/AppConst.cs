using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public enum ServerType
{
    TestServer,
}

public struct ServerEntity
{
   public string ip;
   public int port;
    public ServerEntity(string ip, int port)
    {
        this.ip = ip;
        this.port = port;
    }
}

public class AppConst {
    public const bool DebugMode = true;                        //调试模式-用于内部测试

    /// <summary>
    /// 如果想删掉框架自带的例子，那这个例子模式必须要
    /// 关闭，否则会出现一些错误。
    /// </summary>
    public const bool ExampleMode = true;                       //例子模式 

    /// <summary>
    /// 如果开启更新模式，前提必须启动框架自带服务器端。
    /// 否则就需要自己将StreamingAssets里面的所有内容
    /// 复制到自己的Webserver上面，并修改下面的WebUrl。
    /// </summary>
    public const bool UpdateMode = false;                       //更新模式-默认关闭 
    public const bool AutoWrapMode = true;                      //自动添加Wrap模式

    public const bool UsePbc = true;                           //PBC
    public const bool UseLpeg = true;                          //lpeg
    public const bool UsePbLua = true;                         //Protobuff-lua-gen
    public const bool UseCJson = true;                         //CJson
    public const bool UseSproto = true;                        //Sproto
    public const bool LuaEncode = false;                        //使用LUA编码

    public const int TimerInterval = 1;
    public const int GameFrameRate = 30;                       //游戏帧频

    public const string AppName = "SimpleFramework";           //应用程序名称
    public const string AppPrefix = AppName + "_";             //应用程序前缀
    public const string WebUrl = "http://localhost:6688/";      //测试更新地址

    public static string UserId = string.Empty;                 //用户ID
    public static int SocketPort = 0;                           //Socket服务器端口
    public static string SocketAddress = string.Empty;          //Socket服务器地址


    public static string LuaBasePath {
        get { return Application.dataPath + "/uLua/Source/"; }
    }

    public static string LuaWrapPath {
        get { return LuaBasePath + "LuaWrap/"; }
    }
    //======================================================

    public static ServerEntity GetServer(ServerType type)
    {
        switch (type)
        {
            case ServerType.TestServer:
                return testServer;
            default:
                return testServer;
        }
    }
    public static ServerEntity testServer = new ServerEntity("127.0.0.1", 12345);
    public const string MsgTerminator = "#";
    public const string MsgEncoding = "utf-8";
    public static Encoding DefEncoding = Encoding.UTF8;
    public const int MsgHeadLen = 4;
    public const int Max_Msg_Len = 1024;
    public static string TaskListPath = Application.streamingAssetsPath + "/csvFiles/taskList.csv";
    public static string EquipListPath = Application.streamingAssetsPath + "/csvFiles/equipList.csv";
    public static string SkillListPath = Application.streamingAssetsPath + "/csvFiles/skillList.csv";

    private static string _sessionKey;
    public static string SessionKey
    {
        get
        {
            if (!UITools.isValidString(_sessionKey))
            {
                _sessionKey = UITools.GetSessionKey();
            }
            if (UITools.isValidString(_sessionKey))
            {
                return _sessionKey;
            }
            else
            {
                return "NONE";
            }
        }
        set
        {
            _sessionKey = value;
        }
    }
}
