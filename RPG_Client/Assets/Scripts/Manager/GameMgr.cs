using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleFramework;
using System.IO;
using System;
using SimpleFramework.Manager;

public class GameMgr : MonoBehaviour {
    private List<string> downloadFiles = new List<string>();
	public static GameMgr Instance;
	public GameObject go;
	
	void Awake(){
		Instance = this;
      
	}

    private void InitAllManagers()
    {
        //AudioManager
        AudioManager.Instance.Init();
        ResourceManager.Instance.Init();
        EquipMgr.Instance.Init();
        TaskMgr.Instance.Init();
        SkillMgr.Instance.Init();
    }
	
	void Start () {
		go = GameObject.FindWithTag("GameManager");
		DontDestroyOnLoad(go);

        CheckExtractResource(); //释放资源
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = AppConst.GameFrameRate;
      //  ConnectServer();
	}




    public void LoadFightScene()
    {
        LoadScene("FB01");
    }

    public void LoadGame()
    {
        LoadScene("StartScene");
    }

    public void LoadMainCity()
    {
        LoadScene("NewPlayerCity");
    }

    public void LoadScene(string sceneName)
    {
        DontDestroyOnLoad(go);
        AsyncOperation operation = Application.LoadLevelAsync(sceneName);
		LoadingCotroller.instance.ShowLoadinge(operation ,()=>{
            LoadingCotroller.instance.CloseProgress();
        } , "Loading " + sceneName+"……");
    }


    /// <summary>
    /// 释放资源
    /// </summary>
    public void CheckExtractResource()
    {
        bool isExists = Directory.Exists(Util.DataPath) &&
          Directory.Exists(Util.DataPath + "lua/") && File.Exists(Util.DataPath + "files.txt");
        if (isExists || AppConst.DebugMode)
        {
            StartCoroutine(OnUpdateResource());
            return;   //文件已经解压过了，自己可添加检查文件列表逻辑
        }
        StartCoroutine(OnExtractResource());    //启动释放协成 
    }
    IEnumerator OnExtractResource()
    {
        string dataPath = Util.DataPath;  //数据目录
        string resPath = Util.AppContentPath(); //游戏包资源目录

        if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);
        Directory.CreateDirectory(dataPath);

        string infile = resPath + "files.txt";
        string outfile = dataPath + "files.txt";
        if (File.Exists(outfile)) File.Delete(outfile);

        string message = "正在解包文件:>files.txt";
        Debug.Log(message);
        // facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);

        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;

            if (www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
            yield return 0;
        }
        else File.Copy(infile, outfile, true);
        yield return new WaitForEndOfFrame();

        //释放所有文件到数据目录
        string[] files = File.ReadAllLines(outfile);
        foreach (var file in files)
        {
            string[] fs = file.Split('|');
            infile = resPath + fs[0];  //
            outfile = dataPath + fs[0];

            message = "正在解包文件:>" + fs[0];
            Debug.Log("正在解包文件:>" + infile);
            //  facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);

            string dir = Path.GetDirectoryName(outfile);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            }
            else
            {
                if (File.Exists(outfile))
                {
                    File.Delete(outfile);
                }
                File.Copy(infile, outfile, true);
            }
            yield return new WaitForEndOfFrame();
        }
        message = "解包完成!!!";
        // facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);

        yield return new WaitForSeconds(0.1f);
        message = string.Empty;
        //释放完成，开始启动更新资源
        StartCoroutine(OnUpdateResource());
    }

    /// <summary>
    /// 启动更新下载，这里只是个思路演示，此处可启动线程下载更新
    /// </summary>
    IEnumerator OnUpdateResource()
    {
        LoadingCotroller.instance.ShowLoadinge(()=>{
            LoadingCotroller.instance.CloseProgress();
            LoadingCotroller.instance.gameObject.SetActive(true);
            Debug.Log("------");
        });

        downloadFiles.Clear();

        if (!AppConst.UpdateMode)
        {
            //TODOResManager.initialize(OnResourceInited);
            yield break;
        }
        string dataPath = Util.DataPath;  //数据目录
        Debug.LogWarning(dataPath);
        string url = AppConst.WebUrl;
        string random = DateTime.Now.ToString("yyyymmddhhmmss");
        string listUrl = url + "files.txt";
        Debug.LogWarning("LoadUpdate---->>>" + listUrl);

        WWW www = new WWW(listUrl); yield return www;
        if (www.error != null)
        {
            OnUpdateFailed(string.Empty);
            yield break;
        }
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }

        string filesText = www.text;
        string localFiles = "";
        string oldFileList = dataPath + "files.txt";
        if (File.Exists(oldFileList)) localFiles = File.ReadAllText(oldFileList);
        List<string> needUpdataList = GetNeedUpdataFiles(localFiles.Split('\n') , filesText.Split('\n'));

        string message = string.Empty;

        for (int i = 0; i < needUpdataList.Count; i++)
        {
            Debug.Log(needUpdataList[i]);
            LoadingCotroller.instance.UpdateProgress((float)i / (float)needUpdataList.Count, "Download File : " + needUpdataList[i]);
            string filePath = dataPath + needUpdataList[i];
            if (File.Exists(filePath)) File.Delete(filePath);
            string foldPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(foldPath))
            {
                Directory.CreateDirectory(foldPath);
            }
            www = new WWW(url + needUpdataList[i]); yield return www;
            if (www.error != null)
            {
                OnUpdateFailed(url + needUpdataList[i]);
                yield break;
            }
            File.WriteAllBytes(filePath, www.bytes);
        }

        File.WriteAllText(oldFileList, filesText);
        LoadingCotroller.instance.UpdateProgress(1, "Download Complete");
        yield return new WaitForSeconds(1);
        OnResourceInited();

    }

    private List<string> GetNeedUpdataFiles(string[] localFiles, string[] filesText)
    {
        List<string> needUpdataList = new List<string>();
        Dictionary<string, string> dicLocalFiles = new Dictionary<string, string>();
        for (int i = 0; i < localFiles.Length && localFiles[i].Contains("|"); i++)
		{
            string[] strs = localFiles[i].Split('|');
		    dicLocalFiles.Add(strs[0] , strs[1]);
		}

        for (int i = 0; i < filesText.Length && filesText[i].Contains("|"); i++)
        {
            string[] strs = filesText[i].Split('|');
            if ((dicLocalFiles.ContainsKey(strs[0]) && strs[1] != dicLocalFiles[strs[0]])||
                !dicLocalFiles.ContainsKey(strs[0]))
            {
                needUpdataList.Add(strs[0]);
            }
        }
        dicLocalFiles.Clear();
        dicLocalFiles = null;
        return needUpdataList;
    }


    /// <summary>
    /// 资源初始化结束
    /// </summary>
    public void OnResourceInited()
    {
         //初始化完成
        ConnectServer();
    }

    void OnUpdateFailed(string file)
    {
        string message = "更新失败!>" + file;
        //  facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
    }

    public void ConnectServer()
    {
        NetworkMgr.instance.Connect(AppConst.GetServer(ServerType.TestServer), OnConnect);
    }
    public void OnConnect()
    {
        InitAllManagers();
        LoadGame();
    }
}
