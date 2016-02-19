package jg.rpg.entity.msgEntity;

public class Dog {
	private int id;
	private String username;
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
	public Dog(int id, String username) {
		super();
		this.id = id;
		this.username = username;
	}
}
