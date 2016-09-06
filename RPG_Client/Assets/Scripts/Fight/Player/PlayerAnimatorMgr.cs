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

public delegate bool ClipFinishCondtion();
public class PlayerAnimatorMgr : FightGOBase
{

    public ClipInfo runClip;
    public ClipInfo hitClip;
    public ClipInfo dieClip ;
    public ClipInfo skillOneClip;
    public ClipInfo skillTwoClip;
    public ClipInfo skillThreeClip;
    public ClipInfo skillBasicClip;
    public ClipInfo skillBasicAttackClip02;
    public ClipInfo skillBasicAttackClip03;

	public Animator animator;

    public string curClipName = "idle";
    public static string defClipName = "idle";
	private Dictionary<string,ClipInfo> allClipDict;
	// Use this for initialization
	void Start () {
		if(animator == null) animator = GetComponent<Animator>();
        allClipDict = new Dictionary<string, ClipInfo>();

        addClip(runClip)
            .addClip(hitClip)
            .addClip(dieClip)
            .addClip(skillOneClip)
            .addClip(skillTwoClip)
            .addClip(skillThreeClip)
            .addClip(skillBasicClip)
            .addClip(skillBasicAttackClip02)
            .addClip(skillBasicAttackClip03);


        //allClipDict.Add(runClip.Name , runClip);
        //allClipDict.Add(hitClip.Name,hitClip);
        //allClipDict.Add(dieClip.Name,dieClip);
        //allClipDict.Add(skillOneClip.Name, skillOneClip);
        //allClipDict.Add(skillTwoClip.Name,skillTwoClip);
        //allClipDict.Add(skillThreeClip.Name,skillThreeClip);
        //allClipDict.Add(skillBasicClip.Name,skillBasicClip);
        //allClipDict.Add(skillBasicAttackClip02.Name,skillBasicAttackClip02);
        //allClipDict.Add(skillBasicAttackClip03.Name,skillBasicAttackClip03);
        Reset();
	}
    private PlayerAnimatorMgr addClip(ClipInfo clipInfo)
    {
        if (UITools.isValidString(clipInfo.Name))
        {
            allClipDict.Add(clipInfo.Name, clipInfo);
        }
        return this;
    }
	
	public void Reset(){
        foreach (var item in allClipDict.Values)
        {
              animator.SetBool(item.ConFlag, false);
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

    public ClipInfo GetClipInfoByName(string clipFlag)
    {
        ClipInfo clip = null;
        allClipDict.TryGetValue(clipFlag,out clip);
        return clip;
    }

    public void PlayClip(string clipName, DefAction OnFinish = null, ClipFinishCondtion condtion = null)
    {
        GameTools.Log("PlayClip : " + clipName);
        ClipInfo clip = GetClipInfoByName(clipName);
        GameTools.Log("PlayClip Flag : " + clip.ConFlag);

        string clipFlag = clip.ConFlag;
        animator.SetBool(clipFlag,true);
        curClipName = clip.Name;
        WaitForSec(clip.Duration , ()=> {
            if (OnFinish != null)
            {
                if (condtion != null)
                {
                    if (condtion()) OnFinish();
                }
                else
                {
                    OnFinish();
                }
            }
        });
    }
}
