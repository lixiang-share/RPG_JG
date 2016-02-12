using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

[CustomEditor(typeof(LuaBehaviour), true)]
public class Editor_LuaBehaviour : Editor
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
        if (target is LuaWithNoFile)
        {
            lb.domain = EditorGUILayout.TextField(new GUIContent("Domain:"), lb.domain);
            return;
        }
        lb.isDoString = EditorGUILayout.Toggle(new GUIContent("DoString?"), lb.isDoString);
        if (lb.isDoString)
        {
            lb.domain = EditorGUILayout.TextField(new GUIContent("Domain:"), lb.domain);
            lb.lua_Awake = EditorGUILayout.TextField(new GUIContent("Awake:"), lb.lua_Awake);
            lb.lua_OnClick = EditorGUILayout.TextField(new GUIContent("OnClick:"), lb.lua_OnClick);
            lb.lua_OnCommand = EditorGUILayout.TextField(new GUIContent("OnCommand:"), lb.lua_OnCommand);
            lb.lua_OnEnable = EditorGUILayout.TextField(new GUIContent("OnDisable:"), lb.lua_OnDisable);
            lb.lua_OnHold = EditorGUILayout.TextField(new GUIContent("OnHold:"), lb.lua_OnHold);
            lb.lua_ReceiveData = EditorGUILayout.TextField(new GUIContent("ReceiveData:"), lb.lua_ReceiveData);
            lb.lua_Start = EditorGUILayout.TextField(new GUIContent("Start:"), lb.lua_Start);
            return;
        }


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
        
    }


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



    public void CreateFileDir(string filename)
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
