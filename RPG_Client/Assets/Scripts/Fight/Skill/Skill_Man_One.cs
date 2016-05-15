using UnityEngine;
using System.Collections;

public class Skill_Man_One : SkillBase {

    public float Effect01Time = 0f;
    public float Effect02Time = 1f;
    public float Effect03Time = 2f;

    public GameObject effect01;
    public GameObject effect02;
    public GameObject effect03;

    public float CalDamageTime = 1.5f;
    public override void Release(DefAction OnSuccess = null, DefAction OnFinish = null)
    {
        base.Release(OnSuccess, OnFinish);
        //播放声音
        AudioManager.Instance.PlayAudio(AudioManager.SKill_One);        
        
        //播放特效
        WaitForSec(Effect01Time, () => { PlayEffect(effect01);});
        WaitForSec(Effect02Time, () => { PlayEffect(effect02);});
        WaitForSec(Effect03Time, () => { PlayEffect(effect03);});

        //计算伤害
        WaitForSec(CalDamageTime,()=>{
        
            AttackItem attack = new AttackItem();
            attack.Type = AttackType.Skill01;
            attack.AttackDir = AttackDir.Around;
            attack.Range = 5;
            PlayerFightCtrl.Instance.CalculateDamage(attack);
        });
    }
}
