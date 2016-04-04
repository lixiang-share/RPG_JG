package jg.rpg.common.protocol;
/**
 * 协议描述类，仅仅起标识作用
 */
public class MsgProtocol {
	public final static int  Error = -1;
	public final static int  Success = 0;
	
	
	public final static int  Login = 1;
	public final static int Get_ServerList = 2;
	public final static int Register = 3;
	public final static int Query_Status = 4;
	public final static int PreSelectHero = 5;
	public final static int EnterGame = 6;
	public final static int Get_TaskList = 7;
	public final static int Update_PlayerInfo = 8;
	
	public final static int Get_EquipList = 9;
	public final static int Dress_Equip = 10;
	public final static int Sale_Equip = 11;
	public final static int Upgrade_Equip = 12;
	public final static int Undress_Equip = 13;
	public final static int AcceptTask = 14;
	
	public final static int Get_SkillList = 15;
	public final static int Upgrade_Skill = 16;
	
	public final static int Get_PlayerInfo = 17;
	
}
