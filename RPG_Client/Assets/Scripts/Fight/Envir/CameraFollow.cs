using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public Vector3 relativeInterval;
	public GameObject followGO;

	// Update is called once per frame
	void Update () {
		//GameTools.LogError(followGO.transform.position - transform.position);
		if(followGO != null){
			transform.position = followGO.transform.position + relativeInterval;
		}
	}
}
