using UnityEngine;
using System.Collections;

public class FightManager : MonoBehaviour {

    private bool isResult = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isResult) return;
        if (PlayerFightCtrl.Instance.isDie)
        {
            GameTools.Log("Lose");
            UITools.D("FightResult").CallLuaMethod("Show", false);
            isResult = true;
        }
        if (EnemyManager.Instance.isOver())
        {
            GameTools.Log("Win");

            UITools.D("FightResult").CallLuaMethod("Show", true);
            isResult = true;
        }
	}
}
