using UnityEngine;
using System.Collections;

public class ShowText : MonoBehaviour {

    public float showTime = 1f;

    public void SetText(string mes)
    {
        UILabel label = gameObject.GetComponentInChildren<UILabel>();
        label.text = mes;
        TweenAlpha.Begin(gameObject, showTime, 0).AddOnFinished(() => 
        {
            Destroy(gameObject);
        });
    }

}
