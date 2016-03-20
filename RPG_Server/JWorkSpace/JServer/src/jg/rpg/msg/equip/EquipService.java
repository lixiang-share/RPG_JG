package jg.rpg.msg.equip;

import java.io.IOException;
import java.sql.SQLException;
import java.util.List;

import org.apache.log4j.Logger;

import jg.rpg.common.anotation.HandlerMsg;
import jg.rpg.common.exceptions.EntityHandlerException;
import jg.rpg.common.exceptions.EquipException;
import jg.rpg.common.protocol.MsgProtocol;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.entity.msgEntity.EquipItem;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.msg.equip.controller.EquipController;
import jg.rpg.utils.MsgUtils;

public class EquipService {
	private EquipController controller;
	private Logger logger = Logger.getLogger(getClass());
	public EquipService(){
		controller = new EquipController();
	}
	
	@HandlerMsg(msgType = MsgProtocol.Get_EquipList)
	public void getEquipList(Session session , MsgUnPacker unpacker){
		Player player = session.getPlayer();
		try {
			List<EquipItem> equips = player.getAllEquips();//controller.getEquipsByOwnerID(player.getId());
			MsgPacker packer = MsgUtils.getSuccessPacker();
			controller.packEquipsToMsg(equips , packer);
			MsgUtils.sendMsg(session.getCtx(), packer);
		} catch (Exception e) {
			logger.warn(e.getMessage());
			MsgUtils.SendErroInfo(session.getCtx(), "获取装备信息失败");
		}
	}
	
	
	@HandlerMsg(msgType = MsgProtocol.Dress_Equip)
	public void dressEquip(Session session , MsgUnPacker unpacker){
		 Player player = session.getPlayer();
		 try {
			int equipID = unpacker.popInt();
			player.dressEquip(equipID, true);
			MsgPacker packer = MsgUtils.getSuccessPacker();
			controller.packEquipsToMsg(player.getAllEquips() , packer);
			player.pack(packer);
			MsgUtils.sendMsg(session.getCtx(), packer);
			
		} catch (Exception e) {
			MsgUtils.SendErroInfo(session.getCtx(), e.getMessage());
			logger.debug(e.getMessage());
		}
	}
	
	
	@HandlerMsg(msgType = MsgProtocol.Undress_Equip)
	public void undressEquip(Session session , MsgUnPacker unpacker){
		 Player player = session.getPlayer();
		 try {
			int equipID = unpacker.popInt();
			player.undressEquip(equipID, true);
			MsgPacker packer = MsgUtils.getSuccessPacker();
			controller.packEquipsToMsg(player.getAllEquips() , packer);
			player.pack(packer);
			MsgUtils.sendMsg(session.getCtx(), packer);
			
		} catch (Exception e) {
			MsgUtils.SendErroInfo(session.getCtx(), e.getMessage());
			logger.debug(e.getMessage());
		}
	}
	
	@HandlerMsg(msgType = MsgProtocol.Upgrade_Equip)
	public void upgradeEquip(Session session , MsgUnPacker unpacker){
		 Player player = session.getPlayer();
		 try {
			int equipID = unpacker.popInt();
			player.upgradeEquip(equipID, true);
			MsgPacker packer = MsgUtils.getSuccessPacker();
			controller.packEquipsToMsg(player.getAllEquips() , packer);
			player.pack(packer);
			MsgUtils.sendMsg(session.getCtx(), packer);
			
		} catch (Exception e) {
			MsgUtils.SendErroInfo(session.getCtx(), e.getMessage());
			logger.debug(e.getMessage());
		}
	}
	
	@HandlerMsg(msgType = MsgProtocol.Sale_Equip)
	public void saleEquip(Session session , MsgUnPacker unpacker){
		 Player player = session.getPlayer();
		 try {
			int equipID = unpacker.popInt();
			player.saleEquip(equipID, true);
			MsgPacker packer = MsgUtils.getSuccessPacker();
			controller.packEquipsToMsg(player.getAllEquips() , packer);
			player.pack(packer);
			MsgUtils.sendMsg(session.getCtx(), packer);
			
		} catch (Exception e) {
			MsgUtils.SendErroInfo(session.getCtx(), e.getMessage());
			logger.debug(e.getMessage());
		}
	}
}
