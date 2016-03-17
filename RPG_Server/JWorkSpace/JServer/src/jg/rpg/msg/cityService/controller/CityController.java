package jg.rpg.msg.cityService.controller;

import java.io.IOException;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.msgEntity.EquipItem;
import jg.rpg.entity.msgEntity.Player;

public class CityController {
	public int UpdatePlayerInfo(MsgUnPacker unpacker, Player player) throws IOException, SQLException {
		List<Object> ps = new ArrayList<Object>();
		String sql = "update tb_user set ";
		int len = unpacker.popInt();
		for(int i=0 ; i<len ; i++){
			String key = unpacker.popString();
			String value = unpacker.popString();
			player.updateField(key , value);
			sql =  sql + key + " = ? ,";
			if(value.equals("name")){
				ps.add(value);
			}else{
				ps.add(Integer.parseInt(value));
			}
		}
		sql = sql.substring(0, sql.lastIndexOf(","));
		sql += " where id = ?";
		ps.add(player.getId());
		return	DBHelper.update(DBMgr.getInstance().getDataSource(), sql, ps.toArray());
	}

		// TODO Auto-generated method stub
	public List<EquipItem> getEquipsByOwnweID(int id) throws SQLException {
		String sql = "select * from tb_equips where ownerId = ?";
		List<EquipItem> equips = DBHelper.GetAll(DBMgr.getInstance().getDataSource(), 
				sql, RSHHelper.getEquipItemRSH(), id);
		return equips;
	}

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
