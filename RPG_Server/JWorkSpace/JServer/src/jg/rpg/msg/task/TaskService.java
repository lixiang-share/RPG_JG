package jg.rpg.msg.task;

import java.io.IOException;
import java.sql.SQLException;
import java.util.List;

import org.apache.log4j.Logger;

import jg.rpg.common.anotation.HandlerMsg;
import jg.rpg.common.protocol.MsgProtocol;
import jg.rpg.entity.MsgPacker;
import jg.rpg.entity.MsgUnPacker;
import jg.rpg.entity.Session;
import jg.rpg.entity.msgEntity.Player;
import jg.rpg.entity.msgEntity.Task;
import jg.rpg.msg.player.PlayerService;
import jg.rpg.utils.MsgUtils;

public class TaskService {
	private static Logger logger = Logger.getLogger(PlayerService.class);
	
	@HandlerMsg(msgType = MsgProtocol.Get_TaskList)
	public void getTaskList(Session session , MsgUnPacker unpacker){
		Player player = session.getPlayer();
		if(player == null){
			MsgUtils.SendErroInfo(session.getCtx(), "请重新登录");
			return;
		}
		List<Task> tasks = player.getTasks();
		try {
			MsgPacker packer = MsgUtils.getSuccessPacker();
			if(tasks == null || tasks.isEmpty()){
				packer.addInt(0);
			}else{
				packer.addInt(tasks.size());
				for(Task t : tasks){
					logger.debug(t);
					packer.addInt(t.getId())
						.addInt(t.getTaskId())
						.addInt(t.getOwnerId())
						.addString(t.getType())
						.addInt(t.getStatus())
						.addInt(t.getGoldCount())
						.addInt(t.getDiamondCount())
						.addInt(t.getCurStage())
						.addInt(t.getTotalStage());
				}
			}
			MsgUtils.sendMsg(session.getCtx(), packer);
		} catch (IOException e) {
			MsgUtils.SendErroInfo(session.getCtx(), "服务器错误");
		}
	}
	
	
	@HandlerMsg(msgType = MsgProtocol.AcceptTask)
	public void acceptTask(Session session , MsgUnPacker unpacker){
		Player player = session.getPlayer();
		try {
			int taskID = unpacker.popInt();
			logger.debug(taskID);
			player.acceptTask(taskID);
			MsgPacker msg = MsgUtils.getSuccessPacker();
			MsgUtils.sendMsg(session.getCtx(), msg);
		} catch (IOException | SQLException e) {
			MsgUtils.SendErroInfo(session.getCtx(), "领取任务失败");
		}
		
		
	}
}
