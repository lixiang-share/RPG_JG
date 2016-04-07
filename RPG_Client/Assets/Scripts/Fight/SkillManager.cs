using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SkillManager : MonoBehaviour {
    public string idleClipName = "idle";
    public string runClipName = "run";
    public bool isInit;
    public PlayerAnimatorMgr animMgr;
    public List<SkillItem> skills;
    private Dictionary<int, SkillItem> skillDict;
	void Start () {
        skillDict = new Dictionary<int, SkillItem>();
        if (animMgr == null) animMgr = GetComponent<PlayerAnimatorMgr>();

        List<SkillItem> skills = new List<SkillItem>();
        SkillItem skill = new SkillItem();
        skill.ColdTime = 2;
        skill.Icon = "icon_li";
        skill.Type = "Skill";
        skill.Pos = "one";
        skill.SkillID = 4444;
        skills.Add(skill);

        skill = new SkillItem();
        skill.ColdTime = 1;
        skill.Icon = "iocn_ho";
        skill.Type = "Skill";
        skill.Pos = "two";
        skill.SkillID = 333;
        skills.Add(skill);


        skill = new SkillItem();
        skill.ColdTime = 1;
        skill.Icon = "iocn_fo";
        skill.Type = "Skill";
        skill.Pos = "three";
        skill.SkillID = 222;
        skills.Add(skill);

        skill = new SkillItem();
        skill.ColdTime = 1;
        skill.Icon = "iocn_yi";
        skill.Type = "Base";
        skill.Pos = "basic";
        skill.SkillID = 111;
        skills.Add(skill);

        //this.GetChild("Skill-item").GetComponent<SkillBase>().ReviceSkillInfo(skill);
        InitState(skills);
	}


    public void InitState(List<SkillItem> skills)
    {
        this.skills = skills;
        if (skillDict == null)
            skillDict = new Dictionary<int, SkillItem>();
        else
            skillDict.Clear();
        foreach(SkillItem skill in skills)
        {
            UITools.D("skill." + skill.Pos).GetComponent<SkillBase>().ReviceSkillInfo(skill);
            skillDict.Add(skill.SkillID, skill);
        }
    }
    public bool isAbleReleaseSkill()
    {
        return animMgr.IsClipPlay(runClipName) || animMgr.IsClipPlay(idleClipName);
    }

    public SkillItem GetSkillBySkillID(int skillID)
    {
        if (skillDict.ContainsKey(skillID))
            return skillDict[skillID];
        else
            return null;
    }
    public void ReleaseSkill(int skillID , DefAction OnSuccess=null , DefAction OnFail = null)
    {
        GameTools.Log("ReleaseSkill");
        SkillItem skill = GetSkillBySkillID(skillID);
        if (skill == null || isAbleReleaseSkill())
        {
            if (OnFail != null) OnFail();
            return;
        }
        string skillClipName = "Skill_" + skill.Pos;
        animMgr.PlayClip(skillClipName);
    }
}
