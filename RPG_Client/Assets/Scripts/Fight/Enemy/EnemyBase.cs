using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public delegate void DamageHandle(AttackItem attack, DefAction OnFinish);
public class EnemyBase : FightGOBase {
    public string bloodEffect = "BloodSplatEffect";
    public GameObject BloodEffectPos;
    public string devilHandEffect = "DevilHandMobile";
    public GameObject DevilHandPos;
    public string holyFireEffect = "HolyFireStrike";
    public GameObject holyFirePos;

    private Dictionary<AttackType, DamageHandle> handleDict;
    private bool isInHeight = false;
    public void Start()
    {
        if (handleDict == null) handleDict = new Dictionary<AttackType, DamageHandle>();

        handleDict.Add(AttackType.Normal, GetAttackDamage);
        handleDict.Add(AttackType.Skill01,GetSkillDamage01);
        handleDict.Add(AttackType.Skill02, GetSkillDamage02);
        handleDict.Add(AttackType.Skill03, GetSkillDamage03);

    }
    
    public virtual void GetDamage(AttackItem attack, DefAction OnFinish = null)
    {
        GameTools.LogError(attack.Type + "====> "+attack);
        handleDict[attack.Type](attack , OnFinish);
    }

    public virtual void GetSkillDamage01(AttackItem attack, DefAction OnFinish = null)
    {
        ShowBloodAndSound();
    }

    public virtual void GetSkillDamage02(AttackItem attack, DefAction OnFinish = null)
    {
        //Damage Effect
        ReleaseREffect(holyFireEffect, holyFirePos);
    }

    public virtual void GetSkillDamage03(AttackItem attack, DefAction OnFinish = null)
    {
        ShowBloodAndSound();
        AttackMoveBack(attack.JumpHeight, attack.JumpHeight);
    }


    public virtual void GetAttackDamage(AttackItem attack, DefAction OnFinish = null)
    {
        ShowBloodAndSound();
        switch (attack.Stage)
        {
            case AttackStage.First:
                GetAttack01Damage(attack, OnFinish);
                break;
            case AttackStage.Second:
                GetAttack02Damage(attack, OnFinish);
                break;
            case AttackStage.Third:
                GetAttack03Damage(attack, OnFinish);
                break;
        }
    }

    protected virtual void GetAttack01Damage(AttackItem attack, DefAction OnFinish = null)
    {
        //Move Posotion
        AttackMoveBack(attack.JumpHeight, attack.JumpHeight);
    }

    protected virtual void GetAttack02Damage(AttackItem attack, DefAction OnFinish = null)
    {

    }

    protected virtual void GetAttack03Damage(AttackItem attack, DefAction OnFinish = null)
    {
      
    }




    public void ShowBloodAndSound()
    {
        PlaySound(AudioManager.Hurt);
        ReleaseREffect(bloodEffect, BloodEffectPos);
    }

    
    public void ReleaseREffect(string effectName,GameObject parent)
    {
        GameObject effect = Instantiate(ResourceManager.Instance.LoadFightEffect(effectName)) as GameObject;
        effect.transform.parent = parent.transform;
        effect.transform.localPosition = Vector3.zero;
    }

    public void AttackMoveBack(float dist, float height, float duartion = 0.5f)
    {
        if (isInHeight) return;
        isInHeight = true;
        Vector3 targetPosFinal = transform.position + -1 * transform.forward * dist;
        Vector3 targetPos = targetPosFinal + new Vector3(0, height, 0);
        float duration = duartion / 2;
        if (Enclosure.Instance.isInside(targetPosFinal))
        {
            TweenPos(targetPos, duration, true, () =>
            {
                TweenPos(targetPosFinal, duration, true, () =>
                {
                    isInHeight = false;
                });

            });
        }
    }
}
