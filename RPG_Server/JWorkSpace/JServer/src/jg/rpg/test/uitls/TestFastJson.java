package jg.rpg.test.uitls;

import org.junit.Test;

import com.alibaba.fastjson.JSON;

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
		
		String jsonString = JSON.toJSONString(task);
		System.out.println(jsonString);
	}

}
