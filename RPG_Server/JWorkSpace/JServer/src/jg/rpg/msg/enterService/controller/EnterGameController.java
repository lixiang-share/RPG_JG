package jg.rpg.msg.enterService.controller;

import java.io.UnsupportedEncodingException;
import java.security.NoSuchAlgorithmException;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.entity.msgEntity.Role;
import jg.rpg.entity.msgEntity.ServerEntity;
import jg.rpg.entity.msgEntity.Task;
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
	public Player registerPlayer(Player player) throws SQLException, PlayerHandlerException  {
		
		/*String sql = "select * from tb_user where name = ?";
		Player _player = DBHelper.GetFirst(dbMgr.getDataSource(), sql, 
				RSHHelper.getPlayerRSH() ,player.getUsername());
		if(_player!= null){
			throw new PlayerHandlerException("player Exist : "+player);
		}
		sql = "insert into tb_user values(null,? ,?,?)";
		String pwd = CommUtils.md5Encrypt(player.getPwd());
		_player = DBHelper.insert(dbMgr.getDataSource(), sql, 
				RSHHelper.getPlayerRSH() ,player.getUsername() ,pwd,player.getPhoneNum());*/
		return player.insertToDB();
	}
	public List<Role> getRolesByPlayerID(int playerID) throws SQLException {
		String sql = "select * from tb_role where ownerId = ?";
		List<Role> roles = DBHelper.GetAll(dbMgr.getDataSource(), sql,
				RSHHelper.getRoleRSH(), playerID);
		return roles;
	}
	public ServerEntity getServerByID(int serverID) throws SQLException {
		String sql = "select * from tb_server where id = ?";
		return DBHelper.GetFirst(dbMgr.getDataSource(), sql, 
					RSHHelper.getServerEntityRSH() ,serverID);
	}
	public Role getRoleByPlayerIDAndRole_ID(int playerID, String roleId) throws SQLException {
		String sql = "select * from tb_role where ownerId = ? and roleId = ?";
		Role _role = DBHelper.GetFirst(dbMgr.getDataSource(), sql, 
				RSHHelper.getRoleRSH() ,playerID ,roleId);
		return _role;
	}
	public int updateRoleInfo(int playerID ,Role role) throws SQLException {
		List<Object> list = new ArrayList<Object>();
		StringBuilder sb = new StringBuilder("update tb_role set "); 
		if(role.getName() != null){
			sb.append("name = ? ,");
			list.add(role.getName());
		}
		if(role.getLevel() != 0){
			sb.append("level = ?,");
			list.add(role.getLevel());
		}
		if(role.getGender() == 0 || role.getGender() == 1){
			sb.append("gender = ?");
			list.add(role.getGender());
		}
		sb.append(" where ownerId = ? and roleId = ?");
		list.add(playerID);
		list.add(role.getRole_id());
		String sql = sb.toString();
		Object[] param = list.toArray();
		list = null;
		return DBHelper.update(dbMgr.getDataSource(), sql, param);
	}
	
	public Role insertRole( Role role) throws SQLException {
		String sql = "insert into tb_role values(null ,?,?,?,?,?)";
		Role _role = DBHelper.insert(dbMgr.getDataSource(), sql, 
				RSHHelper.getRoleRSH() ,role.getOwnerId() ,role.getRole_id(),role.getName(),role.getLevel(),role.getGender());
		return _role;
	}
	public List<Task> getTaskListByRoleID(int roleId) throws SQLException {
		String sql = "select * from tb_task where roleId = ?";
		List<Task> tasks = DBHelper.GetAll(DBMgr.getInstance().getDataSource(), sql,
				RSHHelper.getTaskRSH(), roleId);
		return tasks;
	}

}
