package jg.rpg.utils;

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.util.UUID;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.DESKeySpec;

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
	public static String md5Encrypt(String pwd) {
		  MessageDigest md;
		try {
			md = MessageDigest.getInstance("MD5");
			return bytesToHex(md.digest(pwd.getBytes(GameConfig.DefEncoding)));
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		}
		return pwd;
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
	
	private final static String ALGORITHM = "DES";
    public static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x12, 0x34, 0x56, 0x78 };
    private static  String PASSWORD_CRYPT_KEY = new String(Keys);
    
    public final static String decrypt(String data) throws Exception {
        return new String(decrypt(hex2byte(data.getBytes()),
                PASSWORD_CRYPT_KEY.getBytes()));
    }
    public final static String encrypt(String data) throws Exception  {
        return byte2hex(encrypt(data.getBytes(), PASSWORD_CRYPT_KEY.getBytes()));
    }
    
    public final static byte[] decrypt(byte[] data) throws Exception {
        return decrypt(hex2byte(data), PASSWORD_CRYPT_KEY.getBytes());
    }
    public final static  byte[]  encrypt(byte[] data) throws Exception  {
        return encrypt(data, PASSWORD_CRYPT_KEY.getBytes());
    }
    private static byte[] encrypt(byte[] data, byte[] key) throws Exception {
        SecureRandom sr = new SecureRandom();
        DESKeySpec dks = new DESKeySpec(key);
        SecretKeyFactory keyFactory = SecretKeyFactory.getInstance(ALGORITHM);
        SecretKey securekey = keyFactory.generateSecret(dks);
        Cipher cipher = Cipher.getInstance(ALGORITHM);
        cipher.init(Cipher.ENCRYPT_MODE, securekey, sr);
        return cipher.doFinal(data);
    }
    private static byte[] decrypt(byte[] data, byte[] key) throws Exception {
        SecureRandom sr = new SecureRandom();
        DESKeySpec dks = new DESKeySpec(key);
        SecretKeyFactory keyFactory = SecretKeyFactory.getInstance(ALGORITHM);
        SecretKey securekey = keyFactory.generateSecret(dks);
        Cipher cipher = Cipher.getInstance(ALGORITHM);
        cipher.init(Cipher.DECRYPT_MODE, securekey, sr);
        return cipher.doFinal(data);
    }
    public static byte[] hex2byte(byte[] b) {
        if ((b.length % 2) != 0)
            throw new IllegalArgumentException("长度不是偶数");
        byte[] b2 = new byte[b.length / 2];
        for (int n = 0; n < b.length; n += 2) {
            String item = new String(b, n, 2);
            b2[n / 2] = (byte) Integer.parseInt(item, 16);
        }
        return b2;
    }
    public static String byte2hex(byte[] b) {
        String hs = "";
        String stmp = "";
        for (int n = 0; n < b.length; n++) {
            stmp = (java.lang.Integer.toHexString(b[n] & 0XFF));
            if (stmp.length() == 1)
                hs = hs + "0" + stmp;
            else
                hs = hs + stmp;
        }
        return hs.toUpperCase();
    }
}
