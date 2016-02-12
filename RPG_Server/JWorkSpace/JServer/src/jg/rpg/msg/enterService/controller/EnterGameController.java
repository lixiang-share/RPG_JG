package jg.rpg.msg.enterService.controller;

import java.io.UnsupportedEncodingException;
import java.security.NoSuchAlgorithmException;
import java.sql.SQLException;
import java.util.List;

import jg.rpg.common.PlayerMgr;
import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;
import jg.rpg.entity.Player;
import jg.rpg.entity.Session;
import jg.rpg.entity.msgEntity.ServerEntity;
import jg.rpg.exceptions.PlayerHandlerException;
import jg.rpg.utils.CommUtils;

public class EnterGameController {

	private DBMgr dbMgr;
	public EnterGameController(){
		dbMgr = DBMgr.getInstance();
	}
	public List<ServerEntity> getServerList() throws SQLException {
		String sql = "select * from tb_server";
		List<ServerEntity> servers = DBHelper.GetAll(dbMgr.getDataSource(), 
				sql, RSHHelper.getServerEntityRSH());

		return servers;
	}
	public Player getUser(String username, String pwd) throws SQLException, NoSuchAlgorithmException, UnsupportedEncodingException {
		pwd = CommUtils.md5Encrypt(pwd);
		String sql = "select * from tb_user where name = ? and pwd = ?";
		Player player = DBHelper.GetFirst(dbMgr.getDataSource(), sql, 
				RSHHelper.getPlayerRSH() ,username ,pwd);
		return player;
	}
	public Player registerPlayer(Player player) throws SQLException, PlayerHandlerException, NoSuchAlgorithmException, UnsupportedEncodingException {
		
		String sql = "select * from tb_user where name = ?";
		Player _player = DBHelper.GetFirst(dbMgr.getDataSource(), sql, 
				RSHHelper.getPlayerRSH() ,player.getUsername());
		if(_player!= null){
			throw new PlayerHandlerException("player Exist : "+player);
		}
		sql = "insert into tb_user values(null,? ,?,?)";
		String pwd = CommUtils.md5Encrypt(player.getPwd());
		_player = DBHelper.insert(dbMgr.getDataSource(), sql, 
				RSHHelper.getPlayerRSH() ,player.getUsername() ,pwd,player.getPhoneNum());
		return _player;
	}

}
