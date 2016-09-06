using UnityEngine;
using System.Collections;

public class Skill_Man_Three : SkillBase {
    public int Damage;
    public float delayTime01 = 0f;
    public float delayTime02 = 0.8f;
    public float delayTime03 = 1.6f;

    public GameObject effect01;
    public GameObject effect02;
    public GameObject effect03;

    public float damageTime01 = 0.5f;
    public float damageTime02 = 1.5f;
    public float damageTime03 = 2.5f;


    public override void Release(DefAction OnSuccess = null, DefAction OnFinish = null)
    {
        base.Release(OnSuccess, OnFinish);
        WaitForSec(delayTime01, () => { 
            LoadEffect(effect01);
            
        });
        WaitForSec(damageTime01 / 2, () =>
        {
            AttackItem attack = GetSkill03AttackItem();
            attack.Damage = Damage;
            attack.Stage = AttackStage.First;
            CalDamage(attack);
        });
        WaitForSec(delayTime02, () => { 
            LoadEffect(effect02);
          
        });
        WaitForSec(damageTime02 / 2, () =>
        {
            AttackItem attack = GetSkill03AttackItem();
            attack.Damage = Damage*2;

            attack.Stage = AttackStage.Second;
            CalDamage(attack);
        });
        WaitForSec(delayTime03, () => {
            LoadEffect(effect03);
        });
        WaitForSec(damageTime03 / 2, () =>
        {
            AttackItem attack = GetSkill03AttackItem();
            attack.Damage = Damage * 3;
            attack.Stage = AttackStage.Third;
            CalDamage(attack);
        });
    }

    private AttackItem GetSkill03AttackItem()
    {
        AttackItem item = new AttackItem();
        item.Range = 10;
        item.AttackDir = AttackDir.Forward;
        item.Type = AttackType.Skill03;
        return item;
    }

    public void LoadEffect(GameObject go)
    {
        PlayEffect(go);
    }
}
