package jg.rpg.msg.player;

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
import jg.rpg.msg.city.player.PlayerController;
import jg.rpg.utils.MsgUtils;

import org.apache.log4j.Logger;

public class PlayerService {
	private static Logger logger = Logger.getLogger(PlayerService.class);
	private SessionMgr playerMgr;
	private PlayerController controller;
	
	public PlayerService() {
		playerMgr = SessionMgr.getInstance();
		controller = new PlayerController();
	}
	
	@HandlerMsg(msgType = MsgProtocol.Update_PlayerInfo)
	public void updatePlayerInfo(Session session , MsgUnPacker unpacker){
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
	
	@HandlerMsg(msgType = MsgProtocol.Get_PlayerInfo)
	public void getPlayerInfo(Session session , MsgUnPacker unpacker){
		try {
			MsgPacker packer = MsgUtils.getSuccessPacker();
			session.getPlayer().pack(packer);
			MsgUtils.sendMsg(session.getCtx(), packer);
		} catch (Exception e) {
			logger.warn(e.getMessage());
			MsgUtils.SendErroInfo(session.getCtx(), "更新玩家信息失败");
		}
	}
	
	

	


}
