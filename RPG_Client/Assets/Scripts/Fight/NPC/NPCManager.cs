using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour {

    public List<GameObject> nps;
    private Dictionary<int, GameObject> npcDict;
    public static NPCManager Instance;
	// Use this for initialization
	void Start () {
        Instance = this;
        npcDict = new Dictionary<int, GameObject>();
	    if(nps != null && nps.Count != 0){
            foreach (GameObject go in nps)
            {
                int id = int.Parse(go.name.Substring(0, 4));
                npcDict.Add(id, go);
            }
        }
	}

    public GameObject GetNPC(int id)
    {
        GameObject go = null;
        npcDict.TryGetValue(id, out go);
        return go;
    }
}
