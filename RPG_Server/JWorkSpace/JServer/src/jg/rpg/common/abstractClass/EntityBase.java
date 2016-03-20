package jg.rpg.common.abstractClass;

import java.sql.SQLException;

import jg.rpg.common.exceptions.EntityHandlerException;
import jg.rpg.common.exceptions.PlayerHandlerException;

public abstract class EntityBase <T>{

	public  boolean isNeedDelete(){
		return false;
	}
	
	public  boolean isExistInDB() throws SQLException, EntityHandlerException{
		return false;
	}
	
	public T insertToDB() throws SQLException, EntityHandlerException {
		return null;
	}
	
	public  int updateToDB()throws SQLException, EntityHandlerException{
		return 0;
	}
	
	public  int deleteFromDB() throws SQLException {
		return 0;
	}
	
	public void SaveInfoToDB() throws SQLException, EntityHandlerException{
		if(isExistInDB()){
			if(isNeedDelete()){
				deleteFromDB();
			}else{
				updateToDB();
			}
		}else{
			insertToDB();
		}
	}
}
