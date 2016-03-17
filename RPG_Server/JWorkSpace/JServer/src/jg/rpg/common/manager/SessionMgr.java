package jg.rpg.common.manager;

import java.util.HashMap;
import java.util.Map;

import jg.rpg.common.exceptions.PlayerHandlerException;
import jg.rpg.entity.Session;

public class SessionMgr {

	private static SessionMgr inst;
	private Map<String,Session> sessionsMap;
	
	private SessionMgr(){}
	public static SessionMgr getInstance(){
		synchronized (SessionMgr.class) {
			if(inst == null)
				inst = new SessionMgr();
			return inst;
		}
	}
	public void init(){
		sessionsMap = new HashMap<String,Session>();
	}
	
	public boolean containPlayer(String sessionKey){
		synchronized (this) {
			return sessionsMap.containsKey(sessionKey);
		}
	}
	
	public  void addPlayer(String sessionKey , Session session) 
			throws PlayerHandlerException{
		if(containPlayer(sessionKey)){
			throw new PlayerHandlerException("add palyer exception and sessionkey exist");
		}
		synchronized (this) {
			sessionsMap.put(sessionKey, session);
		}
	}
	
	public void removePlayer(String sessionKey) throws PlayerHandlerException{
		if(!containPlayer(sessionKey)){
			throw new PlayerHandlerException("user not exist");
		}
		synchronized (this) {
			sessionsMap.remove(sessionKey);
		}
	}
	
	public Session getSession(String sessionKey) throws PlayerHandlerException{
		if(!containPlayer(sessionKey)){
			throw new PlayerHandlerException("user not exist");
		}
		synchronized (this) {
			return sessionsMap.get(sessionKey);
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
