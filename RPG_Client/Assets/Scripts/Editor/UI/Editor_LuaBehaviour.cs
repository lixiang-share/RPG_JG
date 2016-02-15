using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

[CustomEditor(typeof(LuaBehaviour), true)]
public partial class Editor_LuaBehaviour : Editor
{
    public static string rootPath = UITools.GetLuaPathInEditor()+"/";
    public static string templetPath = rootPath + "UI/Templet.lua";

    public static LuaBehaviour lb;
    public string rename = "";

    public static string curTablename = "";
    public static string oldTablename = "";

    public static string curFileFullname = "";
    public static string oldFileFullname = "";

  

    public override void OnInspectorGUI()
    {
        lb = (LuaBehaviour)target;

        base.DrawDefaultInspector();

        #region 使用DoString方式执行的节点
        if (target is LuaWithNoFile)
        {
            lb.domain = EditorGUILayout.TextField(new GUIContent("Domain:"), lb.domain);
            return;
        }
        lb.isDoString = EditorGUILayout.Toggle(new GUIContent("DoString?"), lb.isDoString);
        if (lb.isDoString)
        {
            if (lb.doStringLuaFile != null && File.Exists(lb.doStringLuaFile))
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Open File"))
                {
                    OpenLuaFile();
                }
                if (GUILayout.Button("Copy File"))
                {
                    CopyeLuaFile();
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (GUILayout.Button("Create File"))
                {
                    CreateLuaFile();
                }
            }

            lb.domain = TextArea("Domain:", lb.domain);
            lb.lua_OnFirstEnable = TextArea("OnFirstEnable:", lb.lua_OnFirstEnable);
            lb.lua_OnEnable = TextArea("OnEnable:", lb.lua_OnEnable);
            lb.lua_OnDisable = TextArea("OnDisable:", lb.lua_OnDisable);
            lb.lua_OnClick = TextArea("OnClick:", lb.lua_OnClick);
            lb.lua_OnCommand = TextArea("OnCommand:", lb.lua_OnCommand);
            lb.lua_OnReceiveData = TextArea("OnReceiveData:", lb.lua_OnReceiveData);

            return;
        }
        #endregion


        #region 处理是用lua文件执行
        InitFilename(lb);
        //处理因为节点移动而造成的文件与节点不匹配的情况
        if (File.Exists(rootPath + oldFileFullname))
        {
            if (oldFileFullname != curFileFullname || oldTablename != curTablename)
            {
                MoveFile(rootPath + oldFileFullname, rootPath + curFileFullname);
            }
        }
        else
        {
            lb.luaFilename = "";
            lb.tableName = "";
        }
        if (File.Exists(rootPath + curFileFullname))
        {
            lb.luaFilename = curFileFullname;
            lb.tableName = curTablename;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open File"))
            {
                OpenFile(rootPath + curFileFullname);
            }
            if (GUILayout.Button("Delete File"))
            {
                Rect wr = new Rect(500, 200, 200, 100);
                DeleteConfirm window = (DeleteConfirm)EditorWindow.GetWindowWithRect(typeof(DeleteConfirm), wr, true, "widow name");
                window.Show();
            }
            EditorGUILayout.EndHorizontal();

            lb.domain = EditorGUILayout.TextField(new GUIContent("Domain:"), lb.domain);
            EditorGUILayout.TextField(new GUIContent("Filename:"), curFileFullname);
            EditorGUILayout.TextField(new GUIContent("Tablename:"), curTablename);

           
        }
        else
        {
            if (GUILayout.Button("Create File"))
            {
                CreateFile(rootPath + curFileFullname);
            }
        }
        #endregion
    }

    #region 使用lua文件执行的相关辅助方法
    public void MoveFile(string originFile, string targetFile)
    {
        if (File.Exists(targetFile)) CreateFileDir(targetFile);

        string content = File.ReadAllText(originFile);
        content = content.Replace(oldTablename, curTablename);
        File.WriteAllText(targetFile , content , Encoding.UTF8);
        File.Delete(originFile);
        string originDir = Path.GetDirectoryName(originFile);
        if (Directory.GetFiles(originDir) == null || Directory.GetFiles(originDir).Length == 0)
        {
            Directory.Delete(originDir);
        }
        AssetDatabase.Refresh();
    }

    public bool renameFile(string oldName , string newName ,string oldTable ,string newTable)
    {
        if (!UITools.isValidString(newTable))
        {
            return false;
        }
        if (File.Exists(newName))
        {
            Debug.LogError("文件名已存在");
            return false;
        }
        if (!File.Exists(oldName))
        {
            Debug.LogError("文件不存在");
            return false;
        }
        string content = File.ReadAllText(oldName);
        content = content.Replace(oldTable, newTable);
        File.WriteAllText(newName, content, Encoding.UTF8);
        File.Delete(oldName);
        AssetDatabase.Refresh();
        return true;
    }


    public void OpenFile(string fullName)
    {
        System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
        Info.FileName = "sublime";
        Info.Arguments = fullName;
        System.Diagnostics.Process.Start(Info);
    }



    public static void CreateFileDir(string filename)
    {
        if (File.Exists(filename))
        {
            return;
        }
        string p = filename.Substring(0, filename.LastIndexOf('/'));
        if (!Directory.Exists(p))
        {
            Directory.CreateDirectory(p);
        }

    }

    public void CreateFile(string filename)
    {
        CreateFileDir(filename);
        //复制模板的内容
        string content = File.ReadAllText(templetPath);
        content = content.Replace("tableName", curTablename);
        File.WriteAllText(filename, content, Encoding.UTF8);
        AssetDatabase.Refresh();
        OpenFile(filename);
    }


