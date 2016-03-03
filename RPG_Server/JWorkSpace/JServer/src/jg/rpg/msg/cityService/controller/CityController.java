package jg.rpg.msg.cityService.controller;

import java.io.IOException;

import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.msgEntity.Player;

public class CityController {

	public void packPlayer(MsgPacker packer, Player player) throws IOException {
		
		packer.addInt(player.getId())
			.addString(player.getUsername())
			.addString(player.getPhoneNum())
			.addInt(player.getLevel())
			.addInt(player.getFc())
			.addInt(player.getExp())
			.addInt(player.getDiamondCount())
			.addInt(player.getGoldCount())
			.addInt(player.getVit())
			.addInt(player.getToughen())
			.addInt(player.getHp())
			.addInt(player.getDamage());
	}

	
}
