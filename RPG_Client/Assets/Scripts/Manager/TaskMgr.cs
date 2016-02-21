using UnityEngine;
using System.Collections;
using System;
using System.IO;
using CsvHelper;
using System.Collections.Generic;

public class TaskMgr : MonoBehaviour {

    public static TaskMgr Instance;
    private static string TaskListPath;
    private IDictionary<int , TaskEntity> taskDict;
    private char defSperator = '|';

    void Awake()
    {
        Instance = this;
        taskDict = new Dictionary<int, TaskEntity>();
        TaskListPath = AppConst.TaskListPath;
    }
    void Start()
    {
        Init();
    }


    public void Init()
    {
        LoadingTask();
    }

    private void LoadingTask()
    {
        using (var reader = new CsvReader(new StreamReader(TaskListPath)))
        {
            while (reader.Read())
            {
                int id = reader.GetField<int>("TaskID");
                TaskEntity task = new TaskEntity();
                task.Id = id;
                task.Name = reader.GetField("Name");
                task.IconName = reader.GetField("Icon");
                task.Desc = reader.GetField("Desc");
                List<string> talks = new List<string>();
                talks.AddRange(reader.GetField<string>("TalkNPC").Split(defSperator));
                task.TalkNPC = talks;
                task.Npc_id = reader.GetField<int>("NPC_ID");
                task.Fb_id = reader.GetField<int>("FB_ID");
                task.Extra01 = reader.GetField("Extra01");
                task.Extra02 = reader.GetField("Extra02");
                task.Extra03 = reader.GetField("Extra03");


                taskDict.Add(id, task);
            }
        }
    }

    public TaskEntity ComposeTask(TaskEntity task)
    {
        TaskEntity _task = GetTaskByID(task.TaskId);
        _task.Status = task.Status;
        _task.GoldCount = task.GoldCount;
        _task.DiamondCount = task.DiamondCount;
        _task.TaskId = task.TaskId;
        _task.CurStage = task.CurStage;
        _task.TotalStage = task.TotalStage;
        _task.RoleId = task.RoleId;
        _task.Type = task.Type;
        return new TaskEntity(_task);
    }


    public TaskEntity GetTaskByID(int id)
    {
        if (taskDict.ContainsKey(id))
            return taskDict[id];
        else
            return null;
    }

}
