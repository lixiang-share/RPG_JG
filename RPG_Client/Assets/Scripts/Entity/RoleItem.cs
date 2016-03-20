using UnityEngine;
using System.Collections;

public class RoleItem
{
    public const int Woman = 2;
    public const int Man = 1;
    private int id;
    private int ownerId;
    private string role_id;
    private string name;
    private int level;
    private string realGender;

    public string Role_id
    {
        get { return role_id; }
        set { role_id = value; }
    }
   
    
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    

    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    private int gender;

    public int Gender
    {
        get { return gender; }
        set { 
            gender = value;
            switch (value)
            {
                case 0:
                    RealGender = "男";
                    break;
                case 1:
                    RealGender = "女";
                    break;
                default:
                    realGender = "?";
                    break;
            }
        }
    }
    

    public string RealGender
    {
        get { return realGender; }
        set { realGender = value; }
    }
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    

    public int OwnerId
    {
        get { return ownerId; }
        set { ownerId = value; }
    }


}
