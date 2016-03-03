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


    #region LuaBehavior 的一些方法支持
    public static T Get<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
            t = go.AddComponent<T>();
        return t;
    }

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

    public static void ShowMsg(string mes)
    {
        //GameObject mesPrefab = ResourceManager.Instance.LoadMesPrefab() as GameObject;
        //GameObject go = NGUITools.AddChild(GameObject.FindWithTag("JGNGUICamera"), mesPrefab);
        //go.transform.localPosition = new Vector3(0, 60, 0);
        //ShowText st = go.GetComponent<ShowText>();
        ShowText st = D("ShowMsg") as ShowText;
        st.SetText(mes);
    }

    public static void Set(GameObject go, string name)
    {
        if (go == null) return;
        if (go.GetComponent<UILabel>() != null)
        {
            go.GetComponent<UILabel>().text = name;
        }
        else if (go.GetComponent<UISprite>() != null)
        {
            go.GetComponent<UISprite>().spriteName = name;
        }
        else if (go.GetComponent<UITexture>() != null)
        {
            go.GetComponent<UITexture>();
        }
    }
    public static void Set(LuaBehaviour lb, string name)
    {
        if(lb != null)
            Set(lb.gameObject, name);
    }




    #endregion

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

    public static void TweenPos_Y(LuaBehaviour lb, float y)
    {
        Vector3 target = new Vector3(lb.transform.localPosition.x, y, lb.transform.localPosition.z);
        TweenPos(lb.gameObject, target, 0.3f);
    }

    public static void TweenPos_Y(LuaBehaviour lb, float y, float duration)
    {
        Vector3 target = new Vector3(lb.transform.localPosition.x, y, lb.transform.localPosition.z);
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

    public static void Tween_Scale(LuaBehaviour lb, float x, float y)
    {
        Tween_Scale(lb, x, y, lb.transform.localScale.z, 0.2f);
    }

    public static void Tween_Scale(LuaBehaviour lb, float x, float y, float z, float duration)
    {
        if (lb == null)
        {
            logError("Tween_Scale Error ,LuaBehaviour is null ");
            return;
        }
        GameObject go = lb.gameObject;
       TweenScale ts = TweenScale.Begin(go, duration, new Vector3(x, y, z));
       ts.AddOnFinished(() => {
           lb.OnCommand("EndTweenScale");
           if (go.GetComponent<TweenScale>() != null) UnityEngine.Object.Destroy(go.GetComponent<TweenScale>());
       });

    }
    #endregion

    #region string 方法扩展
    public static bool isValidString(string str)
    {
        if (str == null || str.Trim().Length == 0)
        {
            return false;
        }
        return true;
    }
    #endregion

    #region PlayerPrefs 封装

    public static void StoreSessionKey(string sessionKey){
        AppConst.SessionKey = sessionKey;
        StoreString("SessionKey", sessionKey);
    }
    public static string GetSessionKey()
    {
        return GetString("SessionKey");
    }
    public static void StoreString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    public static void StoreInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public static void StoreFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static string GetString(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetString(key);
        }
        else
        {
            return "";
        }
        
    }

    public static int GetInt(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
           return PlayerPrefs.GetInt(key);
        }
        else
        {
            return 0;
        }
    }
    public static float GetFloat(string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        else
        {
            return 0;
        }
    }
    #endregion

    #region 一些实体类的转换方法
    public static IList MsgToServerList(MsgUnPacker unpacker)
    {
        return ConvertUitls.MsgToServerList(unpacker);
    }
    public static IList MsgToRoleList(MsgUnPacker unpacker)
    {
        return ConvertUitls.MsgToRoleList(unpacker);
    }
    public static IList MsgToTaskList(MsgUnPacker unpacker)
    {
        return ConvertUitls.MsgToTaskList(unpacker);
    }

    #endregion

}
