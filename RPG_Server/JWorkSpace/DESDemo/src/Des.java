import java.security.SecureRandom;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.DESKeySpec;
/**
 * Copyright 2007 GuangZhou Cotel Co. Ltd.
 * All right reserved.    
 * DES加密解密类.     
 * @author <a href="mailto:xiexingxing1121@126.com" mce_href="mailto:xiexingxing1121@126.com">AmigoXie</a>
 * @version 1.0 
 * Creation date: 2007-7-31 - 上午11:59:28
 */
public class Des {
    /** 加密算法,可用 DES,DESede,Blowfish. */
    private final static String ALGORITHM = "DES";
    public static void main(String[] args) throws Exception {
        String md5Password = "中国软件 tttt csdn.netdfgdfgdfgdfgdfgdfg99999";
        String str = Des.encrypt(md5Password);
        System.out.println("str: " + str);
        str = Des.decrypt(str);
        System.out.println("str: " + str);
    }
    
    
    public final static byte[] decrypt(byte[] data) throws Exception {
        return decrypt(hex2byte(data), PASSWORD_CRYPT_KEY.getBytes());
    }
    public final static  byte[]  encrypt(byte[] data) throws Exception  {
        return encrypt(data, PASSWORD_CRYPT_KEY.getBytes());
    }
    
    public static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x12, 0x34, 0x56, 0x78 };
    private static  String PASSWORD_CRYPT_KEY = new String(Keys);
    
    public final static String decrypt(String data) throws Exception {
        return new String(decrypt(hex2byte(data.getBytes()),
                PASSWORD_CRYPT_KEY.getBytes()));
    }
    public final static String encrypt(String data) throws Exception  {
        return byte2hex(encrypt(data.getBytes(), PASSWORD_CRYPT_KEY
                .getBytes()));
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