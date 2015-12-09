using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

[CustomEditor(typeof(LuaBehaviour), true)]
public class Editor_LuaBehaviour : Editor
{
    public static string path = UITools.GetLuaPathInEditor()+"/";
    public static string templetPath = path + "UI/Templet.lua";

    public static string tableName;
    public override void OnInspectorGUI()
    {
        LuaBehaviour lb = (LuaBehaviour)target;
        string relativeName = GetFilename(lb.gameObject);
        string fullName = path + relativeName;
        if (!File.Exists(fullName))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Table Name:");
            tableName = lb.tableName = GUILayout.TextField(lb.tableName);
            lb.luaFilename = relativeName;
            if (GUILayout.Button("Create File"))
            {
                CreateFile(fullName);
            }
            GUILayout.EndHorizontal();

        }
    }

    public static void CreateFile(string filename) 
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
        //复制模板的内容
        string content = File.ReadAllText(templetPath);
        File.WriteAllText(filename, content, Encoding.UTF8);
        AssetDatabase.Refresh();
    }

    public static string GetFilename(GameObject go)
    {
        //string sceneName = Path.GetFileName(EditorApplication.currentScene);
        //if (sceneName == null || sceneName.Trim().Length == 0)
        //{
        //    sceneName = "UnScene";
        //}
        //if (sceneName.Contains("."))
        //{
        //    sceneName = sceneName.Substring(0, sceneName.IndexOf("."));
        //}
        //string filename = path + "/" + sceneName;
        string filename = "UI";
        List<string> ps = new List<string>();
        Transform p = go.transform.parent;
        while (p != null && p.transform.parent != p)
        {
            if (p.name != "Camera")
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
            filename = filename + "/" + ps[i];
        }
        return (filename +"/"+tableName+ ".lua").Replace(" ", "");
    }
}
