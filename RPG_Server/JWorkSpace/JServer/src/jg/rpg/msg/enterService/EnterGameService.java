package jg.rpg.msg.enterService;

import java.io.IOException;
import java.sql.SQLException;
import java.util.List;

import jg.rpg.common.anotation.HandlerMsg;
import jg.rpg.common.exceptions.PlayerHandlerException;
import jg.rpg.common.manager.SessionMgr;
import jg.rpg.common.manager.DefEntityMgr;
import jg.rpg.common.protocol.MsgProtocol;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.entity.msgEntity.Role;
import jg.rpg.entity.msgEntity.ServerEntity;
import jg.rpg.entity.msgEntity.Task;
import jg.rpg.msg.enterService.controller.EnterGameController;
import jg.rpg.utils.CommUtils;
import jg.rpg.utils.MsgUtils;

import org.apache.log4j.Logger;

public class EnterGameService {
	private Logger logger = Logger.getLogger(getClass());
	private EnterGameController egContoller;
	
	public EnterGameService(){
		egContoller = new EnterGameController();
	}

	@HandlerMsg(msgType = MsgProtocol.Login)
	public void LoginValidate(Session session , MsgUnPacker unpacker){
		logger.debug("LoginValidate");
		MsgPacker packer = new MsgPacker();
		try {
			String username = unpacker.popString();
			String pwd = unpacker.popString();
			Player player = egContoller.getUser(username ,pwd);
			if(player != null){
				Session _session = new Session();
				String sessionKey = CommUtils.generateSessionKey();
				_session.setSessionKey(sessionKey);
				_session.setCtx(session.getCtx());
				_session.setPlayer(player);
				SessionMgr.getInstance().addPlayer(sessionKey, _session);
				
				packer.addInt(MsgProtocol.Success);
				packer.addString(sessionKey);
				egContoller.packPlayer(packer, player);
			}else{
				packer.addInt(MsgProtocol.Error);
				packer.addString("用户名与密码不匹配");
			}
			MsgUtils.sendMsg(session.getCtx(), packer);
			unpacker.close();
			
		} catch (Exception e) {
			logger.warn("handle user login error : "+e.getMessage());
		}
	}
	
	
	@HandlerMsg(msgType = MsgProtocol.Get_ServerList)
	public void GetServerList(Session session , MsgUnPacker unpacker){
		List<ServerEntity> servers = null;
		try {
			servers = egContoller.getServerList();
		} catch (SQLException e) {
			logger.error("read server List exception : " + e.getMessage());
		}
		if(servers == null || servers.isEmpty()){
			logger.warn("read server List empty : " + servers);
		}
		MsgPacker packer = new MsgPacker();
		try {
			packer.addInt(MsgProtocol.Success)
				.addInt(servers.size());
			for(ServerEntity server : servers){
				packer.addInt(server.getId())
					.addString(server.getName())
					.addString(server.getIp())
					.addInt(server.getCount());
			}
			MsgUtils.sendMsg(session.getCtx(), packer);
			unpacker.close();
		} catch (IOException e) {
			logger.error("send msg error : " + e.getMessage());
		}
	}
	
	@HandlerMsg(msgType = MsgProtocol.Register)
	public void registerPlayer(Session session ,MsgUnPacker unpacker ){
		logger.debug("registerPlayer");
		Player player = new Player();
		try {
			player.setUsername(unpacker.popString());
			player.setPwd(unpacker.popString());
			if(unpacker.hasNext())
				player.setPhoneNum(unpacker.popString());
			if(egContoller.registerPlayer(player) != null){
				egContoller.initPlayer(player.getId());
				MsgPacker packer = new MsgPacker();
				packer.addInt(MsgProtocol.Success);
				MsgUtils.sendMsg(session.getCtx(), packer);
			}else{
				throw new PlayerHandlerException("inset to tb_user error : "+player);
			}
		} catch (Exception e) {
			logger.warn("registerPlayer get data error : "+e.getMessage());
			MsgUtils.SendErroInfo(session.getCtx(), "信息输入错误,用户名已存在");
		
		}
	}
	
	@HandlerMsg(msgType = MsgProtocol.Query_Status)
	public void queryStatus(Session session , MsgUnPacker unpacker){
		MsgPacker packer = new MsgPacker();
		try {
			packer.setMsgType(MsgProtocol.Success);
			egContoller.packPlayer(packer, session.getPlayer());
			MsgUtils.sendMsg(session.getCtx(), packer);
		} catch (IOException e) {
			logger.debug("server error : "+e.getMessage());
		}
		
	}
	
	
	@HandlerMsg(msgType = MsgProtocol.PreSelectHero)
	public void preSelectHero(Session session , MsgUnPacker unpacker){
		logger.debug("preSelectHero");
		int playerID = session.getPlayer().getId();
		try {
			List<Role> roles = egContoller.getRolesByPlayerID(playerID);
			MsgPacker packer = MsgUtils.getSuccessPacker();
			packer.addString("公告测试数据\n具体数据形式未确定");
			if(roles == null || roles.isEmpty()){
				packer.addInt(0);
			}else{
				packer.addInt(roles.size());
				for(Role r : roles){
					packer.addInt(r.getId())
						.addInt(r.getOwnerId())
						.addString(r.getRole_id())
						.addString(r.getName())
						.addInt(r.getLevel())
						.addInt(r.getGender());
				}
			}
			MsgUtils.sendMsg(session.getCtx(), packer);
			
		} catch (Exception e) {
			MsgUtils.SendErroInfo(session.getCtx(), "获取角色信息失败");
			logger.warn(e.getMessage());
		}
	}
	
	@HandlerMsg(msgType = MsgProtocol.EnterGame)
	public void enterGame(Session session , MsgUnPacker unpacker){
		logger.debug("enterGame");
		Player player = session.getPlayer();
		int playerID = player.getId();
		try {
			int serverID = unpacker.popInt();
			ServerEntity server = egContoller.getServerByID(serverID);
			if(server != null){
				player.setServer(server);
			}else{
				MsgUtils.SendErroInfo(session.getCtx(), "请正确选择服务器");
				unpacker.close();
				return;
			}
		} catch (IOException | SQLException e) {
			MsgUtils.SendErroInfo(session.getCtx(), "请正确选择服务器");
			logger.warn(e.getMessage());
		}
		
		Role role = new Role();
		try {
			role.setRole_id(unpacker.popString());
			role.setName(unpacker.popString());
			Role _role =egContoller.getRoleByPlayerIDAndRole_ID(playerID , role.getRole_id());
			if(_role != null){
				_role.setName(role.getName());
				_role.updateToDB();
				List<Task> tasks = egContoller.getTaskListByRoleID(player.getId());
				player.setRole(_role);
				player.setTasks(tasks);
			}else{
				MsgUtils.SendErroInfo(session.getCtx(), "请正确选择角色");
				return;
			}
		} catch (IOException | SQLException e) {
			logger.warn(e.getMessage());
			MsgUtils.SendErroInfo(session.getCtx(), "请正确选择角色");
		}	
		
		try {
			MsgPacker packer = MsgUtils.getSuccessPacker();
			MsgUtils.sendMsg(session.getCtx(), packer);
		} catch (IOException e) {
			MsgUtils.SendErroInfo(session.getCtx(), "服务器错误");
			logger.warn(e.getMessage());
		}	
	}
	
	
	
}
