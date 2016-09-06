using UnityEngine;
using System.Collections;

public class EnemyCtrl : FightGOBase {
    public float moveSpeed = 4;
    public float attackRange = 1.5f;
    public float attackInterval = 5f;
    public Animation animation;
    public int Damage = 1;
    public float MaxDist = 10;
    private PlayerFightCtrl player;
	// Update is called once per frame
    public float lastAttackTime = 0f;
    void Start()
    {
        player = PlayerFightCtrl.Instance;
    }

	void Update () {
        if (player.isDie) return;

        towardsPlayer();
        if (isCanAttack())
        {
            Attack();
        }
        else
        {
            MoveToPlayer();
        }

	}

    private bool isCanAttack()
    {
        bool isInAttackRange = Vector3.Distance(transform.position, player.transform.position) <= attackRange;
        return isInAttackRange;
    }

    public void towardsPlayer()
    {
        transform.LookAt(player.transform);
    }

    public void Attack()
    {

        if (Time.time - lastAttackTime < attackInterval)
        {
          //  animation.Play("idle");
            return;
        }
        animation.Play("attack01");
        AttackItem attack = new AttackItem();
        attack.Damage = Damage;
        player.GetDamage(attack);
        lastAttackTime = Time.time;
    }

    public void MoveToPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= MaxDist)
        {
            animation.Play("idle");
            return;
        }
        animation.Play("walk");
        Vector3 nextPos = transform.position  + (player.transform.position - transform.position).normalized 
            * Time.deltaTime * moveSpeed;
        if (Enclosure.Instance.isInside(nextPos))
        {
            transform.position = nextPos;
        }
    }
}
