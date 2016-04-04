using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CsvHelper;
using System.IO;

public class EquipMgr : MonoBehaviour {

    public static EquipMgr Instance;
    private static string EquipListPath;
    private Dictionary<int, EquipItem> equipDict;
    private char defSperator = '|';
    void Awake()
    {
        Instance = this;
        equipDict = new Dictionary<int, EquipItem>();
        EquipListPath = AppConst.EquipListPath;
    }
	// Use this for initialization
    void Start()
    {
        Init();
    }


    public void Init()
    {
        LoadingEquipList();
    }

    private void LoadingEquipList()
    {
        using (var reader = new CsvReader(new StreamReader(EquipListPath)))
        {
            while (reader.Read())
            {
                int id = reader.GetField<int>("id");
                EquipItem equip = new EquipItem();
                equip.EquipId = id;
                equip.Name = reader.GetField("name");
                equip.Icon = reader.GetField("icon");
                equip.Desc = reader.GetField("desc");
                equipDict.Add(id, equip);
            }
        }
    }

    public EquipItem ComposeTask(EquipItem equip)
    {
        EquipItem item = GetTaskByID(equip.EquipId);
        if (item != null)
        {
            equip.Name = item.Name;
            equip.Icon = item.Icon;
            equip.Desc = item.Desc;
        }
        return equip;
    }


    public EquipItem GetTaskByID(int id)
    {
        if (equipDict.ContainsKey(id))
            return equipDict[id];
        return null;
    }
	
}
