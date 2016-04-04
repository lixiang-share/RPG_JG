using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private static PlayerManager inst;
    private Player player;
    private RoleItem role;
    private ServerItem server;

    public ServerItem Server
    {
        get { return server; }
        set { server = value; }
    }
    public RoleItem Role
    {
        get { return role; }
        set { role = value; }
    }

    public static PlayerManager Inst
    {
        get { return PlayerManager.inst; }
    }
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    void Awake()
    {
        if (inst != null)
        {
            Destroy(inst);
            inst = null;
        }
        inst = this;
    }

    public void UpdatePlayerinfo(MsgUnPacker unpacker)
    {
        Player player = UITools.MsgToPlayer(unpacker);
        this.Player = player;
    }

    public void UpdatePlayerinfo()
    {
        MsgPacker msg = new MsgPacker();
        msg.SetType(MsgProtocol.Get_PlayerInfo);
        NetworkMgr.instance.Send(msg, (data) => {
            UpdatePlayerinfo(data);
            UITools.D("PlayerInfo").CallLuaMethod("SetInfo");
            UITools.D("PlayerDetails").CallLuaMethod("SetInfo");
        });
    }
}
