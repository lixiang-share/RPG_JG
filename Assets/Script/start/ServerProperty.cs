using UnityEngine;
using System.Collections;

public class ServerProperty : MonoBehaviour {

    public string ip="127.0.0.1:9080";
    private string _name;
    public string name {
        set {
            transform.Find("Label").GetComponent<UILabel>().text = value;
            _name = value;
        }
        get {
            return _name;
        }
    }

    public int count=100;

    public void OnPress(bool isPress) {
        if (isPress == false) {
            //选择了当前的服务器
            transform.root.SendMessage("OnServerselect",this.gameObject);
        }
    }
	
}