    public void InitFilename(LuaBehaviour lb)
    {

        GameObject go = lb.gameObject;
        string fileFullname = "UI";
        string fileShortname = go.transform.name.Trim().Replace("Clone" , "");
        List<string> ps = new List<string>();

        Transform p = go.transform.parent;
        while (p != null && p.transform.parent != p)
        {
            if (p.name != "Camera" && p.name != "UI Root")
            {
                ps.Add(p.name);
                p = p.parent;
            }
            else
            {
                break;
            }
        }
        for (int i = ps.Count - 1; i >= 0; i--)
        {
            fileFullname = fileFullname + "/" + ps[i];
            fileShortname = fileShortname + "_" + ps[i];
        }
        oldTablename = lb.tableName;
        curTablename = fileShortname;

        oldFileFullname = lb.luaFilename;
        curFileFullname = fileFullname + "/" + curTablename + ".lua";


    }


    public static void DeleteFile()
    {
        if (File.Exists(rootPath + curFileFullname))
        {
            File.Delete(rootPath + curFileFullname);
            lb.tableName = "";
            lb.luaFilename = "";
            AssetDatabase.Refresh();
        }
    }
    #endregion


    //=================================================================================================================


    public static string doStringluaFileRoot = Directory.GetParent(Application.dataPath).FullName + "/LuaFiles";

    public static string[] funcName = {"Domain","OnFirstEnable" ,"OnEnble","OnDisable" , "OnClick" , "OnCommand(param,paramEX)",
                               "OnHold(param)","OnParseData(param)"};

    public static string TextArea(string desc , string content)
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(desc);
        string str = EditorGUILayout.TextArea(content);
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        return str;
    }


    public void CreateLuaFile()
    {
        lb.doStringLuaFile = doStringluaFileRoot + curFileFullname;
        CreateDoStringFile(lb.doStringLuaFile);
        OpenFile(lb.doStringLuaFile);
    }

    public void OpenLuaFile()
    {
        OpenFile(lb.doStringLuaFile);
    }

    public void CopyeLuaFile()
    {
        StringBuilder sb = new StringBuilder();
        string lastFunc = string.Empty;
        string[] contents = File.ReadAllLines(lb.doStringLuaFile);
        foreach (string line in contents)
        {
            if (line.Trim().StartsWith("======"))
            {
                if (lastFunc != string.Empty)
                {
                    if (Application.isPlaying)
                    {
                        if (sb.Length != 0)
                            SetGiuCardInfo(lastFunc, sb.ToString());
                    }
                    else
                    {
                        SetGiuCardInfo(lastFunc, sb.ToString());
                    }
                }
                sb.Remove(0, sb.Length);
                lastFunc = line.Replace('=', ' ').Trim();
            }
            else
            {
                if (line.Trim().Length != 0)
                {
                    sb.Append(System.Environment.NewLine).Append(line);
                }
            }
        }
        if (sb.Length != 0 && lastFunc != string.Empty)
        {
            SetGiuCardInfo(lastFunc, sb.ToString());
        }
    }

    private void SetGiuCardInfo(string lastFunc, string v)
    {
        Debug.Log(lastFunc + " : " + v);
        switch (lastFunc)
        {
            case "Domain":
                lb.domain = v;
                break;
            case "OnFirstEnable":
                lb.lua_OnFirstEnable = v;
                break;
            case "OnEnble":
                lb.lua_OnEnable = v;
                break;
            case "OnDisable":
                lb.lua_OnDisable = v;
                break;
            case "OnClick":
                lb.lua_OnClick = v;
                break;
            case "OnCommand(param,paramEX)":
                lb.lua_OnCommand = v;
                break;
            case "OnHold(param)":
                lb.lua_OnHold = v;
                break;
            case "OnParseData(param)":
                lb.lua_OnReceiveData = v;
                break;
        }
    }

    private static void CreateDoStringFile(string filename)
    {
        CreateFileDir(filename);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < funcName.Length; i++)
        {
            string str = GetGiuCardInfo(funcName[i]);
            if (str != null && str.Length != 0)
            {
                sb.Append(@"=========================  " + funcName[i] + "  ========================")
                    .Append(System.Environment.NewLine)
                    .Append(System.Environment.NewLine)
                    .Append(str)
                    .Append(System.Environment.NewLine)
                    .Append(System.Environment.NewLine);
            }
            else
            {
                sb.Append(@"=========================  " + funcName[i] + "  ========================")
                .Append(System.Environment.NewLine)
                .Append(str)
                .Append(System.Environment.NewLine);
            }
        }
        sb.Append("========================= End =========================");
        File.WriteAllText(filename, sb.ToString(), Encoding.UTF8);
    }
    private static string GetGiuCardInfo(string key)
    {
        switch (key)
        {
            case "Domain":
                return lb.domain;
            case "OnFirstEnable":
                return lb.lua_OnFirstEnable;
            case "OnEnble":
                return lb.lua_OnEnable;
            case "OnDisable":
                return lb.lua_OnDisable;
            case "OnClick":
                return lb.lua_OnClick;
            case "OnCommand(param,paramEX)":
                return lb.lua_OnCommand;
            case "OnHold(param)":
                return lb.lua_OnHold;
            case "OnParseData(param)":
                return lb.lua_OnReceiveData;
        }
        return "";
    }




}






















































class DeleteConfirm : EditorWindow
{
    void OnGUI()
    {
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(new GUIContent("是否确定删除该文件？"));
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Cancel"))
        {
            this.Close();
        }
        if (GUILayout.Button("Delete File"))
        {
            Editor_LuaBehaviour.DeleteFile();
            this.Close();
        }
        EditorGUILayout.EndHorizontal();
    }



















   
}
