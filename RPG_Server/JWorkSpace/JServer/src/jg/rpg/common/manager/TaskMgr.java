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

import jg.rpg.entity.msgEntity.Task;
import jg.rpg.exceptions.InitException;

public class TaskMgr {

	private Map<Integer , Task> taskMap;
	private static TaskMgr inst;
	private TaskMgr(){}
	
	public static TaskMgr getInstance(){
		synchronized(TaskMgr.class){
			if(inst == null){
				inst = new TaskMgr();
			}
		}
		return inst;
	}
	
	public void init() throws InitException, DocumentException{
		taskMap = new HashMap<Integer , Task>();
		LoadingAllTasks();
	}

	private void LoadingAllTasks() throws InitException, DocumentException {
		String path = System.getProperty("user.dir")+
				File.separator+"config"+File.separator+"taskList.xml";
		Document document = parse(path);
		if(document != null){
			process(document);
		}else{
			throw new InitException("ConfigMgr init error!!!");
		}
	}
	private Document parse(String path) throws DocumentException {
		SAXReader reader = new SAXReader(BeanDocumentFactory.getInstance());
		return reader.read(path);
	}

	private void process(Document document) {
		Element root = document.getRootElement();
		List<Element> eTasks = root.elements();
		for(Element e : eTasks){
			Task task = new Task();
			
			task.setId(Integer.parseInt(e.elementText("id")));
			task.setRoleId(Integer.parseInt(e.elementText("roleId")));
			task.setType(e.elementText("type"));
			task.setStatus(Integer.parseInt(e.elementText("status")));
			task.setDiamondCount(Integer.parseInt(e.elementText("diamondCount")));
			task.setGoldCount(Integer.parseInt(e.elementText("goldCount")));
			
			taskMap.put(task.getId(), task);
		}
	}
	
	public List<Task> getTaskList(){
		List<Task> tasks = new ArrayList<Task>();
		tasks.addAll(taskMap.values());
		return tasks;
	}

}
