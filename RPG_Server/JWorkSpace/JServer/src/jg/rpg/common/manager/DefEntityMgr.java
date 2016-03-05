package jg.rpg.common.manager;

import java.io.File;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.Element;
import org.dom4j.bean.BeanDocumentFactory;
import org.dom4j.io.SAXReader;

import jg.rpg.entity.msgEntity.Role;
import jg.rpg.entity.msgEntity.Task;
import jg.rpg.exceptions.InitException;

public class DefEntityMgr {

	private Map<Integer , Task> taskMap;
	private List<Role> defRoles;
	private static DefEntityMgr inst;
	private DefEntityMgr(){}
	
	public static DefEntityMgr getInstance(){
		synchronized(DefEntityMgr.class){
			if(inst == null){
				inst = new DefEntityMgr();
			}
		}
		return inst;
	}
	
	public void init() throws InitException, DocumentException{
		taskMap = new HashMap<Integer , Task>();
		defRoles = new ArrayList<Role>();
		LoadingAllTasks();
		LoadingAllRoles();
	}

	private void LoadingAllRoles() throws DocumentException, InitException {
		String path = System.getProperty("user.dir")+
				File.separator+"config"+File.separator+"roleList.xml";
		Document document = parse(path);
		if(document != null){
			processRoles(document);
		}else{
			throw new InitException("ConfigMgr init error!!!");
		}
		
	}

	private void processRoles(Document document) {
		Element root = document.getRootElement();
		List<Element> eRoles = root.elements();
		for(Element e : eRoles){
			Role role = new Role();
			role.setRole_id(e.elementText("role_id"));
			role.setName(e.elementText("name"));
			role.setLevel(Integer.parseInt(e.elementText("level")));
			role.setGender(Integer.parseInt(e.elementText("gender")));
			defRoles.add(role);
		}
	}
	public List<Role> getAllDefRoles(){
		return new ArrayList<Role>(defRoles);
	}

	private void LoadingAllTasks() throws InitException, DocumentException {
		String path = System.getProperty("user.dir")+
				File.separator+"config"+File.separator+"taskList.xml";
		Document document = parse(path);
		if(document != null){
			processTasks(document);
		}else{
			throw new InitException("ConfigMgr init error!!!");
		}
	}
	

	private Document parse(String path) throws DocumentException {
		SAXReader reader = new SAXReader(BeanDocumentFactory.getInstance());
		return reader.read(path);
	}

	private void processTasks(Document document) {
		Element root = document.getRootElement();
		List<Element> eTasks = root.elements();
		for(Element e : eTasks){
			Task task = new Task();
			
			task.setTaskId(Integer.parseInt(e.elementText("id")));
			task.setOwnerId(Integer.parseInt(e.elementText("roleId")));
			task.setType(e.elementText("type"));
			task.setStatus(Integer.parseInt(e.elementText("status")));
			task.setDiamondCount(Integer.parseInt(e.elementText("diamondCount")));
			task.setGoldCount(Integer.parseInt(e.elementText("goldCount")));
			task.setCurStage(Integer.parseInt(e.elementText("curStage")));
			task.setTotalStage(Integer.parseInt(e.elementText("totalStage")));
			taskMap.put(task.getTaskId(), task);
		}
	}
	
	public List<Task> getTaskList(){
		List<Task> tasks = new ArrayList<Task>();
		tasks.addAll(taskMap.values());
		return tasks;
	}

}
