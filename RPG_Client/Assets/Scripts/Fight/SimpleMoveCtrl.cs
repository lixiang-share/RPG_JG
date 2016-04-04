using UnityEngine;
using System.Collections;
//public enum PlayerState { Idle , Run , Fight}

public enum MoveState{Moving , MovingTarget , Idle}
public delegate bool ArriveCondition(Vector3 target);
public class SimpleMoveCtrl : MonoBehaviour
{

    public string run_name = "run";
    public float StoppingDist = 4f;
   // public GameObject go;
    public Vector3 target;
    public float speed = 8;
    private MoveState curState;
    private Vector3 velocity = Vector3.zero;
    private bool isMoving;
    private bool isAbleMove = true;
    private Animator animatorCtrl;
    private NavMeshAgent navMeshAgent;
    private ArriveCondition Condition;
    private DefAction OnArrive;

    void Awake()
    {
        animatorCtrl = GetComponent<Animator>();
        if (animatorCtrl == null)
        {
            GameTools.LogError("Can not move due to don't attach AnimatorController!!");
            isAbleMove = false;
        }
        curState = MoveState.Idle;
        navMeshAgent = GetComponent<NavMeshAgent>();
        //MoveTarget(go, () => { GameTools.Log("======>>"); }); 
    }

    void Update()
    {
        if (isAbleMove)
        {
            switch (curState)
            {
                case MoveState.Moving:
                    Moving();
                    break;
                case MoveState.MovingTarget:
                    isArriveTarget();
                    break;
                default:
                    break;
            }
        }
        else
        {
            ResetState();
        }
    }

    private void isArriveTarget()
    {
        //if arrive
        bool isArrive = false;
        if (Condition != null)
            isArrive = Condition(target);
        else
            isArrive = Vector3.Distance(transform.position, target) <= StoppingDist;
        if (isArrive)
        {
            if (this.OnArrive != null) this.OnArrive();
            ResetState();
        }
    }

	private void Moving(){
		Vector3 nexPos = velocity * Time.deltaTime + transform.position;
		if (isAbleMove && isMoving && Enclosure.Instance.isInside(nexPos) && velocity.magnitude > 0.01)
		{
			playRun(true);
			transform.position = nexPos;
			transform.rotation = Quaternion.LookRotation(velocity);
		}
		else
		{
			ResetState();
		}
	}
	
	
	
    public void playRun(bool isRun)
    {
        animatorCtrl.SetBool(run_name, isRun);
    }

    public void ResetState()
    {
        playRun(false);
        isMoving = false;
        isAbleMove = false;
        curState = MoveState.Idle;
        velocity = Vector3.zero;
        navMeshAgent.enabled = false;
        target = Vector3.zero;
        this.OnArrive = null;
        this.Condition = null;
    }
    public void Move(Vector3 velocity)
    {
        this.ResetState();
        this.velocity = velocity;
        this.isAbleMove = true;
        this.isMoving = true;
        this.curState = MoveState.Moving;
    }

    public void Move(float x, float z)
    {
        Vector3 v = new Vector3(x, 0, z);
        Move(v);
    }

    public void MoveToTarget(Vector3 target, DefAction OnArrive, ArriveCondition condition = null)
    {
        navMeshAgent.enabled = true;
        isAbleMove = true;
        this.target = target;
        this.OnArrive = OnArrive;
        this.Condition = condition;
        navMeshAgent.destination = target;
        curState = MoveState.MovingTarget;
        navMeshAgent.speed = speed;
        playRun(true);
    }

    public void MoveTarget(GameObject go, DefAction OnArrive, ArriveCondition condition = null)
    {
        MoveToTarget(go.transform.position, OnArrive, condition);
    }
}
