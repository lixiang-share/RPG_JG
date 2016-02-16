using UnityEngine;
using System.Collections;

public class ShowText : LuaWithNoFile
{
    public float showTime = 2f;
    public UILabel label;
    private bool isShowing;

    public override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }

    public void SetText(string mes)
    {
        if (isShowing)
            return;
        isShowing = true;
        label.text = mes;
        UITools.ShowPanel(this.GetComponent<LuaWithNoFile>());
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(showTime);
        isShowing = false;
        UITools.ClosePanel(this.GetComponent<LuaWithNoFile>());
    }

}
