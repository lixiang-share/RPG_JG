package jg.rpg.config;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import jg.rpg.entity.DBEntityInfo;
import jg.rpg.entity.NetEntityInfo;
import jg.rpg.exceptions.InitException;

import org.dom4j.Document;
import org.dom4j.Element;
import org.dom4j.bean.BeanDocumentFactory;
import org.dom4j.io.SAXReader;

public class ConfigMgr {

	private DBEntityInfo strogeDbInfo;
	private NetEntityInfo mainNetInfo;
	private List<String> handlerNames;

	private static boolean isInit;
	private static ConfigMgr inst;
	private ConfigMgr(){
	}
	public static ConfigMgr getInstance(){
		synchronized (ConfigMgr.class) {
			if(inst == null){
				inst = new ConfigMgr();
			}
		}
		return inst;
	}


	public void init() throws Exception {
		String path = System.getProperty("user.dir")+
				File.separator+"config"+File.separator+"config.xml";
		Document document = parse(path);
		if(document != null){
			process(document);
		}else{
			throw new InitException("ConfigMgr init error!!!");
		}
	}
	
	private Document parse(String url) throws Exception {
        SAXReader reader = new SAXReader(BeanDocumentFactory.getInstance());
        return reader.read(url);
    }
	
    private void process(Document document) throws Exception {
    	Element root = document.getRootElement();
    	readDBInfo(root);
    	readGameConfig(root);
    	readHandlerNames(root);
    }
	    
	public List<String> getHandlerNames() {
		return handlerNames;
	}
	private void readHandlerNames(Element root) {
		handlerNames = new ArrayList<String>();
		Element ehn = root.element("handlers");
		List<Element> eList = ehn.elements("handler");
		for(Element e : eList){
			String str = e.getText().trim();
			handlerNames.add(str);
		}
	}
	/**
	 * ∂¡»°DB–≈œ¢
	 * @param root
	 */
	private void readDBInfo(Element root){
    	Element eDB = root.element("db");
    	Element eSDB = eDB.element("StorageDB");
    	strogeDbInfo = new DBEntityInfo();
    	strogeDbInfo.setDriver(eSDB.elementTextTrim("driver"));
    	strogeDbInfo.setUser(eSDB.elementTextTrim("user"));
    	strogeDbInfo.setPwd(eSDB.elementTextTrim("passworld"));
    	strogeDbInfo.setUrl(eSDB.elementTextTrim("url"));
    }
	
    private void readGameConfig(Element root) {
    	Element eGC = root.element("gameConfig");
    	GameConfig.DefEncoding = eGC.elementTextTrim("encoding");
    	GameConfig.MsgHeadLen = Integer.parseInt(eGC.elementTextTrim("msgHeadLen"));
    	GameConfig.Delimiter = eGC.elementTextTrim("delimiter");
    	GameConfig.Max_Frame_Length = Integer.parseInt(eGC.elementTextTrim("max_msg_len"));
	}
    
	public DBEntityInfo getStrogeDbInfo() {
		return strogeDbInfo;
	}
 

}

