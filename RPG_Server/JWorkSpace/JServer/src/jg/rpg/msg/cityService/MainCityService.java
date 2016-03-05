package jg.rpg.msg.cityService;

import java.io.IOException;
import java.util.List;

import org.apache.log4j.Logger;

import jg.rpg.common.anotation.HandlerMsg;
import jg.rpg.common.manager.PlayerMgr;
import jg.rpg.common.protocol.MsgProtocol;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.entity.msgEntity.Role;
import jg.rpg.entity.msgEntity.Task;
import jg.rpg.exceptions.PlayerHandlerException;
import jg.rpg.msg.cityService.controller.CityController;
import jg.rpg.utils.MsgUtils;

public class MainCityService {
	private static Logger logger = Logger.getLogger(MainCityService.class);
	private PlayerMgr playerMgr;
	private CityController controller;
	
	public MainCityService() {
		playerMgr = PlayerMgr.getInstance();
		controller = new CityController();
	}
	
	
	@HandlerMsg(msgType = MsgProtocol.Get_TaskList)
	public void getTaskList(Session session , MsgUnPacker unpacker){
		Player player = session.getPlayer();
		if(player == null){
			MsgUtils.SendErroInfo(session.getCtx(), "请重新登录");
			return;
		}
		List<Task> tasks = player.getTasks();
		try {
			MsgPacker packer = MsgUtils.getSuccessPacker();
			if(tasks == null || tasks.isEmpty()){
				packer.addInt(0);
			}else{
				packer.addInt(tasks.size());
				for(Task t : tasks){
					packer.addInt(t.getId())
						.addInt(t.getTaskId())
						.addInt(t.getOwnerId())
						.addString(t.getType())
						.addInt(t.getStatus())
						.addInt(t.getGoldCount())
						.addInt(t.getDiamondCount())
						.addInt(t.getCurStage())
						.addInt(t.getTotalStage());
				}
			}
			MsgUtils.sendMsg(session.getCtx(), packer);
		} catch (IOException e) {
			MsgUtils.SendErroInfo(session.getCtx(), "服务器错误");
		}
		
	}

	
	@HandlerMsg(msgType = MsgProtocol.Update_PlayerInfo)
	public void getPlayerInfo(Session session , MsgUnPacker unpacker){
		try {
			int row = controller.UpdatePlayerInfo(unpacker,session.getPlayer());
			if(row >= 1){
				MsgPacker packer = MsgUtils.getSuccessPacker();
				MsgUtils.sendMsg(session.getCtx(), packer);
			}else{
				throw new PlayerHandlerException("player not exits");
			}
		} catch (Exception e) {
			logger.warn(e.getMessage());
			MsgUtils.SendErroInfo(session.getCtx(), "更新玩家信息失败");
		}
	}




}
