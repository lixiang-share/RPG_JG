using UnityEngine;
using System.Collections;

public class SkillBase : FightGOBase {
    public SkillItem skillInfo;
    public bool isInit = false;
    public bool isCD = false;
    public LuaBehaviour ctrlGO;
    public UISprite cdSprite;
    public PlayerAnimatorMgr animMgr;
    private float curCDTime;

    public void Update()
    {
        if (isCD)
        {
            if(curCDTime <= 0) { EndCD(); return; }
            curCDTime = curCDTime - Time.deltaTime;
            SetCDProgress(); 
        }
    }

    public void EndCD()
    {
        isCD = false;
        curCDTime = 0f;
        cdSprite.gameObject.SetActive(false);
    }


    public virtual void ReviceSkillInfo(SkillItem skill)
    {
        this.skillInfo = skill;
        InitState();
    }

    public void SetCDProgress()
    {
        if (skillInfo.Type == "Base" || skillInfo.ColdTime < 0.001f) return;
        float progress = curCDTime / skillInfo.ColdTime;
        progress = Mathf.Clamp01(progress);
        if(progress <=0.001F)
        {
            cdSprite.gameObject.SetActive(false);
        }
        else
        {
            if (!cdSprite.gameObject.activeSelf) cdSprite.gameObject.SetActive(true);
            cdSprite.fillAmount = progress;
        }
    }
    public void SetControlGO()
    {
        ctrlGO = UITools.D("skill." + this.skillInfo.Pos);
        cdSprite = ctrlGO.GetChild("s_fore").GetComponent<UISprite>();
        if(ctrlGO is ScaleButton){
            ScaleButton sb = ctrlGO as ScaleButton;
            sb.OnClickListen = OnClickSkill;
        }
    }
    public void InitState()
    {
        SetControlGO();
        animMgr = PlayerFightCtrl.Instance.AnimMgr;
        ctrlGO.GetChild("s_icon").Value = this.skillInfo.Icon;
        ctrlGO.GetChild("s_fore").Value = this.skillInfo.Icon;
        if (skillInfo.Type == "Base") cdSprite.gameObject.SetActive(false);
        curCDTime = 0;
        isCD = true;
        isInit = true;
    }
    public void EnterCD()
    {
        isCD = true;
        curCDTime = skillInfo.ColdTime;
        cdSprite.gameObject.SetActive(true);
    }

    public  void OnClickSkill()
    {
        if (!PlayerFightCtrl.Instance.isAbleFight()||!isInit) return;

        if(skillInfo.Type != SkillItem.Base){
            PlayerFightCtrl.Instance.Attack(skillInfo.SkillID, !isCD);
            if (!isCD && skillInfo.ColdTime != 0  ) 
                EnterCD();
        }
        else
        {
            PlayerFightCtrl.Instance.Attack(skillInfo.SkillID);
        }
    }

    public virtual void Release(DefAction OnSuccess = null, DefAction OnFinish = null)
    {
        if (!isInit) return;
        string skillClipName = "Skill_" + skillInfo.Pos;
        GameTools.Log("Start ReleaseSkill");
        animMgr.PlayClip(skillClipName, OnFinish);
        OnSuccess();
    }
}
