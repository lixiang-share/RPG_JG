using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SkillManager : LuaBehaviour {

    public bool isInit;
    public List<SkillItem> skills;

	void Start () {
        List<SkillItem> skills = new List<SkillItem>();
        SkillItem skill = new SkillItem();
        skill.ColdTime = 1;
        skill.Icon = "icon_li";
        this.GetChild("Skill-item").GetComponent<SkillBase>().ReviceSkillInfo(skill);
	}


    public void InitState(List<SkillItem> skills)
    {
        this.skills = skills;
        foreach(SkillItem skill in skills)
        {
            
        }
    }


	
    public void ReleaseSkill(int skillID , DefAction OnSuccess=null , DefAction OnFail = null)
    {

    }
}
