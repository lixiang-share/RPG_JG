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
        base.OnDisable();
        gameObject.SetActive(false);
        wrapContent.SortBasedOnScrollMovement();
        scrollView.MoveRelative(defScrollViewPos - transform.localPosition);
    }
    public override void Parse(IList list)
    {
        listData = list;
        InitIndex();
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
       // UITools.log(go.name + " : " + realIndex + " : " + wrapIndex);
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
