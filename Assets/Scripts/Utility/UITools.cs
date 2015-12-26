using UnityEngine;
using System.Collections;
using System;
using System.IO;

using SimpleFramework;
using System.Text;

public delegate void FuncObj(object obj);
public static partial class UITools
{
    public static string[] keys = { "@", "$" };
    public static string[] values = { "UITools.", "inst" };

    #region  Debug 重构
    public static FuncObj log
    {
        get
        {
            if (AppConst.DebugMode) return Debug.Log;
            else return (str) => { };
        }
    }
    public static FuncObj logError
    {
        get
        {
            if (AppConst.DebugMode) return Debug.LogError;
            else return (str) => { };
        }
    }
    public static FuncObj logWarning
    {
        get
        {
            if (AppConst.DebugMode) return Debug.LogWarning;
            else return (str) => { };
        }
    }

    public static void Log(System.Object o)
    {
        log(o);
    }
    public static void LogError(System.Object o)
    {
        logError(o);
    }
    public static void LogWarning(System.Object o)
    {
        logWarning(o);
    }
#endregion

    public static T Get<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
            t = go.AddComponent<T>();
        return t;
    }

    #region 打开或者关闭面板

    public static void ShowPanel(LuaBehaviour lb)
    {
        ShowPanel(lb ,1);
    }

    public static void ClosePanel(LuaBehaviour lb)
    { 
        ClosePanel(lb ,PlayerSettingMgr.Instance.PanelDuration);
    }

    public static void ShowPanel(LuaBehaviour lb ,float duration)
    {
        HandlePanel(lb, true, PlayerSettingMgr.Instance.PanelDuration);
    }

    public static void ClosePanel(LuaBehaviour lb, float duration )
    {
        HandlePanel(lb, false, duration);
    }


    public static void HandlePanel(LuaBehaviour lb, bool isShow ,float duration)
    {
       // float alpha = isShow ? 1 : 0;
        if (isShow)
        {
            UIWidget widget = lb.GetComponent<UIWidget>();
            if (widget != null)
            {
                widget.alpha = widget.alpha >= 0.8f ? 0 : widget.alpha;
            }
            UIPanel panel = lb.GetComponent<UIPanel>();
            if (panel != null)
            {
                panel.alpha = panel.alpha >= 0.8f ? 0 : panel.alpha;
            }
            SA(lb,true);
            TweenAlpha.Begin(lb.gameObject, duration, 1);
        }
        else
        {
            TweenAlpha.Begin(lb.gameObject, duration, 0).AddOnFinished(() =>
            {
                SA(lb, false);
                UnityEngine.Object.Destroy(lb.GetComponent<TweenAlpha>());
            });
        }

    }
    #endregion

    #region Tween 封装


    public static void TweenPos_X(LuaBehaviour lb, float x)
    {
        Vector3 target = new Vector3(x, lb.transform.localPosition.y, lb.transform.localPosition.z);
        TweenPos(lb.gameObject, target, 0.3f);
    }

    public static void TweenPos_X(LuaBehaviour lb, float x,float duration)
    {
        Vector3 target = new Vector3(x,lb.transform.localPosition.y, lb.transform.localPosition.z);
        TweenPos(lb.gameObject, target, duration);
    }

    public static void TweenPos(GameObject go, Vector3 targetPos, float duration)
    {
        
        TweenPosition tp = TweenPosition.Begin(go, duration, targetPos);
        LuaBehaviour lb = go.GetComponent<LuaBehaviour>();
        if (lb != null)
        {
            tp.AddOnFinished(() =>
            {
                lb.OnCommand("EndTween");
                if (go.GetComponent<TweenPosition>() != null) UnityEngine.Object.Destroy(go.GetComponent<TweenPosition>());
            });
        }
    }
    #endregion


    public static void SA(LuaBehaviour lb, bool isActive)
    {
        if (lb != null)
        {
            lb.gameObject.SetActive(isActive);
        }
    }

    public static LuaBehaviour D(string domain)
    {
        if (LuaBehaviour.Domains.ContainsKey(domain))
        {
            return LuaBehaviour.Domains[domain];
        }
        return null;
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

    public static void ShowMes(string mes)
    {
        GameObject mesPrefab = ResourceManager.Instance.LoadMesPrefab() as GameObject;
        GameObject go = NGUITools.AddChild(GameObject.FindWithTag("JGNGUICamera"), mesPrefab);
        ShowText st = go.GetComponent<ShowText>();
        st.SetText(mes);
    }
}
