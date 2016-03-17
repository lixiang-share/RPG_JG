package jg.rpg.dao.db;

import java.sql.Connection;
import java.util.List;

import com.mchange.v2.c3p0.ComboPooledDataSource;

import jg.rpg.common.exceptions.InitException;
import jg.rpg.config.ConfigMgr;
import jg.rpg.entity.DBEntityInfo;

public class DBMgr {

	private ComboPooledDataSource dataSource;
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
		dataSource = new ComboPooledDataSource();
	}
	
	public void close(){
		dataSource.close();
	}
	
	public ComboPooledDataSource getDataSource() {
		return dataSource;
	}
	
	
}
