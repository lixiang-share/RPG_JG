using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerAnimatorMgr : MonoBehaviour {
	
	public string runFlag = "Move";
    public string hitFlag = "Hit";
    public string dieFlag = "Die";
	public Animator animator;
	private List<string> anim_names;
	// Use this for initialization
	void Start () {
		if(animator == null) animator = GetComponent<Animator>();
        anim_names = new List<string>();
        anim_names.Add(runFlag);
        anim_names.Add(hitFlag);
        anim_names.Add(dieFlag);
        Rest();
	}
	
	public void Rest(){
		for (int i = 0; i < anim_names.Count; i++) {
			animator.SetBool(anim_names[i], false);
		}
	}
	
	public void PlayerRun(){
     //   Rest();
		animator.SetBool(runFlag , true);
	}

    public bool IsClipPlay(string name)
    {
        GameTools.LogError(animator.GetLayerName(0));
        bool rst = animator.GetAnimatorTransitionInfo(0).IsName("Base Layer.Idle");
       GameTools.LogError(name.ToLower() + " : " + rst);
       return rst;
    }
    public void PlayClip(string clipFlag)
    {
        animator.SetBool(clipFlag,true);
    }
}
