using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour {
	
	public GameObject go;
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

    public void LoadGame(){
        DontDestroyOnLoad(go);
        AsyncOperation operation = Application.LoadLevelAsync("StartScene");
		LoadingCotroller.instance.ShowLoadinge(operation);
	}
}
