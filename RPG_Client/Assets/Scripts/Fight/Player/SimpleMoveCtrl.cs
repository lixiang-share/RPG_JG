using UnityEngine;
using System.Collections;


public enum MoveState{Moving , MovingTarget , Idle}
public delegate bool ArriveCondition(Vector3 target);
public class SimpleMoveCtrl : MonoBehaviour
{

    public float StoppingDist = 4f;
    public Vector3 target;
    public float speed = 8;
    public MoveState curState;
    private Vector3 velocity = Vector3.zero;
    private bool isMoving;
    private bool isAbleMove = true;
    private NavMeshAgent navMeshAgent;
    private ArriveCondition Condition;
    private DefAction OnArrive;
    void Awake()
    {
        curState = MoveState.Idle;
        if(GetComponent<NavMeshAgent>() != null)
            navMeshAgent = GetComponent<NavMeshAgent>();
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
        else
        {
            PlayerMainCityCtrl.Instance.animMgr.PlayRun();
        }
    }

	private void Moving(){
		Vector3 nexPos = velocity * Time.deltaTime + transform.position;
		if (isAbleMove && isMoving 
            && Enclosure.Instance.isInside(nexPos) && velocity.magnitude > 0.01
            &&(EnemyManager.Instance == null || EnemyManager.Instance.isCanMoveNextPhase(nexPos)))
		{
			transform.position = nexPos;
			transform.rotation = Quaternion.LookRotation(velocity);
		}
		else
		{
			ResetState();
		}
	}
	
    public void ResetState()
    {
        isMoving = false;
        isAbleMove = false;
        curState = MoveState.Idle;
        velocity = Vector3.zero;
        if(navMeshAgent != null)
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
        if(navMeshAgent == null)
        {
            GameTools.LogError("Navigation in null!!!");
            return;
        }

        navMeshAgent.enabled = true;
        isAbleMove = true;
        this.target = target;
        this.OnArrive = OnArrive;
        this.Condition = condition;
        navMeshAgent.destination = target;
        curState = MoveState.MovingTarget;
        navMeshAgent.speed = speed;
    }

    public void MoveTarget(GameObject go, DefAction OnArrive, ArriveCondition condition = null)
    {
        MoveToTarget(go.transform.position, OnArrive, condition);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Slope")
        {
            SlopeHandler slopeHandler = other.gameObject.GetComponent<SlopeHandler>();
            Vector3 curPos = transform.position;
            float curY = slopeHandler.GetYOnSlope(curPos);
            transform.position = new Vector3(curPos.x, curY, curPos.z);
        }

    }

    void OnTriggerExit(Collider other)
    {
       // GameTools.LogError("=== Exit ==");
        if (other.tag == "Slope")
        {
           SlopeHandler slopeHandler = other.gameObject.GetComponent<SlopeHandler>();
           Vector3 curPos = transform.position;
           float curY = slopeHandler.GetYLeaveSlop(curPos);
           transform.position = new Vector3(curPos.x, curY, curPos.z);
        }
    }

}
