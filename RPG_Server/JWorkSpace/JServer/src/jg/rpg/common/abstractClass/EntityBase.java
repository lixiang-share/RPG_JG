package jg.rpg.common.abstractClass;

import java.sql.SQLException;

import jg.rpg.exceptions.PlayerHandlerException;

public abstract class EntityBase <T>{

	public  boolean isNeedDelete(){
		return false;
	}
	
	public  boolean isExistInDB() throws SQLException, PlayerHandlerException{
		return false;
	}
	
	public T insertToDB() throws SQLException, PlayerHandlerException {
		return null;
	}
	
	public  int updateToDB()throws SQLException{
		return 0;
	}
	
	public  int deleteFromDB() throws SQLException {
		return 0;
	}
	
	public void SaveInfoToDB() throws SQLException, PlayerHandlerException{
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
