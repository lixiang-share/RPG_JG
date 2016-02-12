package jg.rpg.net.handlers;

import org.apache.log4j.Logger;

import io.netty.buffer.ByteBuf;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.utils.MsgUtils;

public class MsgDeserializerHandler extends SimpleChannelInboundHandler<ByteBuf> {
	private Logger logger = Logger.getLogger(getClass());
	@Override
	protected void channelRead0(ChannelHandlerContext ctx, ByteBuf msg)
			throws Exception {
		logger.debug("MsgDeserializerHandler");
		ctx.fireChannelRead(MsgUtils.DeserializerMsg(msg));
	}

}
