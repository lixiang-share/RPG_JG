using UnityEngine;
using System.Collections;

public class Skill_Man_Two : SkillBase {

    public float effect01Time = 0.1f;
    public float effect02Time = 0.5f;
    public float effect03Time = 1f;
    public float effect04Time = 1.5f;

    public GameObject effect01;
    public GameObject effect02;
    public GameObject effect03;
    public GameObject effect04;

    public float attackTime = 1.5f;

    public override void Release(DefAction OnSuccess = null, DefAction OnFinish = null)
    {
        base.Release(OnSuccess, OnFinish);
        AudioManager.Instance.PlayAudio(AudioManager.Skill_Two);

        WaitForSec(effect01Time, () => { LoadEffect(effect01); });
        WaitForSec(effect02Time, () => { LoadEffect(effect02); });
        WaitForSec(effect03Time, () => { LoadEffect(effect03); });
        WaitForSec(effect04Time, () => { LoadEffect(effect04); });

        WaitForSec(attackTime, () => {

            AttackItem attack = new AttackItem();
            attack.Type = AttackType.Skill02;
            attack.AttackDir = AttackDir.Around;
            attack.Range = 5;
            PlayerFightCtrl.Instance.CalculateDamage(attack);
        });

    }
    public void LoadEffect(GameObject go)
    {
        if (go == null) return;
        go.SetActive(false);
        go.SetActive(true);

        WaitForSec(0.1f, () => { go.GetComponentInChildren<Animation>().Play(); });
    }
}
