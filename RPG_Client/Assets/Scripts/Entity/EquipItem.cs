using UnityEngine;
using System.Collections;

public class EquipItem {

	public const string Equip = "Equip" ;
	public const string Drug = "Drug" ;
	public const string Helm = "Helm" ;
	public const string Cloth = "Cloth" ;
	public const string Weapon = "Weapon" ;
	public const string Shoes = "Shoes" ;
	public const string Necklace = "Necklace" ;
	public const string Bracelet = "Bracelet" ;
	public const string Ring = "Ring" ;
	public const string Wing = "Wing" ;

    private int id;
    private int ownerId;
    private int equipId;
    private string type;
    private string equipType;
    private int star;
    private int quality;
    private int damage;
    private int hp;
    private int fc;
    private string effectType;
    private int effectValue;
    private int level;
    private int amount;
    private bool isDress;
    private bool isMan;
    private string name;
    private string desc;
    private string icon;

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
   

    public int EquipId
    {
        get { return equipId; }
        set { equipId = value; }
    }

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public string EquipType
    {
        get { return equipType; }
        set { equipType = value; }
    }
    private int price;

    public int Price
    {
        get { return price; }
        set { price = value; }
    }

    public int Star
    {
        get { return star; }
        set { star = value; }
    }

    public int Quality
    {
        get { return quality; }
        set { quality = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public int Fc
    {
        get { return fc; }
        set { fc = value; }
    }

    public string EffectType
    {
        get { return effectType; }
        set { effectType = value; }
    }

    public int EffectValue
    {
        get { return effectValue; }
        set { effectValue = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }


    public bool IsDress
    {
        get { return isDress; }
        set { isDress = value; }
    }
    public bool IsMan
    {
        get { return isMan; }
        set { isMan = value; }
    }
}
