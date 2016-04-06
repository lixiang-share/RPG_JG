using UnityEngine;
using System.Collections;

public class SlopeHandler : MonoBehaviour {
    public GameObject heightGO;
    public GameObject lowGO;
    private Vector3 heightPoint;
    private Vector3 lowPoint;
	// Use this for initialization
	void Start () {
        if (heightPoint == null) heightGO = transform.GetChild("height-point");
        if (lowGO == null) lowGO = transform.GetChild("low-point");
        heightPoint = heightGO.transform.position;
        lowPoint = lowGO.transform.position;
	}

    public float GetYOnSlope(Vector3 pos)
    {
        float h = heightPoint.y - lowPoint.y;
        Vector2 A = new Vector2(heightPoint.x, heightPoint.z);
        Vector2 B = new Vector2(lowPoint.x, lowPoint.z);
        Vector2 C = new Vector2(pos.x, pos.z);
        float b = Vector2.Distance(A, C);
        float c = Vector2.Distance(A, B);
        float a = Vector2.Distance(C, B);
        float d = (Mathf.Pow(a, 2) + Mathf.Pow(c, 2) - Mathf.Pow(b, 2)) / (2 * c);
        Vector3 _heightPoint = new Vector3(heightPoint.x , lowPoint.y , heightPoint.z);
        float  _AB = Vector3.Distance(_heightPoint, lowPoint);
        float detaY = d / _AB * h;
        return detaY + lowPoint.y;
    }

    public float GetYLeaveSlop(Vector3 pos)
    {
        float dist2Height = Vector3.Distance(pos, heightPoint);
        float dist2Low = Vector3.Distance(pos, lowPoint);
        return dist2Height < dist2Low ? heightPoint.y : lowPoint.y;
    }
}
