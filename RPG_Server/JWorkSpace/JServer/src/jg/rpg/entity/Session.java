package jg.rpg.entity;

import jg.rpg.entity.msgEntity.Player;
import jg.rpg.utils.config.GameConfig;
import io.netty.channel.ChannelHandlerContext;

public class Session {

	private ChannelHandlerContext ctx;
	private String sessionKey;
	private long generateTime;
	private long vaildTimeInterval;
	private Player player;
	


	public Session(){
		setGenerateTime(System.currentTimeMillis());
		setVaildTimeInterval(GameConfig.VaildTimeInterval);
	}
	
	
	public Player getPlayer() {
		return player;
	}
	public void setPlayer(Player player) {
		this.player = player;
	}
	
	public long getVaildTimeInterval() {
		return vaildTimeInterval;
	}

	public void setVaildTimeInterval(long vaildTimeInterval) {
		this.vaildTimeInterval = vaildTimeInterval;
	}

	
	public void updata(ChannelHandlerContext ctx){
		setGenerateTime(System.currentTimeMillis());
		setCtx(ctx);
	}
	public boolean isVaild(){
		return System.currentTimeMillis() < getGenerateTime() + getVaildTimeInterval();
	}
	
	public long getGenerateTime() {
		return generateTime;
	}

	public void setGenerateTime(long generateTime) {
		this.generateTime = generateTime;
	}

	public String getSessionKey() {
		return sessionKey;
	}

	public void setSessionKey(String sessionKey) {
		this.sessionKey = sessionKey;
	}

	public ChannelHandlerContext getCtx() {
		return ctx;
	}

	public void setCtx(ChannelHandlerContext ctx) {
		this.ctx = ctx;
	}
}
