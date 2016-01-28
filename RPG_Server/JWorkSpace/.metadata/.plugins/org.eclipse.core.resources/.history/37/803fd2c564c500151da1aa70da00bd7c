package jg.rpg.dao.db;

import java.sql.Connection;
import java.util.List;

import com.mchange.v2.c3p0.ComboPooledDataSource;

import jg.rpg.entity.DBEntityInfo;
import jg.rpg.exceptions.InitException;
import jg.rpg.utils.config.ConfigMgr;

public class DBMgr {

	private Connection strogeDBConn;
	private static DBMgr inst;
	public static DBMgr getInstance(){
		if(inst == null){
			inst = new DBMgr();
		}
		return inst;
	}
	private DBMgr(){
		
	}

	public void init() throws InitException {
		//DBEntityInfo info = ConfigMgr.getInstance().getStrogeDbInfo();
		//strogeDBConn = DBHelper.getConnection(info.getDriver(), info.getUrl(), info.getUrl(),info.getPwd());
		//ComboPooledDataSource cpds = new ComboPooledDataSource();
		//cpds.setMaxStatements( 180 );
	}
	
	public <T> List<T> GetAll(String sql) {
		return null;
	}
	
	
	
}
