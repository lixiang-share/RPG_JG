using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CsvHelper;
using System.IO;

public class SkillMgr : MonoBehaviour {

    public static SkillMgr Instance;
    private static string SkillListPath;
    private IDictionary<int, SkillItem> skillDict;
    private char defSperator = '|';

    void Awake()
    {
        Instance = this;
        skillDict = new Dictionary<int, SkillItem>();
        SkillListPath = AppConst.SkillListPath;
    }

    public void Init()
    {
        LoadingSkill();
    }

    private void LoadingSkill()
    {
        using (var reader = new CsvReader(new StreamReader(SkillListPath)))
        {
            while (reader.Read())
            {
                int id = reader.GetField<int>("SkillID");
                SkillItem skill = new SkillItem();
                skill.Id = id;
                skill.Name = reader.GetField("Name");
                skill.Icon = reader.GetField("Icon");
                skill.Desc = reader.GetField("Desc");
                skillDict.Add(id, skill);
            }
        }
    }

    public SkillItem ComposeSkill(SkillItem skill)
    {
        SkillItem _skill = GetTaskByID(skill.SkillID);
        if (_skill != null)
        {
            skill.Name = _skill.Name;
            skill.Icon = _skill.Icon;
            skill.Desc = _skill.Desc;
        }
        return skill;
    }


    public SkillItem GetTaskByID(int id)
    {
        if (skillDict.ContainsKey(id))
            return skillDict[id];
        else
            return null;
    }
}
