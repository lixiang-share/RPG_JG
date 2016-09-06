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
    public bool isInHeight = false;
    public Animation animation;
    public float dieDelay = 2f;
    private int hp = 10;

    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if (value < 0)
            {
                hp = 0;
            }
            else
            {
                hp = value;
            }
        }
    }

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
       // GameTools.LogError(attack.Type + "====> "+attack);
        handleDict[attack.Type](attack , OnFinish);
        CalDamage(attack);
    }


    public void CalDamage(AttackItem attack)
    {
        this.Hp = this.Hp - (int)attack.Damage;
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


    public void AttackMoveBack(float dist, float height, float duartion = 0.5f)
    {
        if (isInHeight) return;
        Vector3 targetPosFinal = transform.position + -1 * transform.forward * dist;
        Vector3 targetPos = targetPosFinal + new Vector3(0, height, 0);
        float duration = duartion / 2;
        if (Enclosure.Instance.isInside(targetPosFinal))
        {
            isInHeight = true;
            TweenPos(targetPos, duration, true, () =>
            {
                WaitForSec(0.1f, () => {
                    TweenPos(targetPosFinal, duration, true, () =>
                    {
                        isInHeight = false;
                    });
                });
            });
        }
    }


    public void  Die(){
        animation.Play("die");
        Destroy(gameObject, dieDelay);
    }
}
