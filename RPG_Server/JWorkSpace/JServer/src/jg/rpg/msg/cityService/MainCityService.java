package jg.rpg.msg.cityService;

import java.io.IOException;
import java.util.List;

import jg.rpg.common.anotation.HandlerMsg;
import jg.rpg.common.manager.PlayerMgr;
import jg.rpg.common.protocol.MsgProtocol;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.entity.msgEntity.Role;
import jg.rpg.entity.msgEntity.Task;
import jg.rpg.utils.MsgUtils;

public class MainCityService {
	private PlayerMgr playerMgr;
	
	public MainCityService() {
		playerMgr = PlayerMgr.getInstance();
	}
	@HandlerMsg(msgType = MsgProtocol.Get_TaskList)
	public void getTaskList(Session session , MsgUnPacker unpacker){
		Role role = session.getPlayer().getRole();
		if(role == null){
			MsgUtils.SendErroInfo(session.getCtx(), "未选择角色，请重新登录");
			return;
		}
		List<Task> tasks = role.getTasks();
		try {
			MsgPacker packer = MsgUtils.getSuccessPacker();
			if(tasks == null || tasks.isEmpty()){
				packer.addInt(0);
			}else{
				packer.addInt(tasks.size());
				for(Task t : tasks){
					packer.addInt(t.getId())
						.addInt(t.getRoleId())
						.addString(t.getType())
						.addInt(t.getStatus())
						.addInt(t.getGoldCount())
						.addInt(t.getDiamondCount());
				}
			}
			MsgUtils.sendMsg(session.getCtx(), packer);
		} catch (IOException e) {
			MsgUtils.SendErroInfo(session.getCtx(), "服务器错误");
		}
		
	}
}
