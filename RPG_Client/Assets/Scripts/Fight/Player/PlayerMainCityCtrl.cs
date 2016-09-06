using UnityEngine;
using System.Collections;

public class PlayerMainCityCtrl : MonoBehaviour {
    
    public float minResponseVal = 0.005f;
    public bool isAbleMove = true;
    public static PlayerMainCityCtrl Instance;
    public float speed = 10;
    private SimpleMoveCtrl moveCtrl;
    public PlayerAnimatorMgr animMgr;
    public GameObject go;
    void Awake()
    {
        Instance = this;
        moveCtrl = GetComponent<SimpleMoveCtrl>();
        animMgr = GetComponent<PlayerAnimatorMgr>();
        if (moveCtrl == null)
        {
            GameTools.LogError("Player Con not move due to Don't attack SimplaMoveCrlt");
            isAbleMove = false;
        }
    }

    void Start()
    {
        //moveCtrl.MoveTarget(go, () =>
        //{
        //    GameTools.LogError("========>>>>");
        //});
    }
	// Update is called once per frame
	void Update () {
        if (moveCtrl.curState != MoveState.MovingTarget && isAbleMove)
        {
            PlayerMove();
        }
	}

    private void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if ((Mathf.Abs(v) < minResponseVal && Mathf.Abs(h) < minResponseVal) || !isAbleMove){
            moveCtrl.ResetState();
            animMgr.Reset();
        }
        else{
            moveCtrl.Move(-1 * h * speed, -1 * v * speed);
            animMgr.PlayRun();
        }
    }

    public void moveToFight()
    {
        moveToFight(1003);
    }

    public void moveToFight(int npcID)
    {
        Vector3 target = NPCManager.Instance.GetNPC(npcID).transform.position;
        moveCtrl.MoveToTarget(target, () => {
            moveCtrl.ResetState();
            animMgr.Reset();
            animMgr.PlayRun();
            UITools.ShowPanel(UITools.D("FBView"));
        });
    }



    public void DoTask(TaskEntity task)
    {
        switch (task.Status)
        {
            case TaskEntity.NotClaim:
                ClaimTask(task);
                break;
            case TaskEntity.NotComplete:
                NextTask(task);
                break;
            case TaskEntity.Complete:
                ClaimRewards(task);
                break;
            case TaskEntity.Finish:
                Finish(task);
                break;
        }
    }

    public void ClaimTask(TaskEntity task)
    {
        isAbleMove = false;
        GameObject npcGO = NPCManager.Instance.GetNPC(task.Npc_id);
        moveCtrl.MoveTarget(npcGO, () => {
            GameTools.LogError("========>>>>");
            TalkToNPC.Instance.Talk(task.TalkNPC, () => {
                isAbleMove = true;
                GameTools.LogError("=== Talk end =====>"+task.TaskId);
                AcceptTask(task.Id);
            });
        });
    }

    public void AcceptTask(int taskID)
    {
        MsgPacker packer = new MsgPacker();
        packer.SetType(MsgProtocol.AcceptTask);
        packer.add<int>(taskID);
        NetworkMgr.instance.Send(packer, (data) => {
            UITools.ShowMsg("任务领取成功");
        });
    }

    public void NextTask(TaskEntity task)
    {
        moveToFight();
    }
    public void ClaimRewards(TaskEntity task)
    {

    }
    public void Finish(TaskEntity task)
    {

    }
}
