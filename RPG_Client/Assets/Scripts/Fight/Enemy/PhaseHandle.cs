using UnityEngine;
using System.Collections;

public class PhaseHandle : MonoBehaviour {

    public bool isBlock;
    public float range = 3;
    public EnemyPhase curPhase = EnemyPhase.FirstPhase;

    private bool isHasSpawn = false;
	// Update is called once per frame
	void Update () {
        if (!isBlock && !isHasSpawn)
        {
            if (Vector3.Distance(PlayerFightCtrl.Instance.transform.position, transform.position) <= range)
            {
                EnemySpawn.Instance.SpawEnemy(curPhase);
                isHasSpawn = true;
            }
        }
	}

    public bool isInRange(Vector3 pos)
    {
        return Vector3.Distance(transform.position, pos) <= range;
    }
}
