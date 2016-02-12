package jg.rpg.entity;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;

import jg.rpg.common.intf.IMsgHandler;

public class MsgHandlerItem implements IMsgHandler {

	private Method handlerMethod;
	private Object obj;
	
	public Method getHandlerMethod() {
		return handlerMethod;
	}
	public void setHandlerMethod(Method handlerMethod) {
		this.handlerMethod = handlerMethod;
	}

	public Object getObj() {
		return obj;
	}
	public void setObj(Object obj) {
		this.obj = obj;
	}
	@Override
	public void handleMsg(Session session, MsgUnPacker unpacker) {
		try {
			handlerMethod.invoke(obj, session , unpacker);
		} catch (Exception e) {
			e.printStackTrace();
		} 
	}
	
	
}
