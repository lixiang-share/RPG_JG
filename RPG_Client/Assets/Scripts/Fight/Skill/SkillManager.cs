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
    private Dictionary<int, SkillBase> skillCtrlDict;
	void Start () {
        skillDict = new Dictionary<int, SkillItem>();
        skillCtrlDict = new Dictionary<int, SkillBase>();
        if (animMgr == null) animMgr = PlayerFightCtrl.Instance.AnimMgr;

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
        foreach(SkillItem skill in skills)
        {
            SkillBase skillCtrl = transform.GetChild("skill_" + skill.Pos).GetComponent<SkillBase>();
            skillCtrl.ReviceSkillInfo(skill);

            skillDict.Add(skill.SkillID, skill);
            skillCtrlDict.Add(skill.SkillID, skillCtrl);
        }
    }
    public bool isAbleReleaseSkill()
    {
        return  animMgr.IsClipPlay(idleClipName);
    }

    public SkillItem GetSkillItemBySkillID(int skillID)
    {
        if (skillDict.ContainsKey(skillID))
            return skillDict[skillID];
        else
            return null;
    }
    public SkillBase GetSkillCtrlBySkillID(int skillID)
    {
        if (skillCtrlDict.ContainsKey(skillID))
            return skillCtrlDict[skillID];
        else
            return null;
    }

    public void ReleaseSkill(int skillID, DefAction OnSuccess = null, DefAction OnFinish = null, DefAction OnFail = null)
    {
        GameTools.Log("ReleaseSkill");
        SkillItem skill = GetSkillItemBySkillID(skillID);
        if (skill == null || !isAbleReleaseSkill())
        {
            if (OnFail != null) OnFail();
            return;
        }
        SkillBase skillCtrl = GetSkillCtrlBySkillID(skillID);
        if (skillCtrl != null) skillCtrl.Release(OnSuccess , OnFinish);


  
    }
}
