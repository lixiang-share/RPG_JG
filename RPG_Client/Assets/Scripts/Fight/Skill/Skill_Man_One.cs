using UnityEngine;
using System.Collections;

public enum BaseSkillState { Idle,Attack01,Attack02,Attack03 };

public class Skill_Man_One : SkillBase {

    public float moveDist = 2;
    public float durationTime = 1;
    public float delayTime = 0.5f;

    public float FirstTime = 0;
    public float SecondTime = 1;
    public float ThirdTime = 2;

    public GameObject effectGOFirst;
    public GameObject effectGOSecond;
    public GameObject effectGOThird;

    public string attack01Clip = "Skill_basic";
    public string attack02Clip = "Basic_attack02";
    public string attack03Clip = "Basic_attack03";

    public float DelayCalAttack01DamageTime = 0.5f;
    public float DelayCalAttack02DamageTime = 0.05f;
    public float DelayCalAttack03DamageTime = 0.1f;

    public BaseSkillState curState = BaseSkillState.Idle;
    public BaseSkillState targettState = BaseSkillState.Idle;

    public override bool HasComoSkill()
    {
        GameTools.LogError("HasComoSkill");
        switch (curState)
        {
            case BaseSkillState.Idle:
                return false;
            case BaseSkillState.Attack01:
                if (animMgr.IsClipPlay(attack01Clip))
                    return true;
                else
                    return false;
            case BaseSkillState.Attack02:
                if (animMgr.IsClipPlay(attack02Clip))
                    return true;
                else
                    return false;
            case BaseSkillState.Attack03:
                return false;
        }
        return false;
    }

    public override void ReleaseComboSkill()
    {
        GameTools.LogError("ReleaseComboSkill");
        switch (targettState)
        {
            case BaseSkillState.Idle:
                targettState = BaseSkillState.Idle;
                break;
            case BaseSkillState.Attack01:
                WaitForSec(SecondTime, () => { PlayEffect(effectGOSecond); });
                targettState = BaseSkillState.Attack02;
                break;
            case BaseSkillState.Attack02:
                WaitForSec(ThirdTime, () => { PlayEffect(effectGOThird); });
                targettState = BaseSkillState.Attack03;
                break;
            default:
                targettState = BaseSkillState.Attack03;
                break;
        }
    }


    public override void Release(DefAction OnSuccess = null, DefAction OnFinish = null)
    {
        targettState = curState = BaseSkillState.Attack01;
        //播放角色动作
        base.Release(OnSuccess, () => {
            OnReleaseFinish(OnFinish);
        });
        //控制猪脚瞬间移动
        WaitForSec(delayTime , ()=>{
            FastMove();
        });
        AudioManager.Instance.PlayAudio(AudioManager.Man_Comm_Attack01);
        //控制特效显示
        PlayEffect(effectGOFirst);
        //计算伤害值
        WaitForSec(DelayCalAttack01DamageTime, () =>
        {
            AttackItem attack = new AttackItem();
            attack.Type = AttackType.Normal01;
            PlayerFightCtrl.Instance.CalculateDamage(attack);
        });
    }

    public void OnReleaseFinish(DefAction OnFinish)
    {
        if (targettState == curState)
        {
            ResetState();
            OnFinish();
            return;
        }
        switch (curState)
        {
            case BaseSkillState.Idle:
                ResetState();
                break;
            case BaseSkillState.Attack01:
                if (targettState > curState)
                {
                    ReleaseAttack02(OnFinish);
                    curState = BaseSkillState.Attack02;
                }
                break;
            case BaseSkillState.Attack02:
                if (targettState > curState)
                {
                    ReleaseAttack03(OnFinish);
                    curState = BaseSkillState.Attack03;
                }
                break;
            default:
                ResetState();
                OnFinish();
                break;
        }  
    }

    private void ResetState()
    {
        curState = targettState = BaseSkillState.Idle;
    }

    private void ReleaseAttack02(DefAction OnFinish)
    {
        AudioManager.Instance.PlayAudio(AudioManager.Man_Comm_Attack02);
        GameTools.LogError("ReleaseAttack02222");
        animMgr.PlayClip(attack02Clip, () =>
        {
            OnReleaseFinish(OnFinish);
        });
        WaitForSec(DelayCalAttack02DamageTime, () =>
        {
            AttackItem attack = new AttackItem();
            attack.Type = AttackType.Normal02;
            PlayerFightCtrl.Instance.CalculateDamage(attack);
        });
    }
    private void ReleaseAttack03(DefAction OnFinish)
    {
        WaitForSec(0.5f, () => {
            AudioManager.Instance.PlayAudio(AudioManager.Man_Comm_Attack03);
        });
        GameTools.LogError("ReleaseAttack0333");
        animMgr.PlayClip(attack03Clip, () =>
        {
            OnReleaseFinish(OnFinish);
        });
        WaitForSec(DelayCalAttack03DamageTime, () =>
        {
            AttackItem attack = new AttackItem();
            attack.Type = AttackType.Normal03;
            PlayerFightCtrl.Instance.CalculateDamage(attack);
        });
    }

    public void FastMove()
    {
        GameObject playerGO = PlayerFightCtrl.Instance.gameObject;
        Vector3 detaDist = playerGO.transform.forward.normalized * moveDist;
        Vector3 targetPos = playerGO.transform.position + detaDist;
        if (Enclosure.Instance.isInside(targetPos))
        {
            TweenPosition.Begin(playerGO, durationTime, targetPos,true).AddOnFinished(
                () => {
                    if (playerGO.GetComponent<TweenPosition>() != null)
                    {
                        Object.Destroy(playerGO.GetComponent<TweenPosition>());
                    }
                }
            );
        }
    }

    public void PlayEffect(GameObject effectGO)
    {
        if (effectGO == null) return;
        if (!effectGO.activeSelf) effectGO.SetActive(true);
        NcCurveAnimation[] ncs = effectGO.GetComponentsInChildren<NcCurveAnimation>();
        foreach (NcCurveAnimation nc in ncs)
        {
            if (!nc.enabled) nc.enabled = true;
            nc.ResetAnimation();
        }
    }

}
