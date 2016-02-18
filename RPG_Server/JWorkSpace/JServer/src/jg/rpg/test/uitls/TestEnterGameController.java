package jg.rpg.test.uitls;

import java.sql.SQLException;

import jg.rpg.common.manager.PlayerMgr;
import jg.rpg.config.ConfigMgr;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.exceptions.InitException;
import jg.rpg.exceptions.PlayerHandlerException;
import jg.rpg.msg.MsgMgr;
import jg.rpg.msg.enterService.controller.EnterGameController;
import jg.rpg.utils.CommUtils;

import org.junit.Test;

public class TestEnterGameController {

	@Test
	public void registerPlayer() {
		try {
			ConfigMgr.getInstance().init();
			DBMgr.getInstance().init();
			PlayerMgr.getInstance().init();
			MsgMgr.getInstance().init();
			EnterGameController c = new EnterGameController();
			Player player = new Player();
			player.setUsername("b4");
			player.setPwd("2222");
			player.setPhoneNum("3333");
		
			c.registerPlayer(player);
			System.out.println(CommUtils.md5Encrypt("11").length());
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

}
