using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public delegate void OnTalkFinish();
public class TalkToNPC : MonoBehaviour {

	public static TalkToNPC Instance;
	public UILabel l_content;
	public UILabel l_btnDesc;
    private int curStage = 0;
    private List<string> contents;
    private OnTalkFinish onFinish;
    void Awake()
    {
	    Instance = this;
	    gameObject.SetActive(false);
    }
    public void Talk(List<string> contents , OnTalkFinish onFinish = null)
    {
        if (contents != null && contents.Count != 0)
        {
            this.contents = contents;
            this.onFinish = onFinish;
            curStage = 0;
            BegainTalk();
        }
    }
    private void BegainTalk()
    {
	    if(!gameObject.active) gameObject.SetActive(true);
	    curStage = -1;
	    Next();
    }

    public void Next()
    {
	    ++curStage ;
	    if(curStage >= contents.Count){
	    	Finish();
	    }else if(curStage == contents.Count-1){
	    	l_content.text = contents[curStage];
	    	l_btnDesc.text = "确定";;
	    }else{
	    	l_content.text = contents[curStage];
	    	l_btnDesc.text = "下一步"; 
	    }
    }

    private void Finish()
    {
        curStage = -1;
        contents = null;
        gameObject.SetActive(false);
        if(onFinish != null) onFinish();
    }
}

