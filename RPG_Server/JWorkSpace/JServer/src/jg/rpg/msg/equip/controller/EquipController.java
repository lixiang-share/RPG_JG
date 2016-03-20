package jg.rpg.msg.equip.controller;

import java.io.IOException;
import java.sql.SQLException;
import java.util.List;

import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.msgEntity.EquipItem;

public class EquipController {
	// TODO Auto-generated method stub


public void packEquipsToMsg(List<EquipItem> equips , MsgPacker packer) throws IOException {
	packer.addInt(equips.size());
	for(EquipItem item : equips){
		packer.addInt(item.getId())
			.addInt(item.getOwnerId())
			.addInt(item.getEquipId())
			.addInt(item.getLevel())
			.addInt(item.getAmount())
			.addBool(item.isDress())
			.addBool(item.isMan())
			.addString(item.getType())
			.addString(item.getEquipType())
			.addInt(item.getPrice())
			.addInt(item.getStar())
			.addInt(item.getQuality())
			.addString(item.getEffectType())
			.addInt(item.getEffectValue())
			.addInt(item.getHp())
			.addInt(item.getDamage())
			.addInt(item.getFc());
	}
	
}

}
