package jg.rpg.entity.msgEntity;

import java.sql.SQLException;
import java.util.List;

import jg.rpg.common.abstractClass.EntityBase;
import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;

public class Role extends EntityBase<Role> {

	private int id;
	private int ownerId;
	private String role_id;
	private String name;
	private int level;
	private int gender;

	

	public int getId() {
		return id;
	}
	public void setId(int id) {
		this.id = id;
	}
	public int getOwnerId() {
		return ownerId;
	}
	public void setOwnerId(int ownerId) {
		this.ownerId = ownerId;
	}
	public String getRole_id() {
		return role_id;
	}
	public void setRole_id(String role_id) {
		this.role_id = role_id;
	}
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public int getLevel() {
		return level;
	}
	public void setLevel(int level) {
		this.level = level;
	}
	public int getGender() {
		return gender;
	}
	public void setGender(int gender) {
		this.gender = gender;
	}
	
//=====================================================	

	@Override
	public boolean isExistInDB() throws SQLException {
		String sql = "select * from tb_role where ownerId = ? and roleId = ?";
		Role role = DBHelper.GetFirst(DBMgr.getInstance().getDataSource(), sql,
				RSHHelper.getRoleRSH(), getOwnerId() , getRole_id());
		return role != null;
	}
	
	@Override
	public Role insertToDB() throws SQLException {
		if( isExistInDB()){
			throw new SQLException("Role has Exist");
		}
		String sql = "insert into tb_role values(null , ? , ? ,? , ? , ?)";
		Role role = DBHelper.insert(DBMgr.getInstance().getDataSource(), sql,
				RSHHelper.getRoleRSH(), getOwnerId(),getRole_id(),getName(),getLevel() , getGender());
		setId(role.getId());
		return this;
	}
	@Override
	public int updateToDB() throws SQLException {
		if( !isExistInDB()){
			throw new SQLException("Role not Exist");
		}
		String sql = "update tb_role set name = ? , level = ? , gender = ?  where ownerId = ? and roleId = ? ";
		int row = DBHelper.update(DBMgr.getInstance().getDataSource(), sql,
				getName() , getLevel(),getGender() , getOwnerId() , getRole_id());
		return row;
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}
