using UnityEngine;
using System.Collections;

public class Player{
    private int id;
    private RoleItem role;
    private string username;
    private string phoneNum;
    private int level;
    private int fc;
    private int exp;
    private int diamondCount;
    private int goldCount;
    private int toughen;
    private int vit;
    private int hp;
    private int damage;
    private int vip;
    private int totalToughen = 50;
    private int totalVit = 100;

    public int CurNeedExp{
       
        get
        {
            int total = 100;
            for (int i = 0; i < Level; i++)
            {
                total += 100 * (i - 1);
            }
            return total;
        }
    }

    public int TotalToughen
    {
        get { return totalToughen; }
        set { totalToughen = value; }
    }
    

    public int TotalVit
    {
        get { return totalVit; }
        set { totalVit = value; }
    }


    public int Vip
    {
        get { return vip; }
        set { vip = value; }
    }
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public string Username
    {
        get { return username; }
        set { username = value; }
    }
    

    public string PhoneNum
    {
        get { return phoneNum; }
        set { phoneNum = value; }
    }
    

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    

    public int Fc
    {
        get { return fc; }
        set { fc = value; }
    }

    public int Exp
    {
        get { return exp; }
        set { exp = value; }
    }

    public int DiamondCount
    {
        get { return diamondCount; }
        set { diamondCount = value; }
    }

    public int GoldCount
    {
        get { return goldCount; }
        set { goldCount = value; }
    }

    public int Vit
    {
        get { return vit; }
        set { vit = value; }
    }

    public int Toughen
    {
        get { return toughen; }
        set { toughen = value; }
    }

    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
   

    public RoleItem Role
    {
        get { return role; }
        set { role = value; }
    }


}
