package jg.rpg.entity.msgEntity;

import java.sql.SQLException;

import jg.rpg.common.abstractClass.EntityBase;
import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;
import jg.rpg.exceptions.PlayerHandlerException;
import jg.rpg.utils.CommUtils;

public class Player extends EntityBase<Player> {

	private int id;
	private String username;
	private String pwd;
	private String phoneNum;
	private int level;

	private int fc;
	private int exp;
	private int diamondCount;
	private int goldCount;
	private int vit;
	private int toughen;
	private int hp;
	private int damage;
	
	private ServerEntity server;
	
	
	
	public int getLevel() {
		return level;
	}
	public void setLevel(int level) {
		this.level = level;
	}
	public int getFc() {
		return fc;
	}
	public void setFc(int fc) {
		this.fc = fc;
	}
	public int getExp() {
		return exp;
	}
	public void setExp(int exp) {
		this.exp = exp;
	}
	public int getDiamondCount() {
		return diamondCount;
	}
	public void setDiamondCount(int diamondCount) {
		this.diamondCount = diamondCount;
	}
	public int getGoldCount() {
		return goldCount;
	}
	public void setGoldCount(int goldCount) {
		this.goldCount = goldCount;
	}
	public int getVit() {
		return vit;
	}
	public void setVit(int vit) {
		this.vit = vit;
	}
	public int getToughen() {
		return toughen;
	}
	public void setToughen(int toughen) {
		this.toughen = toughen;
	}
	public int getHp() {
		return hp;
	}
	public void setHp(int hp) {
		this.hp = hp;
	}
	public int getDamage() {
		return damage;
	}
	public void setDamage(int damage) {
		this.damage = damage;
	}
	private Role role;
	
	
	
	public ServerEntity getServer() {
		return server;
	}
	public void setServer(ServerEntity server) {
		this.server = server;
	}
	public Role getRole() {
		return role;
	}
	public void setRole(Role role) {
		this.role = role;
	}
	public String getPhoneNum() {
		return phoneNum;
	}
	public void setPhoneNum(String phoneNum) {
		this.phoneNum = phoneNum;
	}
	public int getId() {
		return id;
	}
	public void setId(int id) {
		this.id = id;
	}
	public String getUsername() {
		return username;
	}
	public void setUsername(String username) {
		this.username = username;
	}
	public String getPwd() {
		return pwd;
	}
	public void setPwd(String pwd) {
		this.pwd = pwd;
	}
	
	
	//=====================数据库信息同步=========================
	
	@Override
	public boolean isExistInDB() throws SQLException, PlayerHandlerException {
		String sql = "select * from tb_user where name = ?";
		Player _player = DBHelper.GetFirst(DBMgr.getInstance().getDataSource(), sql, 
				RSHHelper.getPlayerRSH() ,getUsername());
		return _player != null;
	}
	
	@Override
	public Player insertToDB() throws SQLException, PlayerHandlerException {
		if(isExistInDB()){
			throw new PlayerHandlerException("player Exist : "+this);
		}
		String sql = "insert into tb_user values(null,?,?,?,?,?,?,?,?,?,?,?,?)";
		String pwd = CommUtils.md5Encrypt(getPwd());
		Player _player = DBHelper.insert(DBMgr.getInstance().getDataSource(), sql, RSHHelper.getPlayerRSH() ,
				getUsername() ,pwd,getPhoneNum(),getLevel(),getFc(),
				getExp(),getDiamondCount(),getGoldCount(),getVit(),
				getToughen(),getHp(),getDamage());
		setId(_player.getId());
		return this;
	}
	

	@Override
	public String toString() {
		return "Player [id=" + id + ", username=" + username + ", pwd=" + pwd
				+ ", phoneNum=" + phoneNum + "]";
	}


}
