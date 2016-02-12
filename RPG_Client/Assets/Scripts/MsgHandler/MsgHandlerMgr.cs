using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MsgHandlerMgr
{
    private List<IMsgHandler> handlers;
    private int curHandlerIndex = 0;

    public MsgHandlerMgr()
    {
        Init();
    }
    private void Init()
    {
        handlers = new List<IMsgHandler>();
    }

    public MsgHandlerMgr RegisterHander(IMsgHandler handler)
    {
        if (handler != null)
            handlers.Add(handler);
        return this;
    }

    public void RemoveHandler(IMsgHandler handler)
    {
        if (handler != null)
            handlers.Remove(handler);
    }

    public void RemoveHandler(int index)
    {
        if (index < 0 || index >= handlers.Count) return;
        handlers.RemoveAt(index);

    }

    public void HandleMsg(MsgUnPacker unpacker)
    {
        UITools.log("start to Handler msg");
        curHandlerIndex = 0;
        if (curHandlerIndex < handlers.Count)
        {
            handlers[curHandlerIndex].HandleMsg(this , unpacker);
        }
    }

    public void NextHandler(MsgUnPacker unpacker)
    {
        UITools.log("Next Hnader ......");
        if (curHandlerIndex < handlers.Count -1 )
        {
            handlers[++curHandlerIndex].HandleMsg(this, unpacker);
        }
    }
}

