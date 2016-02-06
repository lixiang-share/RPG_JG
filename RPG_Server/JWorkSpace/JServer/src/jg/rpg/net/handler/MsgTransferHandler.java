package jg.rpg.net.handler;

import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.msg.MsgMgr;

import org.apache.log4j.Logger;

public class MsgTransferHandler extends SimpleChannelInboundHandler<MsgUnPacker>{
	private Logger logger = Logger.getLogger(getClass());
	@Override
	protected void channelRead0(ChannelHandlerContext ctx, MsgUnPacker unpacker)
			throws Exception {
		logger.debug("MsgTransferHandler");
		Session session = new Session();
		session.setCtx(ctx);
		MsgMgr.getInstance().handleMsg(session, unpacker);
	}

}
