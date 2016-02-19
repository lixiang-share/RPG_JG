package jg.rpg.test.uitls;

import java.util.ArrayList;
import java.util.List;

import org.junit.Test;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.parser.Feature;

import jg.rpg.entity.msgEntity.Cat;
import jg.rpg.entity.msgEntity.Dog;
import jg.rpg.entity.msgEntity.Task;

public class TestFastJson {

	@Test
	public void testSerialize() {
		Task task = new Task();
		task.setId(0);
		task.setRoleId(1234);
		task.setStatus(3);
		task.setType("Main");
		task.setDiamondCount(1000);
		task.setGoldCount(4000);
		
		
		Cat cat = new Cat();
		cat.setId("catID");
		cat.setBreed("bread");
		cat.setName("cat");
		
		List<Dog> dogs = new ArrayList<Dog>();
		dogs.add(new Dog(1,"dog1"));
		dogs.add(new Dog(2,"dog2"));
		dogs.add(new Dog(3,"dog3"));
		cat.setDogs(dogs);
		
		String jsonString = JSON.toJSONString(cat);
		//JSON.parse(jsonString)
		//VO vo = JSON.parseObject("...", VO.class, Feature.DisableCircularReferenceDetect)
		System.out.println(jsonString);
	}

}
