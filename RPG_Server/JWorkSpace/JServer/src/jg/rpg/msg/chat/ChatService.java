package jg.rpg.msg.chat;

import io.netty.buffer.ByteBuf;

import java.io.IOException;
import java.util.Date;

import org.apache.log4j.Logger;

import jg.rpg.common.anotation.HandlerMsg;
import jg.rpg.common.protocol.MsgProtocol;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.utils.MsgUtils;

public class ChatService {
	private Logger logger = Logger.getLogger(getClass());
	
	@HandlerMsg(msgType = MsgProtocol.Login)
	public void handlerChatMsg(Session session , MsgUnPacker unpacker){
		try {
			logger.debug(unpacker.popInt());
			logger.debug(unpacker.popString());
		} catch (IOException e1) {
			e1.printStackTrace();
		}
		MsgPacker _msg = new MsgPacker();
		try {
		//	_msg.addInt(10);
		//	_msg.addDouble(521);
		//	_msg.addFloat(0.1f);
			_msg.addIntArray(new int[]{100,101,102,103});
			_msg.addString("hello client");
		} catch (IOException e) {
			e.printStackTrace();
		}
		ByteBuf buff = null;
		try {
			buff = MsgUtils.serializerMsg(_msg);
		} catch (IOException e) {
			e.printStackTrace();
		}
		String str = "您已经开启与服务端链接"+" "+new Date()+" "+session.getCtx().channel().localAddress();
		session.getCtx().writeAndFlush(buff);
	}
}
