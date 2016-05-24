using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enclosure : MonoBehaviour {
    public List<GameObject> nodes;
    private List<Vector3> encloure;
    public static Enclosure Instance;
    // Use this for initialization
    void Start () {
        Instance = this;
        encloure = new List<Vector3>();
        if(nodes != null && nodes.Count != 0)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                encloure.Add(nodes[i].transform.position);
            }
        }
    }

    public bool isInside(Vector3 pos)
    {
        return isInside(pos.x, pos.z);
    }
	
    public bool isInside(float x , float z)
    {
        bool rst = false;
        for (int i = 0 , j = encloure.Count-1; i < encloure.Count; i++)
        {
            if(
                (encloure[i].z <= z && encloure[j].z>z || 
                encloure[i].z > z && encloure[j].z <= z ) &&
                (encloure[i].x <= x || encloure[j].x <= x) &&
                (encloure[i].x + (z - encloure[i].z) * 
                (encloure[j].x - encloure[i].x) / 
                (encloure[j].z - encloure[i].z) < x )
              )
            {
                rst = !rst;
            }
            j = i;
        }
        return rst;
    }
}
