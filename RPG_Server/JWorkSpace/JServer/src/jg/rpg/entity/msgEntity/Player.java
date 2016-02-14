package jg.rpg.entity.msgEntity;

import java.sql.ResultSet;
import java.sql.SQLException;

import org.apache.commons.dbutils.ResultSetHandler;

public class Player {

	private int id;
	private String username;
	private String pwd;
	private String phoneNum;
	private ServerEntity server;
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
	@Override
	public String toString() {
		return "Player [id=" + id + ", username=" + username + ", pwd=" + pwd
				+ ", phoneNum=" + phoneNum + "]";
	}

}
