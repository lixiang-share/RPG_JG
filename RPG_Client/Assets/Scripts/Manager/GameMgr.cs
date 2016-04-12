using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour {
	public static GameMgr Instance;
	public GameObject go;
	
	void Awake(){
		Instance = this;
        InitAllManagers();
	}

    private void InitAllManagers()
    {
        //AudioManager
        AudioManager.Instance.Init();
        ResourceManager.Instance.Init();
    }
	
	void Start () {
		go = GameObject.FindWithTag("GameManager");
		DontDestroyOnLoad(go);
        ConnectServer();
	}
	
	public void ConnectServer(){
        NetworkMgr.instance.Connect( AppConst.GetServer(ServerType.TestServer), OnConnect);
    }
    public void OnConnect()
    {
        UITools.log("GameMgr == > OnConnect");
        LoadGame();
    }
    public void LoadGame()
    {
        LoadScene("StartScene");
    }

    public void LoadScene(string sceneName)
    {
        DontDestroyOnLoad(go);
        AsyncOperation operation = Application.LoadLevelAsync(sceneName);
		LoadingCotroller.instance.ShowLoadinge(operation);
    }


}
