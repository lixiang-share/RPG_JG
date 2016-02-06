using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IMsgHandler
{
   void HandleMsg(MsgHandlerMgr ctx , MsgUnPacker unpacker);
}