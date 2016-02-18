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
}
