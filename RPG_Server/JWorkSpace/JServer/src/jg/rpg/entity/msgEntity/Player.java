package jg.rpg.entity.msgEntity;

import java.io.IOException;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import jg.rpg.common.abstractClass.EntityBase;
import jg.rpg.common.exceptions.EntityHandlerException;
import jg.rpg.common.exceptions.EquipException;
import jg.rpg.common.exceptions.GoldNotEnoughException;
import jg.rpg.common.exceptions.PlayerHandlerException;
import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;
import jg.rpg.entity.MsgPacker;
import jg.rpg.utils.CommUtils;

public class Player extends EntityBase<Player> {
	public static int MaxVit = 100;  
	public static int MaxToughen = 50; 
	private int id;
	private String username;
	private String pwd;
	private String phoneNum;
	private int level;
	private int vip;
	private int fc;
	private int exp;
	private int diamondCount;
	private int goldCount;
	private int vit;
	private int toughen;
	private int hp;
	private int damage;
	private Role role;
	private ServerEntity server;
	private List<Task> tasks;
	private Map<Integer , EquipItem> equipMap;
	private List<Skill> skills;
	

	public List<Skill> getSkills() {
		return skills;
	}
	public void setSkills(List<Skill> skills) {
		this.skills = skills;
	}
	public List<Skill> getSkillsByCurRole() {
		List<Skill> _skills = new ArrayList<Skill>();
		for(Skill s : getSkills()){
			if(s.getRoleGender() == getRole().getGender())
				_skills.add(s);
		}
		return _skills;
	}
	
	
	public Map<Integer, EquipItem> getEquipMap() {
		return equipMap;
	}
	public void setEquips(List<EquipItem> equipList) {
		if(equipList == null || equipList.isEmpty()) return;
		if(equipMap == null) equipMap = new HashMap<Integer , EquipItem>();
		for(EquipItem item : equipList){
			equipMap.put(item.getEquipId(), item);
		}
	}
	public List<EquipItem> getAllEquips(){
		List<EquipItem> equips = new ArrayList<EquipItem>(getEquipMap().values());
		return equips;
	}
	public boolean isHasEquip(int equipID){
		return equipMap.containsKey(equipID);
	}
	public EquipItem getEquipByEquipID(int equipID){
		return equipMap.get(equipID);
	}
	
