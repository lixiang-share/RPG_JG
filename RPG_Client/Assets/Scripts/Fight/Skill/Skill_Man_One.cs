using UnityEngine;
using System.Collections;

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

    public override void Release(DefAction OnSuccess = null, DefAction OnFinish = null)
    {
        //播放角色动作
        base.Release(OnSuccess, OnFinish);
        //控制猪脚瞬间移动
        WaitForSec(delayTime , ()=>{
            FastMove();
        });

        //控制特效显示
        WaitForSec(FirstTime, () => {
            if (effectGOFirst == null) return;
            if (!effectGOFirst.activeSelf) effectGOFirst.SetActive(true);
            PlayEffect(effectGOFirst);
        });

        WaitForSec(SecondTime, () =>
        {
            if (effectGOSecond == null) return;
            if (!effectGOSecond.activeSelf) effectGOSecond.SetActive(true);
            PlayEffect(effectGOSecond);

        });

        WaitForSec(ThirdTime, () =>
        {
            if (effectGOThird == null) return;
            if (!effectGOThird.activeSelf) effectGOThird.SetActive(true);
            PlayEffect(effectGOThird);

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
        NcCurveAnimation[] ncs = effectGO.GetComponentsInChildren<NcCurveAnimation>();
        foreach (NcCurveAnimation nc in ncs)
        {
            if (!nc.enabled) nc.enabled = true;
            nc.ResetAnimation();
        }
    }

}
