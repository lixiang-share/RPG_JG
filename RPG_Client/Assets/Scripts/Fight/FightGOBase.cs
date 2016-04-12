using UnityEngine;
using System.Collections;

public class FightGOBase : MonoBehaviour {

    private int ID;


    public void WaitForSec(float sec , DefAction action)
    {
        StartCoroutine(_WaitForSec(sec, action));
    }
    private IEnumerator _WaitForSec(float sec , DefAction action)
    {
        yield return new WaitForSeconds(sec);
        if (action != null) action();
    }
    public void TweenPos(Vector3 targetPos, float duration ,bool worldSpace,  DefAction OnFinish = null)
    {
        TweenPosition tp = GetComponent<TweenPosition>();
        if (tp != null)
        {
            tp.worldSpace = worldSpace;
            tp.from = transform.position;
            tp.to = targetPos;
            tp.onFinished.Clear();
            tp.AddOnFinished(() => {
                if (OnFinish != null) OnFinish();
            });
            tp.PlayForward();
        }
        tp = TweenPosition.Begin(gameObject, duration, targetPos, worldSpace);
        tp.AddOnFinished(() =>
        {
            //if (gameObject.GetComponent<TweenPosition>() != null) UnityEngine.Object.Destroy(gameObject.GetComponent<TweenPosition>());
            if (OnFinish != null) OnFinish();
        });
        
    }

}
