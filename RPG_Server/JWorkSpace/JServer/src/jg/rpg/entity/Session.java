package jg.rpg.entity;

import io.netty.channel.ChannelHandlerContext;

public class Session {

	private ChannelHandlerContext ctx;

	public ChannelHandlerContext getCtx() {
		return ctx;
	}

	public void setCtx(ChannelHandlerContext ctx) {
		this.ctx = ctx;
	}
	
	
}
