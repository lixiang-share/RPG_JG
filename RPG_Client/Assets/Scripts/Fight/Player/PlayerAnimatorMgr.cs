using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ClipInfo{
    [SerializeField]
    private string conFlag;
    [SerializeField]
    private float duration;
    [SerializeField]
    private string name;
    public string ConFlag
    {
        get
        {
            return conFlag;
        }

        set
        {
            conFlag = value;
        }
    }

    public float Duration
    {
        get
        {
            return duration;
        }

        set
        {
            duration = value;
        }
    }

    public string Name
    {
        get
        {
            if (name == null || name == "") return ConFlag.ToLower();
            return name;
        }

        set
        {
            name = value;
        }
    }

    public ClipInfo() { }
    public ClipInfo(string conFlag) { this.ConFlag = conFlag; Duration = 5f; }
    public ClipInfo(float duration) { this.Duration = duration; }
    public ClipInfo(string conFlag, float duration) { this.ConFlag = conFlag; this.Duration = duration; }
}

public class PlayerAnimatorMgr : FightGOBase
{

    public ClipInfo runClip;
    public ClipInfo hitClip;
    public ClipInfo dieClip ;
    public ClipInfo skillOneClip;
    public ClipInfo skillTwoClip;
    public ClipInfo skillThreeClip;
    public ClipInfo skillBasicClip;



	public Animator animator;

    public string curClipName = "idle";
    public static string defClipName = "idle";
	private List<ClipInfo> allClips;
	// Use this for initialization
	void Start () {
		if(animator == null) animator = GetComponent<Animator>();
        allClips = new List<ClipInfo>();
        allClips.Add(runClip);
        allClips.Add(hitClip);
        allClips.Add(dieClip);
        allClips.Add(skillOneClip);
        allClips.Add(skillTwoClip);
        allClips.Add(skillThreeClip);
        allClips.Add(skillBasicClip);
        Reset();
	}
	
	public void Reset(){
		for (int i = 0; i < allClips.Count; i++) {
			animator.SetBool(allClips[i].ConFlag, false);
		}
        curClipName = defClipName;
	}
	
	public void PlayRun(){
        if(curClipName != runClip.ConFlag)
        {
            curClipName = runClip.Name;
		    animator.SetBool(runClip.ConFlag , true);
        }
	}

    public bool IsClipPlay(string name)
    {
       bool rst = name.ToLower() == curClipName.ToLower();
        GameTools.Log(name.ToLower() + "=="+ curClipName+" : " + rst);
        return rst;
    }

    public ClipInfo GetClipInfoByFlag(string clipFlag)
    {
        ClipInfo clip = null;
        foreach(ClipInfo c in allClips)
        {
            if (c.ConFlag == clipFlag) { clip = c; break; }
        }
        return clip;
    }

    public void PlayClip(string clipFlag,DefAction OnFinish = null)
    {
        Reset();
        animator.SetBool(clipFlag,true);
        ClipInfo clip = GetClipInfoByFlag(clipFlag);
        curClipName = clip.Name;
        WaitForSec(clip.Duration , ()=> {
            GameTools.Log("Reset");
            Reset();
            if (OnFinish != null) OnFinish();
        });
    }
}
