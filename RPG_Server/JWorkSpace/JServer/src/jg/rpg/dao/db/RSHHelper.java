package jg.rpg.dao.db;

import java.sql.ResultSet;
import java.sql.SQLException;

import org.apache.commons.dbutils.ResultSetHandler;

import jg.rpg.entity.msgEntity.Cat;
import jg.rpg.entity.msgEntity.EquipItem;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.entity.msgEntity.Role;
import jg.rpg.entity.msgEntity.ServerEntity;
import jg.rpg.entity.msgEntity.Skill;
import jg.rpg.entity.msgEntity.Task;
/**
 * 集中处理model层的ORM映射问题
 * @author jiuguang
 */
public class RSHHelper {

	public static ResultSetHandler<Cat> getCatRSH(){
		ResultSetHandler<Cat> rsh = new ResultSetHandler<Cat>(){
			@Override
			public Cat handle(ResultSet rs) throws SQLException {
				if(!rs.next()){
					return null;
				}
				Cat cat = new Cat();
				cat.setId(rs.getString(1));
				cat.setBreed(rs.getString(2));
				cat.setName(rs.getString(3));
				return cat;
			}
		};
		return rsh;
	}
	
	public static ResultSetHandler<ServerEntity> getServerEntityRSH(){
		ResultSetHandler<ServerEntity> rsh = new ResultSetHandler<ServerEntity>(){
			@Override
			public ServerEntity handle(ResultSet rs) throws SQLException {
				if(!rs.next()){
					return null;
				}
				ServerEntity server = new ServerEntity();
				server.setId(rs.getInt(1));
				server.setName(rs.getString(2));
				server.setIp(rs.getString(3));
				server.setCount(rs.getInt(4));
				return server;
			}
		};
		return rsh;
	}
	
	public static ResultSetHandler<Player> getPlayerRSH(){
		ResultSetHandler<Player> rsh = new ResultSetHandler<Player>(){
			@Override
			public Player handle(ResultSet rs) throws SQLException {
				if(!rs.next()){
					return null;
				}
				Player player = new Player();
				int columnCount = rs.getMetaData().getColumnCount();
				if(columnCount >= 1)
					player.setId(rs.getInt(1));
				if(columnCount >= 2)
					player.setUsername(rs.getString("name"));
				if(columnCount >= 3)
					player.setPwd(rs.getString("pwd"));
				if(columnCount >= 4)
					player.setPhoneNum(rs.getString("phone"));
				if(columnCount >= 5)
					player.setLevel(rs.getInt("level"));
				if(columnCount >= 6)
					player.setFc(rs.getInt("fc"));
				if(columnCount >= 7)
					player.setExp(rs.getInt("exp"));
				if(columnCount >= 8)
					player.setDiamondCount(rs.getInt("diamondCount"));
				if(columnCount >= 9)
					player.setGoldCount(rs.getInt("goldCount"));
				if(columnCount >= 10)
					player.setVit(rs.getInt("vit"));
				if(columnCount >= 11)
					player.setToughen(rs.getInt("toughen"));
				if(columnCount >= 12)
					player.setHp(rs.getInt("hp"));
				if(columnCount >= 13)
					player.setDamage(rs.getInt("damage"));
				if(columnCount >= 14)
					player.setVit(rs.getInt("vip"));
				return player;
			}
		};
		return rsh;
	}

	public static ResultSetHandler<Role> getRoleRSH() {
		ResultSetHandler<Role> rsh = new ResultSetHandler<Role>(){
			@Override
			public Role handle(ResultSet rs) throws SQLException {
				if(!rs.next()){
					return null;
				}
				int columnCount = rs.getMetaData().getColumnCount();
				Role role = new Role();
				if(columnCount >=1 )
					role.setId(rs.getInt(1));
				if(columnCount >=2 )
					role.setOwnerId(rs.getInt("ownerId"));
				if(columnCount >=3 )
					role.setRole_id(rs.getString("roleId"));
				if(columnCount >=4 )
					role.setName(rs.getString("name"));
				if(columnCount >=5 )
					role.setLevel(rs.getInt("level"));
				if(columnCount >=6 )
					role.setGender(rs.getInt("gender"));
				return role;
			}
		};
		return rsh;
	}
	
