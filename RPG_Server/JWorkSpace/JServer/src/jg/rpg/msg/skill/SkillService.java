package jg.rpg.msg.skill;

import java.io.IOException;
import java.sql.SQLException;
import java.util.List;

import jg.rpg.common.anotation.HandlerMsg;
import jg.rpg.common.exceptions.EntityHandlerException;
import jg.rpg.common.exceptions.SkillHandleException;
import jg.rpg.common.protocol.MsgProtocol;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.entity.msgEntity.Skill;
import jg.rpg.msg.player.PlayerService;
import jg.rpg.msg.skill.controller.SkillController;
import jg.rpg.utils.MsgUtils;

import org.apache.log4j.Logger;

public class SkillService {
	private static Logger logger = Logger.getLogger(PlayerService.class);
	private SkillController controller;
	public SkillService(){
		controller = new SkillController();
	}
	
	@HandlerMsg(msgType = MsgProtocol.Get_SkillList)
	public void getSkillList(Session session , MsgUnPacker unpacker){
		Player player = session.getPlayer();
		if(player == null){
			MsgUtils.SendErroInfo(session.getCtx(), "请重新登录");
			return;
		}
		List<Skill> skills = player.getSkillsByCurRole();
		try {
			MsgPacker packer = MsgUtils.getSuccessPacker();
			if(skills == null || skills.isEmpty()){
				packer.addInt(0);
			}else{
				packer.addInt(skills.size());
				for(Skill skill : skills){
					logger.debug(skill);
					skill.packToMsg(packer);
				}
			}
			MsgUtils.sendMsg(session.getCtx(), packer);
		} catch (IOException e) {
			MsgUtils.SendErroInfo(session.getCtx(), "服务器错误");
		}
	}
	
	
	
	
	@HandlerMsg(msgType = MsgProtocol.Upgrade_Skill)
	public void upgradeSkill(Session session , MsgUnPacker unpacker){
		Player player = session.getPlayer();
		try {
			int skillID = unpacker.popInt();
			controller.upgradeSkill(player, skillID);
			this.getSkillList(session, unpacker);
		} catch  (IOException e) {
			MsgUtils.SendErroInfo(session.getCtx(), "技能升级失败");
		} catch (SkillHandleException e) {
			MsgUtils.SendErroInfo(session.getCtx(),e.getMessage());
		} catch (Exception e) {
			MsgUtils.SendErroInfo(session.getCtx(), "服务器错误");
		}
	
	}
}
