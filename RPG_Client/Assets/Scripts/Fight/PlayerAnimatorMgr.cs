using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerAnimatorMgr : MonoBehaviour {
	
	private string runName = "Move";
	public Animator animator;
	private List<string> anim_names;
	// Use this for initialization
	void Start () {
		if(animator == null) animator = GetComponent<Animator>();
        anim_names = new List<string>();
		anim_names.Add(runName);
	}
	
	public void Rest(){
		for (int i = 0; i < anim_names.Count; i++) {
			animator.SetBool(anim_names[i], false);
		}
	}
	
	public void PlayerRun(){
		Rest();
		animator.SetBool(runName , true);
	}
}
