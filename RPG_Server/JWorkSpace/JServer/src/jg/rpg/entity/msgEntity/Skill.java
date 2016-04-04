package jg.rpg.entity.msgEntity;

import java.io.IOException;
import java.sql.SQLException;

import jg.rpg.common.abstractClass.EntityBase;
import jg.rpg.common.exceptions.EntityHandlerException;
import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;
import jg.rpg.entity.MsgPacker;

public class Skill extends EntityBase<Skill>{
	public final static String Man = "Warrior";
	public final static String Woman = "FemaleAssassin";
	public final static String One = "one";
	public final static String Two = "two";
	public final static String Three = "three";
	public final static String Base = "Base";
	public final static int UpgradeUniteVal = 500;
	
	private int id;
	private int skillID;
	private int ownerID;
	private String roleType;
	private String type;
	private String pos;
	private int coldTime;
	private int baseFC;
	private int fc;
	private Player owner;
	private int level;
	
	public Player getOwner() {
		return owner;
	}
	public void setOwner(Player owner) {
		this.owner = owner;
	}
	public int getFc() {
		if(Base.equalsIgnoreCase(getType())){
			return baseFC * owner.getLevel();
		}else{
			return baseFC * getLevel();
		}
	}

	
	public int getId() {
		return id;
	}
	public int getSkillID() {
		return skillID;
	}
	public int getOwnerID() {
		return ownerID;
	}
	public String getRoleType() {
		return roleType;
	}
	public String getType() {
		return type;
	}
	public String getPos() {
		return pos;
	}
	public int getColdTime() {
		return coldTime;
	}
	public int getBaseFC() {
		return baseFC;
	}
	public int getLevel() {
		return level;
	}

	public void setId(int id) {
		this.id = id;
	}
	public void setSkillID(int skillID) {
		this.skillID = skillID;
	}
	public void setOwnerID(int ownerID) {
		this.ownerID = ownerID;
	}
	public void setRoleType(String roleType) {
		this.roleType = roleType;
	}
	public void setType(String type) {
		this.type = type;
	}
	public void setPos(String pos) {
		this.pos = pos;
	}
	public void setColdTime(int coldTime) {
		this.coldTime = coldTime;
	}
	public void setBaseFC(int baseFC) {
		this.baseFC = baseFC;
	}
	public void setLevel(int level) {
		this.level = level;
	}
	
	
	@Override
	public String toString() {
		return "Skill [id=" + id + ", skillID=" + skillID + ", ownerID="
				+ ownerID + ", roleType=" + roleType + ", type=" + type
				+ ", pos=" + pos + ", coldTime=" + coldTime + ", baseFC="
				+ baseFC + ", level=" + level + "]";
	}
	public int getRoleGender(){
		if(Skill.Man.equals(getRoleType())){
			return Role.Man;
		}else if(Skill.Woman.equals(getRoleType())){
			return Role.Woman;
		}
		return Role.Unknow;
	}
	
	
	
	@Override
	public boolean isExistInDB() throws SQLException, EntityHandlerException {
		String sql = "select * from tb_skill where skillId = ? and ownerId = ?";
		Skill skill = DBHelper.GetFirst(DBMgr.getInstance().getDataSource(), sql,
				RSHHelper.getSkillRSH(), getSkillID() , getOwnerID());
		return skill != null;
	}
	
	@Override
	public Skill insertToDB() throws SQLException, EntityHandlerException {
		if(isExistInDB()){
			throw new SQLException("Skill has Exist!");
		}
		String sql = "insert into tb_skill values(null,? ,?,?,?,?,?,?,?)";
		Skill skill = DBHelper.insert(DBMgr.getInstance().getDataSource(), sql,
				RSHHelper.getSkillRSH(), getOwnerID(),getSkillID(),getRoleType(),getType(),
				getPos(),getColdTime(),getBaseFC(),getLevel());
		setId(skill.getId());
		return this;
	}
	@Override
	public int updateToDB() throws SQLException, EntityHandlerException {
		String sql = "update tb_skill set level = ? where id = ?";
		return DBHelper.update(DBMgr.getInstance().getDataSource(), sql 
				,getLevel(), getId());
	}
	
	
	public MsgPacker packToMsg(MsgPacker packer) throws IOException{
		packer.addInt(getId())
			.addInt(getOwnerID())
			.addInt(getSkillID())
			.addString(getRoleType())
			.addString(getType())
			.addString(getPos())
			.addInt(getColdTime())
			.addInt(getFc())
			.addInt(getLevel());
		return packer;
	}
	
}
