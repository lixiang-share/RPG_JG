using UnityEngine;
using System.Collections;

public class SkillBase : ScaleButton {
    public SkillItem skillInfo;
    public bool isInit = false;
    public bool isCD = false;
    public UISprite cdSprite;
    private float curCDTime;

    public override void Start()
    {
        base.Start();
        if (cdSprite == null) cdSprite = this.GetChild("s_fore").GetComponent<UISprite>();
    }

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

    public void InitState()
    {
        this.GetChild("s_icon").Value = this.skillInfo.Icon;
        this.GetChild("s_fore").Value = this.skillInfo.Icon;
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

    public override void OnClick()
    {
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
}
