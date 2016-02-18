package jg.rpg.utils;

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.UUID;

import jg.rpg.config.GameConfig;

public class CommUtils {

	public static void log(String mes){
		System.out.println(mes);
	}
	
	public static void logWarring(String mes){
		System.out.println(mes);
	}
	
	public static void logError(String mes){
		System.out.println(mes);
	}
	
	//TODO
	public static String md5Encrypt(String pwd) throws NoSuchAlgorithmException, UnsupportedEncodingException {
		  MessageDigest md = MessageDigest.getInstance("MD5");
          return bytesToHex(md.digest(pwd.getBytes(GameConfig.DefEncoding)));
	}
	private static String bytesToHex(byte[] bytes) {
		StringBuffer md5str = new StringBuffer();
		//把数组每一字节换成16进制连成md5字符串
		int digital;
		for (int i = 0; i < bytes.length; i++) {
			 digital = bytes[i];
			if(digital < 0) {
				digital += 256;
			}
			if(digital < 16){
				md5str.append("0");
			}
			md5str.append(Integer.toHexString(digital));
		}
		return md5str.toString();
	}
	public static String generateSessionKey() {
		return UUID.randomUUID().toString().replaceAll("-", "");
	}
}
