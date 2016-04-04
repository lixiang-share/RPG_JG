package jg.rpg.msg.skill.controller;

import java.sql.SQLException;

import jg.rpg.common.exceptions.EntityHandlerException;
import jg.rpg.common.exceptions.SkillHandleException;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.entity.msgEntity.Skill;

public class SkillController {

	public void upgradeSkill(Player player ,int skillID) throws SkillHandleException, SQLException, EntityHandlerException{
		Skill skill = player.getSkillBySkillID(skillID);
		if(skill.getLevel() >= player.getRole().getLevel()){
			throw new SkillHandleException("技能等级不得超过玩家等级");
		}
		int needGold = (skill.getLevel()+1)*Skill.UpgradeUniteVal;
		if(needGold > player.getGoldCount()){
			throw new SkillHandleException("金币不足");
		}
		//Upgrade
		player.setGoldCount(player.getGoldCount() - needGold);
		skill.setLevel(skill.getLevel() + 1);
		skill.updateToDB();
	}
}