	public static ResultSetHandler<Task> getTaskRSH(){
		ResultSetHandler<Task> rsh = new ResultSetHandler<Task>(){
			@Override
			public Task handle(ResultSet rs) throws SQLException {
				if(!rs.next()){
					return null;
				}
				int columnCount = rs.getMetaData().getColumnCount();
				Task task = new Task();
				if(columnCount >=1 )
					task.setId(rs.getInt(1));
				if(columnCount >=2 )
					task.setOwnerId(rs.getInt("ownerId"));
				if(columnCount >=3 )
					task.setType(rs.getString("type"));
				if(columnCount >=4 )
					task.setStatus(rs.getInt("status"));
				if(columnCount >=5 )
					task.setGoldCount(rs.getInt("goldCount"));
				if(columnCount >=6 )
					task.setDiamondCount(rs.getInt("diamondCount"));
				if(columnCount>=7)
					task.setTaskId(rs.getInt("taskId"));
				if(columnCount>=8) 
					task.setCurStage(rs.getInt("curStage"));
				if(columnCount >= 9)
					task.setTotalStage(rs.getInt("totalStage"));
				return task;
			}
		};
		return rsh;
	}
	
	
	
	public static ResultSetHandler<EquipItem> getEquipItemRSH(){
		ResultSetHandler<EquipItem> rsh = new ResultSetHandler<EquipItem>(){
			@Override
			public EquipItem handle(ResultSet rs) throws SQLException {
				if(!rs.next()){
					return null;
				}
				int columnCount = rs.getMetaData().getColumnCount();
				EquipItem item = new EquipItem();
				if(columnCount == 1){
					item.setId(rs.getInt(1));
					return item;
				}
				item.setId(rs.getInt(1));
				item.setOwnerId(rs.getInt("ownerId"));
				item.setEquipId(rs.getInt("equipId"));
				item.setLevel(rs.getInt("level"));
				item.setAmount(rs.getInt("amount"));
				item.setDress(rs.getBoolean("isDress"));
				item.setMan(rs.getBoolean("isMan"));
				item.setType(rs.getString("type"));
				item.setEquipType(rs.getString("equipType"));
				item.setPrice(rs.getInt("price"));
				item.setStar(rs.getInt("star"));
				item.setQuality(rs.getInt("quality"));
				item.setEffectType(rs.getString("effectType"));
				item.setEffectValue(rs.getInt("effectValue"));
				item.setHp(rs.getInt("hp"));
				item.setFc(rs.getInt("fc"));
				item.setDamage(rs.getInt("damage"));
				return item;
			}
		};
		return rsh;
	}
	
	
	
	
	
	public static ResultSetHandler<Skill> getSkillRSH(){
		ResultSetHandler<Skill> rsh = new ResultSetHandler<Skill>(){
			@Override
			public Skill handle(ResultSet rs) throws SQLException {
				if(!rs.next()){
					return null;
				}
				int columnCount = rs.getMetaData().getColumnCount();
				Skill skill = new Skill();
				if(columnCount >=1 )
					skill.setId(rs.getInt(1));
				if(columnCount >=2 )
					skill.setOwnerID(rs.getInt("ownerId"));
				if(columnCount >=3 )
					skill.setRoleType(rs.getString("roleType"));
				if(columnCount >= 4)
					skill.setSkillID(rs.getInt("skillId"));
				if(columnCount >=5 )
					skill.setType(rs.getString("type"));
				if(columnCount >=6 )
					skill.setPos(rs.getString("pos").toLowerCase());
				if(columnCount >=7 )
					skill.setColdTime(rs.getInt("coldTime"));
				if(columnCount>=8)
					skill.setBaseFC(rs.getInt("baseFC"));
				if(columnCount>=9) 
					skill.setLevel(rs.getInt("level"));
				return skill;
			}
		};
		return rsh;
	}
}

