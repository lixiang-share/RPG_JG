using UnityEngine;
using System.Collections;
public enum PlayerState{Move,Idle,Fight}

public class PlayerFightCtrl : MonoBehaviour {
	public static PlayerFightCtrl Instance;
	
	
	public float speed = 10;
	public float minResponseVal = 0.005f;
	public bool isAbleMove = true;
	public PlayerState curState = PlayerState.Idle;
	public PlayerAnimatorMgr animMgr;
    public SimpleMoveCtrl moveCtrl;

    void Start () {
		Instance = this;
		if(animMgr == null) animMgr = GetComponent<PlayerAnimatorMgr>();
        if (moveCtrl == null) moveCtrl = GetComponent<SimpleMoveCtrl>();
	}
	
	void Update () {
		if(isAbleMove)
			PlayerMove();
	}
	
	public void PlayerMove(){
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
        if((Mathf.Abs(v) < minResponseVal && Mathf.Abs(h) < minResponseVal) || !isAbleMove)
        {
            moveCtrl.ResetState();
            animMgr.Rest();
        }
        else {
            moveCtrl.Move( h * speed,v * speed);
            animMgr.PlayerRun();
        }
    }
}
