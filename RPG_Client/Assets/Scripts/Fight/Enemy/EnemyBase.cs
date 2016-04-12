using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public delegate void DamageHandle(AttackItem attack, DefAction OnFinish);
public class EnemyBase : FightGOBase {
    public string bloodEffect = "BloodSplatEffect";
    public GameObject BloodEffectRoot;
    public string devilHandEffect = "DevilHandMobile";
    public GameObject DevilHandRoot;

    private Dictionary<AttackType, DamageHandle> handleDict;
    private bool isInHeight = false;
    public void Start()
    {
        if (handleDict == null) handleDict = new Dictionary<AttackType, DamageHandle>();
        handleDict.Add(AttackType.Normal01, GetAttack01Damage);
        handleDict.Add(AttackType.Normal02, GetAttack02Damage);
        handleDict.Add(AttackType.Normal03, GetAttack03Damage);
    }
    
    public virtual void GetDamage(AttackItem attack, DefAction OnFinish = null)
    {
        GameTools.Log("Enemy GetDamage ===> "+attack.Type);
        handleDict[attack.Type](attack , OnFinish);

    }

    protected virtual void GetAttack01Damage(AttackItem attack, DefAction OnFinish = null)
    {
        //Release Effect
        GameObject effect = Instantiate(ResourceManager.Instance.LoadFightEffect(bloodEffect)) as GameObject;
        effect.transform.parent = BloodEffectRoot.transform;
        effect.transform.localPosition = Vector3.zero;
        
        //Move Posotion
        if (!isInHeight)
        {
            //here may spawn bug due to enemy may not toward to player,but just do it
            isInHeight = true;
            Vector3 targetPosFinal = transform.position + -1 * transform.forward * attack.JumpHeight;
            Vector3 targetPos = targetPosFinal + new Vector3(0, attack.JumpHeight, 0);
            float duration = attack.JumpDuration / 2;
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



    protected virtual void GetAttack02Damage(AttackItem attack, DefAction OnFinish = null)
    {

    }

    protected virtual void GetAttack03Damage(AttackItem attack, DefAction OnFinish = null)
    {
        GameObject effect = Instantiate(ResourceManager.Instance.LoadFightEffect(devilHandEffect)) as GameObject;
        effect.transform.parent = DevilHandRoot.transform;
        effect.transform.localPosition = Vector3.zero;
    }
}
