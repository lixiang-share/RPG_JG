package jg.rpg;
import java.beans.PropertyVetoException;
import java.sql.SQLException;
import java.util.List;

import org.apache.log4j.Logger;

import com.mchange.v2.c3p0.ComboPooledDataSource;

import jg.rpg.dao.db.DBHelper;
import jg.rpg.dao.db.DBMgr;
import jg.rpg.dao.db.RSHHelper;
import jg.rpg.entity.Cat;
import jg.rpg.exceptions.InitException;
import jg.rpg.msg.MsgMgr;
import jg.rpg.net.NetworkMgr;
import jg.rpg.utils.config.ConfigMgr;

public class LaunchServer {
	
	public static void main(String[] args) throws PropertyVetoException, SQLException {	
			try {
				ConfigMgr.getInstance().init();
				DBMgr.getInstance().init();
				MsgMgr.getInstance().init();
				NetworkMgr.getInstance().init(null);
			} catch (InitException e) {
				e.printStackTrace();
			} catch (Exception e) {
				e.printStackTrace();
			}

			
	}
}
