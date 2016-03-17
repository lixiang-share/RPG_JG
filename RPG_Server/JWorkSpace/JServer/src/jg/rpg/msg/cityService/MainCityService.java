package jg.rpg.msg.cityService;

import java.io.IOException;
import java.util.List;

import jg.rpg.common.anotation.HandlerMsg;
import jg.rpg.common.exceptions.PlayerHandlerException;
import jg.rpg.common.manager.SessionMgr;
import jg.rpg.common.protocol.MsgProtocol;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.entity.msgEntity.EquipItem;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.entity.msgEntity.Task;
import jg.rpg.msg.cityService.controller.CityController;
import jg.rpg.utils.MsgUtils;

import org.apache.log4j.Logger;

public class MainCityService {
	private static Logger logger = Logger.getLogger(MainCityService.class);
	private SessionMgr playerMgr;
	private CityController controller;
	
	public MainCityService() {
		playerMgr = SessionMgr.getInstance();
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
	
	@HandlerMsg(msgType = MsgProtocol.Get_EquipList)
	public void getEquipList(Session session , MsgUnPacker unpacker){
		Player player = session.getPlayer();
		try {
			List<EquipItem> equips = controller.getEquipsByOwnweID(player.getId());
			MsgPacker packer = MsgUtils.getSuccessPacker();
			controller.packEquipsToMsg(equips , packer);
			MsgUtils.sendMsg(session.getCtx(), packer);
		} catch (Exception e) {
			logger.warn(e.getMessage());
			MsgUtils.SendErroInfo(session.getCtx(), "获取装备信息失败");
		}
	}




}
