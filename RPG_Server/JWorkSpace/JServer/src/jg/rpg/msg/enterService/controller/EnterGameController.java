package jg.rpg.msg.enterService.controller;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.security.NoSuchAlgorithmException;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import org.apache.log4j.Logger;

import jg.rpg.common.exceptions.EntityHandlerException;
import jg.rpg.common.exceptions.PlayerHandlerException;
import jg.rpg.common.manager.DefEntityMgr;
import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.msgEntity.EquipItem;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.entity.msgEntity.Role;
import jg.rpg.entity.msgEntity.ServerEntity;
import jg.rpg.entity.msgEntity.Task;
import jg.rpg.utils.CommUtils;

public class EnterGameController {

	private DBMgr dbMgr;
	private Logger logger = Logger.getLogger(getClass());
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
	public Player registerPlayer(Player player) throws SQLException, EntityHandlerException  {
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
	public List<Task> getTaskListByRoleID(int ownerId) throws SQLException {
		String sql = "select * from tb_task where ownerId = ?";
		List<Task> tasks = DBHelper.GetAll(DBMgr.getInstance().getDataSource(), sql,
				RSHHelper.getTaskRSH(), ownerId);
		return tasks;
	}
	
	
	
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
				.addInt(player.getDamage())
				.addInt(player.getVit());
	}
	
	public void initPlayer(int playerID) throws SQLException, EntityHandlerException {
		List<Role> allDefRoles = DefEntityMgr.getInstance().getAllDefRoles();
		for(Role role : allDefRoles){
			role.setOwnerId(playerID);
			role.insertToDB();
		}
		
		List<Task> tasks = DefEntityMgr.getInstance().getTaskList();
		for(Task task : tasks){
			task.setOwnerId(playerID);
			task.setId(-1);
			task.insertToDB();
		}
		
		List<EquipItem> equips = DefEntityMgr.getInstance().getAllEquips();
		for(EquipItem item : equips){
			item.setOwnerId(playerID);
			item.insertToDB();
		}
	}

}
