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
        //TweenPosition tp = GetComponent<TweenPosition>();
        //if (tp != null)
        //{
        //    tp.worldSpace = worldSpace;
        //    tp.from = transform.position;
        //    tp.to = targetPos;
        //    tp.SetOnFinished(() =>
        //    {
        //        if (OnFinish != null) OnFinish();
        //        tp.onFinished.Clear();
        //    });
        //    tp.PlayForward();
        //}
        //else
        //{
        //    tp = TweenPosition.Begin(gameObject, duration, targetPos, worldSpace);
        //    tp.SetOnFinished(() =>
        //    {
        //        //if (gameObject.GetComponent<TweenPosition>() != null) UnityEngine.Object.Destroy(gameObject.GetComponent<TweenPosition>());
        //        if (OnFinish != null) OnFinish();
        //        tp.onFinished.Clear();
        //    });
        //}
        if (Enclosure.Instance.isInside(targetPos))
        {
               TweenPosition tweenPos = TweenPosition.Begin(this.gameObject, duration, targetPos, true);
               tweenPos.AddOnFinished(
                () =>
                {
                    if (OnFinish != null) OnFinish();
                    if (tweenPos != null)
                    {
                        Object.Destroy(tweenPos);
                    }
                }
            );
        }
    }
    public void PlaySound(string sound)
    {
        AudioManager.Instance.PlayAudio(sound);
    }
    public void ReleaseREffect(string effectName, GameObject parent)
    {
        GameObject effect = Instantiate(ResourceManager.Instance.LoadFightEffect(effectName)) as GameObject;
        effect.transform.parent = parent.transform;
        effect.transform.localPosition = Vector3.zero;
    }
}
