package jg.rpg.entity.msgEntity;

import java.sql.SQLException;

import jg.rpg.common.abstractClass.EntityBase;
import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;

public class Task extends EntityBase<Task>{

	private int id;
	private String type;
	private int roleId;
	private int status;
	private int goldCount;
	private int diamondCount;
	
	public int getRoleId() {
		return roleId;
	}
	public int getDiamondCount() {
		return diamondCount;
	}
	public void setDiamondCount(int diamondCount) {
		this.diamondCount = diamondCount;
	}
	public void setRoleId(int roleId) {
		this.roleId = roleId;
	}
	public int getId() {
		return id;
	}
	public void setId(int id) {
		this.id = id;
	}
	public String getType() {
		return type;
	}
	public void setType(String type) {
		this.type = type;
	}
	public int getStatus() {
		return status;
	}
	public void setStatus(int status) {
		this.status = status;
	}
	public int getGoldCount() {
		return goldCount;
	}
	public void setGoldCount(int goldCount) {
		this.goldCount = goldCount;
	}
	@Override
	public boolean isNeedDelete() {
		return getStatus() >= 4 ? true : false;
	}
	@Override
	public boolean isExistInDB() throws SQLException {
		String sql = "select * from tb_task where id = ? and roleId = ?";
		Task task = DBHelper.GetFirst(DBMgr.getInstance().getDataSource(), sql,
				RSHHelper.getTaskRSH(), getId() , getRoleId());
		return task != null;
	}

	@Override
	public int updateToDB() throws SQLException {
		String sql = "update tb_task set status = ? where id = ? and roleId = ?";
		
		return DBHelper.update(DBMgr.getInstance().getDataSource(), sql 
				,getStatus(), getId() , getRoleId());
	}
	@Override
	public int deleteFromDB() throws SQLException {
		String sql = "delete from tb_task where id = ?";
		return DBHelper.update(DBMgr.getInstance().getDataSource(), sql , getId());
	}
	
	@Override
	public Task insertToDB() throws SQLException {
		if(isExistInDB()){
			throw new SQLException("Task has Exist!");
		}
		String sql = "insert into tb_task values(null ,?,?,?,?,?)";
		Task task = DBHelper.insert(DBMgr.getInstance().getDataSource(), sql,
				RSHHelper.getTaskRSH(), getRoleId() , getType(),getStatus() , getGoldCount() , getDiamondCount());
		setId(task.getId());
		return this;
	}
	
}
