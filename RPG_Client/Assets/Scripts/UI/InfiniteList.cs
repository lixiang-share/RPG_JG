using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfiniteList : LuaBehaviour {

    public UIWrapContent wrapContent;
    public UIScrollView scrollView;
    public IList listData;
    public int row = 1;
    private IList<Transform> mChildren;
    private int minIndex;
    private int maxIndex;
    private Vector3 defScrollViewPos;
    private bool isInitListPos;


    public override void Awake()
    {
        base.Awake();
        if (!isInitListPos)
        {
            defScrollViewPos = scrollView.transform.localPosition;
            isInitListPos = true;
        }
        wrapContent.onInitializeItem += OnInitializeItem;
        mChildren = wrapContent.mChildren;
        gameObject.SetActive(false);
    }
    public override void OnDisable()
    {
        UITools.log("Disable...");
        base.OnDisable();
        gameObject.SetActive(false);
        scrollView.transform.localPosition = defScrollViewPos;
        foreach (Transform t in mChildren)
            t.gameObject.SetActive(false);
    }
    public override void Parse(IList list)
    {
        UITools.log("Data Count : " + list.Count);
        listData = list;
        InitIndex();
     //   InitItem();
       
    }

    private void InitItem()
    {
        if (row <= 1)
        {
            for (int i = 0; i < mChildren.Count; i++)
            {
                if (i  < listData.Count)
                {
                    mChildren[i].gameObject.SetActive(true);
                    UITools.Get<LuaBehaviour>(mChildren[i].gameObject).ReceiveData(listData[i]);
                }
                else
                {
                    mChildren[i].gameObject.SetActive(false);

                }
            }
        }
        else
        {
            int totalCount = listData.Count;
            for (int i = 0; i < mChildren.Count; i++)
            {
                GameObject pGO = mChildren[i].gameObject;
                if (i < listData.Count / row)
                {
                    totalCount -= row;
                    pGO.SetActive(true);
                    for (int j = 0; j < pGO.transform.childCount; j++)
                    {
                        pGO.transform.GetChild(j).gameObject.SetActive(true);
                        UITools.Get<LuaBehaviour>(pGO.transform.GetChild(j).gameObject).ReceiveData(listData[i*2+j]);
                    }
                }
                else
                {
                    if (totalCount > 0)
                    {      
                        pGO.SetActive(true);
                        for (int k = 0; k < pGO.transform.childCount; k++)
                        {
                            pGO.transform.GetChild(k).gameObject.SetActive(false);
                        }
                        for (int j = 0; j < totalCount; j++)
                        {
                            for (int k = 0; k < pGO.transform.childCount; k++)
                            {
                                if (pGO.transform.GetChild(j).name.Contains("" + j))
                                {     
                                    pGO.transform.GetChild(j).gameObject.SetActive(true);
                                    UITools.Get<LuaBehaviour>(pGO.transform.GetChild(j).gameObject).ReceiveData(listData[i * 2 + j]);
                                }
                            }
                        }
                        totalCount = 0;
                    }
                    else
                    {
                        pGO.SetActive(false);
                    }
                }
            }
        }
    }

    private void InitIndex()
    {

        if (listData.Count <= 0)
        {
            UITools.SA(this, false);
        }
        else
        {
            if(row <= 1){
                wrapContent.minIndex = minIndex = (listData.Count - 1) * -1;
                wrapContent.maxIndex = maxIndex = 0;
                for (int i = 0; i < listData.Count && i<mChildren.Count; i++)
                {
                    mChildren[i].gameObject.SetActive(true);
                }


            }else{
                minIndex = (listData.Count / 2)-1;
                if (listData.Count % 2 > 0) minIndex++;
                wrapContent.minIndex = minIndex * -1;
                wrapContent.maxIndex = maxIndex = 0;
            }
            UITools.SA(this, true);
        }


    }


    public void OnInitializeItem(GameObject go, int wrapIndex, int realIndex)
    {
        UITools.log(go.name + " : " + realIndex + " : " + wrapIndex);
        if (listData == null || listData.Count == 0)
        {
            UITools.SA(UITools.Get<LuaBehaviour>(go), false);
            return;
        }
        int index = realIndex * -1;
        int totalCount = listData.Count;

        if (row <= 1)
        {
            if (index < totalCount)
            {
                go.SetActive(true);
                UITools.Get<LuaBehaviour>(go).ReceiveData(listData[index]);
            }
            else
            {
                go.SetActive(false);
            }
        }
        else if (row > 1 && index < totalCount/row)
        {
            go.SetActive(true);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                child.SetActive(true);
                UITools.Get<LuaBehaviour>(child).ReceiveData(listData[index * row + i]);
            }
        }
        else if (row > 1 && totalCount % row != 0 && index == totalCount / row)
        {
            go.SetActive(true);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                child.SetActive(false);
            }
            for (int j = 0; j < totalCount%row; j++)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    GameObject child = go.transform.GetChild(i).gameObject;
                    if (child.name.Contains("" + j))
                    {
                        child.SetActive(true);
                        UITools.Get<LuaBehaviour>(child).ReceiveData(listData[index * row + j]);
                    }
                }
            }

        }
        else
        {
            go.SetActive(false);
        }
    }
	
}
