package jg.rpg.entity.msgEntity;

import java.sql.SQLException;

import jg.rpg.common.abstractClass.EntityBase;
import jg.rpg.common.exceptions.EntityHandlerException;
import jg.rpg.common.exceptions.PlayerHandlerException;
import jg.rpg.common.manager.DefEntityMgr;
import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;

public class EquipItem extends EntityBase{
	//静态常量
	public static final String Equip = "Equip" ;
	public static final String Drug = "Drug" ;
	public static final String Helm = "Helm" ;
	public static final String Cloth = "Cloth" ;
	public static final String Weapon = "Weapon" ;
	public static final String Shoes = "Shoes" ;
	public static final String Necklace = "Necklace" ;
	public static final String Bracelet = "Bracelet" ;
	public static final String Ring = "Ring" ;
	public static final String Wing = "Wing" ;
	public static final String Energy = "Energy";
	
	private int id;
	private int ownerId;
	private int equipId;
	private String type;
	private String equipType;
	private int price;
	private int star;
	private int quality;
	private int damage;
	private int hp;
	private int fc;
	private String effectType;
	private int effectValue;
	private int level;
	private int amount;
	private boolean isDress;
	private boolean isMan;
	private int defHp;
	private int defFc;
	private int defDamage;
	
	
	
	public int getDefHp() {
		return defHp;
	}
	public void setDefHp(int defHp) {
		this.defHp = defHp;
	}
	public int getDefFc() {
		return defFc;
	}
	public void setDefFc(int defFc) {
		this.defFc = defFc;
	}
	public int getDefDamage() {
		return defDamage;
	}
	public void setDefDamage(int defDamage) {
		this.defDamage = defDamage;
	}
	public boolean isMan() {
		return isMan;
	}
	public void setMan(boolean isMan) {
		this.isMan = isMan;
	}
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
	public int getEquipId() {
		return equipId;
	}
	public void setEquipId(int equipId) {
		this.equipId = equipId;
	}
	public String getType() {
		return type;
	}
	public void setType(String type) {
		this.type = type;
	}
	public String getEquipType() {
		return equipType;
	}
	public void setEquipType(String equipType) {
		this.equipType = equipType;
	}
	public int getPrice() {
		return price;
	}
	public void setPrice(int price) {
		this.price = price;
	}
	public int getStar() {
		return star;
	}
	public void setStar(int star) {
		this.star = star;
	}
	public int getQuality() {
		return quality;
	}
	public void setQuality(int quality) {
		this.quality = quality;
	}
	public int getDamage() {
		if(damage == 0){
			EquipItem defEquip = DefEntityMgr.getInstance().getDefEquip(getEquipId());
			damage = calculate(defEquip.getDefDamage());
		}
		return damage;
	}
	public void setDamage(int damage) {
		this.damage = damage;
	}
	public int getHp() {
		if(hp == 0){
			EquipItem defEquip = DefEntityMgr.getInstance().getDefEquip(getEquipId());
			hp = calculate(defEquip.getDefHp());
		}
		return hp;
	}
	public void setHp(int hp) {

		this.hp = hp;
	}
	public int getFc() {
		if(fc == 0){
			EquipItem defEquip = DefEntityMgr.getInstance().getDefEquip(getEquipId());
			fc = calculate(defEquip.getDefFc());
		}
		return fc;
	}
	public void setFc(int fc) {
		this.fc = fc;
	}
	public String getEffectType() {
		return effectType;
	}
	public void setEffectType(String effectType) {
		this.effectType = effectType;
	}
	public int getEffectValue() {
		return effectValue;
	}
	public void setEffectValue(int effectValue) {
		this.effectValue = effectValue;
	}
	public int getLevel() {
		return level;
	}
	public void setLevel(int level) {
		this.level = level;
	}
	public int getAmount() {
		return amount;
	}
	public void setAmount(int amount) {
		this.amount = amount;
	}
	public boolean isDress() {
		return isDress;
	}
	public void setDress(boolean isDress) {
		this.isDress = isDress;
	}

	
	public int calculate(int defalutValue){
		return getLevel()*defalutValue;
	}
	
	@Override
	public String toString() {
		return "EquipItem [id=" + id + ", ownerId=" + ownerId + ", equipId="
				+ equipId + ", type=" + type + ", equipType=" + equipType
				+ ", price=" + price + ", star=" + star + ", quality="
				+ quality + ", damage=" + damage + ", hp=" + hp + ", fc=" + fc
				+ ", effectType=" + effectType + ", effectValue=" + effectValue
				+ ", level=" + level + ", amount=" + amount + ", isDress="
				+ isDress + ", isMan=" + isMan + ", defHp=" + defHp
				+ ", defFc=" + defFc + ", defDamage=" + defDamage + "]";
	}
	//=======================  DB 相关操作  =====================
	
	@Override
	public int deleteFromDB() throws SQLException {
		String sql = "delete from tb_equips where id = ?";
		return DBHelper.update(DBMgr.getInstance().getDataSource(), sql , getId());
	}
	@Override
	public boolean isExistInDB() throws SQLException, EntityHandlerException {
		String sql = "select * from tb_equips where ownerId = ? and equipId = ?";
		EquipItem item = DBHelper.GetFirst(DBMgr.getInstance().getDataSource(), sql, RSHHelper.getEquipItemRSH(),
				getOwnerId() , getEquipId());
		return item != null;
	}
	@Override
	public Object insertToDB() throws SQLException, EntityHandlerException {
		if(isExistInDB()){
			throw new EntityHandlerException("EquipItem has exist "+ getOwnerId() +" : "+getEquipId());
		}
		String sql = "insert into tb_equips values(null , ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
		EquipItem item = DBHelper.insert(DBMgr.getInstance().getDataSource(), sql, RSHHelper.getEquipItemRSH(), 
				getOwnerId(),getEquipId() , getLevel(),getAmount(),
				isDress() , isMan(),getType(),
				getEquipType(),getPrice(),getStar(),getQuality(),
				getEffectType(),getEffectValue(),
				getHp(),getDamage(),getFc());
		setId(item.getId());
		return this;
	}
	@Override
	public int updateToDB() throws SQLException {
		String sql = "update tb_equips set level = ? , amount = ?,isDress = ?,price = ?,star = ? ,quality = ? ,effectValue = ?,hp = ?,damage= ?,fc = ? where id = ?";
		return DBHelper.update(DBMgr.getInstance().getDataSource(),sql, getLevel() , getAmount(),
				isDress() , getPrice() , getStar() ,getQuality() , getEffectValue() ,
				getHp() , getDamage() ,getFc(),getId());
	}
	public void upgrade() {
		setLevel(getLevel() + 1);
		EquipItem defEquip = DefEntityMgr.getInstance().getDefEquip(getEquipId());
		damage = calculate(defEquip.getDefDamage());
		fc = calculate(defEquip.getDefFc());
		hp = calculate(defEquip.getDefHp());
	}
	
}
