using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum PlayerState{Move,Idle,Fight}

public class PlayerFightCtrl : FightGOBase{
	public static PlayerFightCtrl Instance;
	
	
	public float speed = 10;
	public float minResponseVal = 0.005f;
	public bool isAbleMove = true;
	public PlayerState curState = PlayerState.Idle;
    public PlayerAnimatorMgr animMgr;
    public GameObject bloodPos;
    private SkillManager skillMgr;
    private SimpleMoveCtrl moveCtrl;
    private EnemyManager enemyMgr;
    public int hp = 50;
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if (value < 0)
            {
                animMgr.Reset();
                isAbleMove = false;
                hp = 0;
            }
            else
            {
                hp = value;
            }
        }
    }
    public bool isDie
    {
        get
        {
            return Hp <= 0;
        }
    }

    public EnemyManager EnemyMgr
    {
        get {
            if (enemyMgr == null) enemyMgr = EnemyManager.Instance;
            return enemyMgr; 
        }
    }

    public PlayerAnimatorMgr AnimMgr
    {
        get {
            if (animMgr == null) 
                animMgr = GetComponent<PlayerAnimatorMgr>();
            return animMgr; 
        }
    }

    public SimpleMoveCtrl MoveCtrl
    {
        get {
            if (moveCtrl == null) moveCtrl = GetComponent<SimpleMoveCtrl>();
            return moveCtrl; 
        }
    }

    public SkillManager SkillMgr
    {
        get {
            if (skillMgr == null) skillMgr = GetComponentInChildren<SkillManager>();
            return skillMgr; 
        }
    }
    void OnEnable () {
		Instance = this;
	}

    public void GetDamage(AttackItem attack)
    {
        this.Hp = this.Hp - (int)attack.Damage;
        PlaySound(AudioManager.Hurt);
        ReleaseREffect("BloodSplatEffect", bloodPos);
    }

    void Update() {
        if (isDie) return;
        if (isAbleMove &&(curState == PlayerState.Idle || curState == PlayerState.Move))
			PlayerMove();
	}
	
	public void PlayerMove(){
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
        if((Mathf.Abs(v) < minResponseVal && Mathf.Abs(h) < minResponseVal))
        {
            MoveCtrl.ResetState();
            AnimMgr.Reset();
            curState = PlayerState.Idle;
        }
        else {
            curState = PlayerState.Move;
            MoveCtrl.Move(h * speed, v * speed);
            AnimMgr.PlayRun();
        }
    }
    public bool isAbleFight()
    {
        return curState == PlayerState.Idle ;
    }
    public void Attack(int skillID, bool enable = true)
    {
        if(enable)
            SkillMgr.ReleaseSkill(skillID ,
                () => { curState = PlayerState.Fight; isAbleMove = false; },
                () => { 
                    curState = PlayerState.Idle; isAbleMove = true;
                    AnimMgr.Reset();
                    GameTools.LogError("ReleaseSkill Finish!!!");
                }
            );
    }

    public void CalculateDamage(AttackItem attack)
    {
        List<EnemyBase> enemies = EnemyMgr.GetEnemies((enemy) => {
            bool isInRange = Vector3.Distance(transform.position, enemy.transform.position) < attack.Range;
            bool isDir = false;
            if (attack.AttackDir == AttackDir.Around)
            {
                isDir = true;
            }
            else
            {
                Vector3 enemyRelativePos = transform.InverseTransformVector(enemy.transform.position);

                Vector3 meToEnemyVector = enemy.transform.position - transform.position;
                float factor =  Vector3.Dot(meToEnemyVector,transform.forward);
                if ((attack.AttackDir == AttackDir.Forward && factor > 0) ||
                    (attack.AttackDir == AttackDir.Back && factor < 0))
                {
                    isDir = true;
                }
                else{
                    isDir = false;
                }
                    
            }
            return isDir && isInRange;
        });
        if (enemies == null || enemies.Count == 0) return;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetDamage(attack);
        }
    }
}
