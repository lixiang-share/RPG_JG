using UnityEngine;
using System.Collections;
/// <summary>
/// 定义消息类型
/// </summary>
public class MsgProtocol {
    //请求协议后，服务端返回状态
    public const int Error = -1;
    public const int Success = 0;

    //协议类型
    public const int Login = 1;
    public const int Get_ServerList = 2;
    public const int Register = 3;
    public const int Query_Status = 4;
    public const int PreSelectHero = 5;
	public const int EnterGame = 6;
	public const int Get_TaskList = 7;
    public const int Update_PlayerInfo = 8;
    public const int Get_EquipList = 9;
	public const int Dress_Equip = 10;
	public const int Sale_Equip = 11;
    public const int Upgrade_Equip = 12;
    public const int Undress_Equip = 13;
	public const int AcceptTask = 14;
	
	public const int Get_SkillList = 15;
	public const int Upgrade_Skill = 16;

    public const int Get_PlayerInfo = 17;
}
