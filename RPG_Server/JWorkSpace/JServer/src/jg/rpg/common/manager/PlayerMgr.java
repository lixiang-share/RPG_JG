package jg.rpg.common.manager;

import java.util.HashMap;
import java.util.Map;

import jg.rpg.entity.Session;
import jg.rpg.exceptions.PlayerHandlerException;

public class PlayerMgr {

	private static PlayerMgr inst;
	private Map<String,Session> playersMap;
	
	private PlayerMgr(){}
	public static PlayerMgr getInstance(){
		synchronized (PlayerMgr.class) {
			if(inst == null)
				inst = new PlayerMgr();
			return inst;
		}
	}
	public void init(){
		playersMap = new HashMap<String,Session>();
	}
	
	public boolean containPlayer(String sessionKey){
		synchronized (this) {
			return playersMap.containsKey(sessionKey);
		}
	}
	
	public  void addPlayer(String sessionKey , Session session) 
			throws PlayerHandlerException{
		if(containPlayer(sessionKey)){
			throw new PlayerHandlerException("add palyer exception and sessionkey exist");
		}
		synchronized (this) {
			playersMap.put(sessionKey, session);
		}
	}
	
	public void removePlayer(String sessionKey) throws PlayerHandlerException{
		if(!containPlayer(sessionKey)){
			throw new PlayerHandlerException("user not exist");
		}
		synchronized (this) {
			playersMap.remove(sessionKey);
		}
	}
	
	public Session getSession(String sessionKey) throws PlayerHandlerException{
		if(!containPlayer(sessionKey)){
			throw new PlayerHandlerException("user not exist");
		}
		synchronized (this) {
			return playersMap.get(sessionKey);
		}
	}
	
	public Session getVaildSession(String sessionkey) throws PlayerHandlerException{
		Session session = getSession(sessionkey);
		if(session.isVaild()){
			return session;
		}else{
			return null;
		}
	}
}
