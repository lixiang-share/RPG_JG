using UnityEngine;
using System.Collections;
using System;

public class UITools{

    public static T Get<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
            t = go.AddComponent<T>();
        return t;
    }

    public void TweenPos(LuaBehaviour lb , float duration , Vector3 targetPos)
    {
        //TweenPosition tp = Get<TweenPosition>(lb.gameObject);
        TweenPosition tp = TweenPosition.Begin(lb.gameObject, duration, targetPos);
        tp.AddOnFinished(() =>
        {
            lb.OnCommand("EndTween");
        });
    }
}
