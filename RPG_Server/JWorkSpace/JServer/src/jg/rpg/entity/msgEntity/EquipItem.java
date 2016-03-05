package jg.rpg.entity.msgEntity;

public class EquipItem {
	//¾²Ì¬³£Á¿
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
		return damage;
	}
	public void setDamage(int damage) {
		this.damage = damage;
	}
	public int getHp() {
		return hp;
	}
	public void setHp(int hp) {
		this.hp = hp;
	}
	public int getFc() {
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

	
}
