using UnityEngine;
using System.Collections;
using System;
using System.IO;

using SimpleFramework;
using System.Text;

public delegate void FuncStr(string mes); 
public class UITools{
    public static string[] keys = { "@", "$" };
    public static string[] values = { "UITools.", "inst" };

    #region  Debug 重构
    public static FuncStr log
    {
        get
        {
            if (AppConst.DebugMode) return Debug.Log;
            else return (str) => { };
        }
    }
    public static FuncStr logError
    {
        get
        {
            if (AppConst.DebugMode) return Debug.LogError;
            else return (str) => { };
        }
    }
    public static FuncStr logWarning
    {
        get
        {
            if (AppConst.DebugMode) return Debug.LogWarning;
            else return (str) => { };
        }
    }

    public static void Log(string mes)
    {
        log(mes);
    }
    public static void LogError(string mes)
    {
        logError(mes);
    }
    public static void LogWarning(string mes)
    {
        logWarning(mes);
    }
#endregion

    public static T Get<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
            t = go.AddComponent<T>();
        return t;
    }

    public void TweenPos(LuaBehaviour lb , float duration , Vector3 targetPos)
    {
        //TweenPosition tp = Get<TweenPosition>(lb.gameObject);
        TweenPosition tp = TweenPosition.Begin(lb.gameObject, duration, targetPos);
        tp.AddOnFinished(() =>
        {
            lb.OnCommand("EndTween");
        });
    }

    public static bool isValidString(string str)
    {
        if (str == null || str.Trim().Length == 0)
        {
            return false;
        }
        return true;
    }

    public static string GetLuaPathInEditor()
    {
        string path = Application.dataPath + "/lua";
        return path;
    }

    public static bool isLuaFileExits(string filename)
    {
        return File.Exists(Util.LuaPath(filename));
    }

    public static void Compile(string filename)
    {
        if (File.Exists(filename))
        {
            string content = File.ReadAllText(filename);
            for (int i = 0; i < keys.Length; i++)
            {
                content = content.Replace(keys[i], values[i]);
            }
            File.WriteAllText(filename, content, Encoding.UTF8);
        }
    }
}
