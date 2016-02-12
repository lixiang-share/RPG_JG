package jg.rpg.net.handlers;

import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import jg.rpg.common.PlayerMgr;
import jg.rpg.common.protocol.MsgProtocol;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.exceptions.PlayerHandlerException;
import jg.rpg.msg.MsgMgr;
import jg.rpg.utils.MsgUtils;

import org.apache.log4j.Logger;

public class LoginValidateHandler extends SimpleChannelInboundHandler<MsgUnPacker> {
	private Logger logger = Logger.getLogger(getClass());
	private PlayerMgr playerMgr;
	private MsgMgr msgMgr;
	
	public LoginValidateHandler(){
		super();
		playerMgr = PlayerMgr.getInstance();
		msgMgr = MsgMgr.getInstance();
	}
	@Override
	protected void channelRead0(ChannelHandlerContext ctx, MsgUnPacker msg)
			throws Exception {
		logger.debug("LoginValidateHandler");
		String sessionKey = msg.popString();
		logger.debug("sessionKey : "+sessionKey);
		Session session = null;
		try {
			session = playerMgr.getVaildSession(sessionKey);
			logger.debug("getVaildSession : "+session);
		} catch (PlayerHandlerException e) {
			logger.debug("PlayerHandlerException : "+e.getMessage());
			int msgType = msg.getInt(1);
			if( msgType== MsgProtocol.Login || msgType == MsgProtocol.Register){
				session = new Session();
				session.setCtx(ctx);
			}
		}
		if(session != null){
			session.updata();
			session.setCtx(ctx);
			msgMgr.handleMsg(session, msg);
		}else{
			MsgPacker packer = new MsgPacker();
			packer.addInt(MsgProtocol.Error);
			MsgUtils.sendMsg(ctx, packer);
		}
			
	}

}
