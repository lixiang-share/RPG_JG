package jg.rpg.dao.db;

import java.sql.ResultSet;
import java.sql.SQLException;

import org.apache.commons.dbutils.ResultSetHandler;

import jg.rpg.entity.Player;
import jg.rpg.entity.msgEntity.Cat;
import jg.rpg.entity.msgEntity.ServerEntity;
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
				return player;
			}
		};
		return rsh;
	}
}

