using UnityEngine;
using System.Collections;

public class SkillItem {

	public const string Man = "Warrior";
	public const string Woman = "FemaleAssassin";
	public const string One = "one";
	public const string Two = "two";
    public const string Three = "three";
    public const string Basic = "basic";
    public const string Base = "Base";
    private int id;
    private int skillID;
    private int ownerID;
    private string roleType;
    private string type;
    private int coldTime;
    private int fc;
    private int level;
    private string icon;
    private string desc;
    private string name;
    private string pos;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    

    public int SkillID
    {
        get { return skillID; }
        set { skillID = value; }
    }

    public int OwnerID
    {
        get { return ownerID; }
        set { ownerID = value; }
    }

    public string RoleType
    {
        get { return roleType; }
        set { roleType = value; }
    }

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public string Pos
    {
        get { return pos; }
        set { pos = value; }
    }

    public int ColdTime
    {
        get { return coldTime; }
        set { coldTime = value; }
    }

    public int Fc
    {
        get { return fc; }
        set { fc = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public string Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    public string Desc
    {
        get { return desc; }
        set { desc = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

}
