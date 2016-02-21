using UnityEngine;
using System.Collections;

public static class ConvertUitls {

    public static IList MsgToServerList(MsgUnPacker unpacker)
    {
        IList list = new ArrayList();
        int msgLen = unpacker.PopInt();
        for (int i = 0; i < msgLen; i++)
        {
            ServerItem item = new ServerItem();
            item.Id = unpacker.PopInt();
            item.Name = unpacker.PopString();
            item.Ip = unpacker.PopString();
            item.Count = unpacker.PopInt();
            list.Add(item);
        }
        return list;
    }

    public static IList MsgToRoleList(MsgUnPacker unpacker)
    {
        IList list = new ArrayList();
        int roleLen = unpacker.PopInt();
        for (int i = 0; i < roleLen; i++)
        {
            RoleItem role  = new RoleItem();
            role.Id = unpacker.PopInt();
            role.OwnerId = unpacker.PopInt();
            role.Role_id = unpacker.PopString();
            role.Name = unpacker.PopString();
            role.Level = unpacker.PopInt();
            role.Gender = unpacker.PopInt();
            list.Add(role);
        }
        return list;
    }

    public static IList MsgToTaskList(MsgUnPacker unpacker)
    {
        IList list = new ArrayList();
        int taskLen = unpacker.PopInt();
        for (int i = 0; i < taskLen; i++)
        {
            TaskEntity task = new TaskEntity();
            task.Id = unpacker.PopInt();
            task.TaskId = unpacker.PopInt();
            task.RoleId = unpacker.PopInt();
            task.Type = unpacker.PopString();
            task.Status = unpacker.PopInt();
            task.GoldCount = unpacker.PopInt();
            task.DiamondCount = unpacker.PopInt();
            task.CurStage = unpacker.PopInt();
            task.TotalStage = unpacker.PopInt();
            list.Add(TaskMgr.Instance.ComposeTask(task));
        }
        return list;
    }
}
