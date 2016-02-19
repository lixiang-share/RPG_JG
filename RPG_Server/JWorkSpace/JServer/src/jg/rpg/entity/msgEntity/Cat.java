package jg.rpg.entity.msgEntity;

import java.util.List;

public class Cat {

	private String id;
	private String breed;
	private String name;
	private List<Dog> dogs;
	
	
	
	public List<Dog> getDogs() {
		return dogs;
	}
	public void setDogs(List<Dog> dogs) {
		this.dogs = dogs;
	}
	public String getId() {
		return id;
	}
	public void setId(String id) {
		this.id = id;
	}
	public String getBreed() {
		return breed;
	}
	public void setBreed(String breed) {
		this.breed = breed;
	}
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	@Override
	public String toString() {
		return "Cat [id=" + id + ", breed=" + breed + ", name=" + name + "]";
	}
	
	
}
