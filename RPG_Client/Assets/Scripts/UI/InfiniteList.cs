using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfiniteList : LuaBehaviour {

    public UIWrapContent wrapContent;
    public UIScrollView scrollView;
    public IList listData;
    public IList groupData;
    public int row = 1;
    private IList<Transform> mChildren;
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
        if (list.Count <= 0)
        {
            UITools.SA(this, false);
        }
        else
        {
            if (row <= 1)
            {
                wrapContent.minIndex  = (listData.Count - 1) * -1;
                wrapContent.maxIndex  = 0;
            }
            else
            {
                wrapContent.maxIndex = 0;
                int _minIndex = listData.Count % row == 0 ? listData.Count / row - 1 : listData.Count / row;
                wrapContent.minIndex = _minIndex * -1;

                groupData = new ArrayList();
                for (int i = 0; i < listData.Count / row; i++)
                {
                    ArrayList _list = new ArrayList();
                    for (int j = 0; j < row; j++)
                    {
                        _list.Add(listData[i * row + j]);
                    }
                    groupData.Add(_list);
                }
                ArrayList l = new ArrayList();
                for (int i = listData.Count - listData.Count%row; i < listData.Count; i++)
                {
                    l.Add(listData[i]);
                }
                if (l.Count != 0) groupData.Add(l);


            }
            UITools.SA(this, true);
            wrapContent.mFirstTime = true;
            wrapContent.WrapContent();
            wrapContent.mFirstTime = false;
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
        else if (row > 1 && index < groupData.Count)
        {
            go.SetActive(true);
            ArrayList _list = groupData[index] as ArrayList;
            if (_list.Count >= go.transform.childCount)
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    GameObject child = go.transform.GetChild(i).gameObject;
                    child.SetActive(true);
                    UITools.Get<LuaBehaviour>(child).ReceiveData(_list[i]);
                }
            }
            else
            {
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    GameObject child = go.transform.GetChild(i).gameObject;
                    child.SetActive(false);
                }
                for (int j = 0; j < _list.Count; j++)
                {
                    for (int i = 0; i < go.transform.childCount; i++)
                    {
                        GameObject child = go.transform.GetChild(i).gameObject;
                        if (child.name.Contains("" + j))
                        {
                            child.SetActive(true);
                            UITools.Get<LuaBehaviour>(child).ReceiveData(_list[j]);
                        }
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
