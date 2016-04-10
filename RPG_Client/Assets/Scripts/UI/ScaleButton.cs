using UnityEngine;
using System.Collections;

public class ScaleButton : LuaBehaviour
{
    //public Vector3 scaleFactor = new Vector3(0.9f, 0.9f, 0.9f);
    [HideInInspector]
    public float scaleFactor = 0.9f;
    private Vector3 originScale;
    [HideInInspector]
    public DefAction OnClickListen;

    public void OnEnable()
    {
        originScale = transform.localScale;
        base.OnEnable();
    }

    public override void OnHold(bool isHold)
    {
        if (isHold)
        {
            TweenScale scale = GetComponent<TweenScale>();
            if (scale != null)
            {
                Destroy(scale);
                scale = null;
            }
            gameObject.transform.localScale = originScale*scaleFactor;
            AudioManager.Instance.PlayAudio(AudioManager.AUDIO_BTN);
        }
        else
        {
            TweenScale.Begin(gameObject,0.2f,originScale);
        }
        base.OnHold(isHold);
    }

    public override void OnClick()
    {
        base.OnClick();
        if (OnClickListen != null) OnClickListen();
    }
}
