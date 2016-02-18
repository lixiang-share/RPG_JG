package jg.rpg.test.uitls;

import jg.rpg.common.manager.PlayerMgr;
import jg.rpg.config.ConfigMgr;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.msg.MsgMgr;
import jg.rpg.msg.enterService.EnterGameService;
import jg.rpg.msg.enterService.controller.EnterGameController;

import org.junit.Test;

public class TestEnterGameService {

	@Test
	public void testEnterGame() throws Exception {
		ConfigMgr.getInstance().init();
		DBMgr.getInstance().init();
		PlayerMgr.getInstance().init();
		MsgMgr.getInstance().init();
		EnterGameService c = new EnterGameService();
		Session session = new Session();
		Player player = new Player();
		player.setId(24);
		session.setPlayer(player);
		
		
		MsgPacker packer= new MsgPacker();
		packer.addInt(1)
			.addString("girl")
			.addString("a1")
			.addInt(1)
			.addInt(0);
		c.enterGame(session, new MsgUnPacker(packer.Serialize()));
	}

}