	public List<Task> getTasks() {
		return tasks;
	}
	public void setTasks(List<Task> tasks) {
		this.tasks = tasks;
	}
	
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
		if(vit > MaxVit) vit = MaxVit;
		this.vit = vit;
	}
	public int getToughen() {
		return toughen;
	}
	public void setToughen(int toughen) {
		if(toughen > MaxToughen) toughen = MaxToughen;
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
	@Override
	public String toString() {
		return "Player [id=" + id + ", username=" + username + ", pwd=" + pwd
				+ ", phoneNum=" + phoneNum + "]";
	}
	
	//==================  Player 的一些行为方法封装  =========================
	public void updateField(String key, String value) {
		if(key.equals("name")){
			setUsername(value);
		}else if(key.equals("phone")){
			setPhoneNum(value);
		}else if(key.equals("level")){
			setLevel(Integer.parseInt(value));
		}else if(key.equals("fc")){
			setFc(Integer.parseInt(value));
		}else if(key.equals("exp")){
			setExp(Integer.parseInt(value));
		}else if(key.equals("diamondCount")){
			setDiamondCount(Integer.parseInt(value));
		}else if(key.equals("goldCount")){
			setGoldCount(Integer.parseInt(value));
		}else if(key.equals("vit")){
			setVit(Integer.parseInt(value));
		}else if(key.equals("toughen")){
			setToughen(Integer.parseInt(value));
		}else if(key.equals("hp")){
			setHp(Integer.parseInt(value));
		}else if(key.equals("damage")){
			setDamage(Integer.parseInt(value));
		}
	}

	
	public void pack(MsgPacker packer) throws IOException {
		packer.addInt(getId())
			.addString(getUsername())
			.addString(getPhoneNum())
			.addInt(getLevel())
			.addInt(getFc())
			.addInt(getExp())
			.addInt(getDiamondCount())
			.addInt(getGoldCount())
			.addInt(getVit())
			.addInt(getToughen())
			.addInt(getHp())
			.addInt(getDamage())
			.addInt(getVit());
	}

	
	//=====================数据库信息同步=========================
	
	@Override
	public boolean isExistInDB() throws SQLException, EntityHandlerException {
		String sql = "select * from tb_user where name = ?";
		Player _player = DBHelper.GetFirst(DBMgr.getInstance().getDataSource(), sql, 
				RSHHelper.getPlayerRSH() ,getUsername());
		return _player != null;
	}
	
	@Override
	public Player insertToDB() throws SQLException, EntityHandlerException {
		if(isExistInDB()){
			throw new EntityHandlerException("player Exist : "+this);
		}
		String sql = "insert into tb_user values(null,?,?,?,?,?,?,?,?,?,?,?,?,?)";
		String pwd = CommUtils.md5Encrypt(getPwd());
		Player _player = DBHelper.insert(DBMgr.getInstance().getDataSource(), sql, RSHHelper.getPlayerRSH() ,
				getUsername() ,pwd,getPhoneNum(),getLevel(),getFc(),
				getExp(),getDiamondCount(),getGoldCount(),getVit(),
				getToughen(),getHp(),getDamage(),getVit());
		setId(_player.getId());
		return this;
	}
	
	@Override
	public int updateToDB() throws SQLException, EntityHandlerException {
		String sql = "update tb_user set phone = ? , level = ? , fc = ? , exp = ? ,diamondCount =? , goldCount = ? , vit = ?,toughen = ? ,hp = ? , damage = ? , vip = ? where id = ?";
		return DBHelper.update(DBMgr.getInstance().getDataSource(),sql,getPhoneNum() , getLevel() , getFc() ,
				getExp(),getDiamondCount() , getGoldCount() ,getVit() ,getToughen() ,getHp(),getDamage(),getVit() , getId());
	}
	//=================================== 装备相关操作  ==============================
	public boolean isTypeDressed(String equipType){
		boolean rst = false;
		for(EquipItem equip : getAllEquips()){
			if(equip.isDress() && equip.getType().equals(EquipItem.Equip) 
					&& equipType.equals(equip.getEquipType())){
				rst = true;
			}
		}
		return rst;
	}
	
	public void UpdateEquipInfo(EquipItem equip) throws SQLException, EntityHandlerException{
		//更新装备信息
		if(equip.getAmount() == 0){
			equipMap.remove(equip.getEquipId());
			equip.deleteFromDB();
		}else{
			equip.updateToDB();
		}
		//更新玩家信息
		updateToDB();
	}
	public boolean isGenderMatch(boolean isMan) {
		Role  role = getRole();
		int gender = role.getGender();
		if(isMan && gender == 1){
			return true;
		}else if(!isMan && gender == 2)
			return true;
		else{
			return false;
		}
	}
	public void dressEquip(int equipID , boolean isSync) throws EquipException, SQLException, EntityHandlerException{
		EquipItem equip = getEquipByEquipID(equipID);
		if(equip == null )  throw new EquipException("玩家缺少装备" + getUsername());
		if(equip.isDress()) throw new EquipException("该装备已穿戴 : " +equip.getEquipId()+" : "+ getUsername());
		if(equip.getType().equals(EquipItem.Equip) 
				&& isTypeDressed(equip.getEquipType())){
			throw new EquipException("同种类型装备不能重复穿戴  ");
		}
		if(equip.getType().equals(EquipItem.Equip) 
				&& !isGenderMatch(equip.isMan())){
			throw new EquipException("与玩家选择角色性别不匹配 ");
		}
		if(equip.getType().equals(EquipItem.Equip)){
			setHp(getHp() + equip.getHp());
			setDamage(getDamage() + equip.getDamage());
			setFc(getFc() + equip.getFc());
			equip.setDress(true);
		}else if(equip.getType().equals(EquipItem.Drug)){
			if(equip.getEffectType().equals(EquipItem.Energy)){
				if(equip.getEffectType().equals(EquipItem.Energy) && getVit() >= MaxVit){
					throw new EquipException("超过体力上限，无需再补充");
				}
				if(equip.getAmount() >= 1){
					setVit(getVit() + equip.getEffectValue());
					equip.setAmount(equip.getAmount() - 1);
				}else{
					equip.deleteFromDB();
					throw new EquipException("物品数量不足 ");
				}
			}
		}else{
			//宝箱
		}
		if(isSync){
			UpdateEquipInfo(equip);
		}

	}
	

	public void undressEquip(int equipID , boolean isSync) throws EquipException, SQLException, EntityHandlerException{
		EquipItem equip = getEquipByEquipID(equipID);
		if(equip == null || !equip.isDress()) throw new EquipException("该装备未穿戴");
		if(!equip.getType().equals(EquipItem.Equip)){
			throw new EquipException("无法卸载非装备物品");
		}
		if(equip.getType().equals(EquipItem.Equip)){
			setHp(getHp() - equip.getHp());
			setDamage(getDamage() - equip.getDamage());
			setFc(getFc() - equip.getFc());
		}else{
			//宝箱
		}
		equip.setDress(false);
		if(isSync){
			UpdateEquipInfo(equip);
		}
	}
	
	public void upgradeEquip(int equipID , boolean isSync) throws EquipException, SQLException, EntityHandlerException, GoldNotEnoughException{
		EquipItem equip = getEquipByEquipID(equipID);
		if(equip == null || !equip.getType().equals(EquipItem.Equip)) throw new EquipException("无法升级该装备");
		int goldNeed = (equip.getLevel() + 1)*equip.getPrice();
		if(getGoldCount() - goldNeed < 0 ) 
			throw new GoldNotEnoughException("升级装备金币不足 ");
		boolean isDress = equip.isDress();
		if(isDress)
			this.undressEquip(equipID, isSync);
		setGoldCount(getGoldCount() - goldNeed);
		equip.upgrade();
		if(isDress)
			this.dressEquip(equipID, isSync);
		else
			equip.updateToDB();
		
	}
	public void saleEquip(int equipID , boolean isSync) throws EquipException, SQLException, EntityHandlerException{
		EquipItem equip = getEquipByEquipID(equipID);
		if(equip == null) throw new EquipException("装备不存在");
		if(equip.isDress()) throw new EquipException("已装备的物品无法卸载");
		if(equip.getAmount() >= 1){
			setGoldCount(getGoldCount()+equip.getPrice()); 
			equip.setAmount(equip.getAmount()-1);
		}else{
			throw new EquipException("装备数量不足 ：" +equip);
		}
		if(isSync)
			UpdateEquipInfo(equip);
	}
	
	public void discardEquip(int equipID) throws EquipException, SQLException, EntityHandlerException{
		EquipItem equip = getEquipByEquipID(equipID);
		if(equip == null) throw new EquipException("装备不存在：" +equip);
		this.undressEquip(equipID, false);
		equip.deleteFromDB();
	}

	///==================  Task ===========================
	public Task getTaskByID(int taskID){
		for(Task task : getTasks()){
			if(task.getTaskId() == taskID) return task;
		}
		return null;
	}
	
	public void acceptTask(int taskID) throws SQLException{
		Task task = getTaskByID(taskID);
		if(task != null){
			task.setStatus(Task.NotComplete);
			task.updateToDB();
		}
	}
	public Skill getSkillBySkillID(int skillID) {
		for(Skill skill : getSkillsByCurRole()){
			if(skill.getSkillID() == skillID)
				return skill;
		}
		return null;
	}
}
