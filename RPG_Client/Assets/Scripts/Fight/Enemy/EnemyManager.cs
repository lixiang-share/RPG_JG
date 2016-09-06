using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate bool FilterEnemyCondition(EnemyBase enemy);
public class EnemyManager : FightGOBase {

    public List<EnemyBase> enemies;
    public List<PhaseHandle> phaseHandles;

    private List<EnemyBase> dieEnemies = new List<EnemyBase>();
    public static EnemyManager Instance;
    public void OnEnable()
    {
        if (Instance != null) { Destroy(Instance.gameObject); Instance = null; }
        Instance = this;
    }

    public void Update()
    {
        for (int i = 0; i < enemies.Count ; i++)
        {
            if (enemies[i] == null || enemies[i].Hp <= 0)
            {
                dieEnemies.Add(enemies[i]);
            }
        }
        for (int i = 0; i < dieEnemies.Count; i++)
        {
            enemies.Remove(dieEnemies[i]);
            if(dieEnemies != null)
                dieEnemies[i].Die();
        }
        dieEnemies.Clear();
    }

    public List<EnemyBase> GetEnemies(FilterEnemyCondition filter)
    {
        List<EnemyBase> _enemies = new List<EnemyBase>();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (filter(enemies[i])) _enemies.Add(enemies[i]);
        }
        return _enemies;
    }
    public List<EnemyBase> GetAllEnemy()
    {
        return new List<EnemyBase>(enemies);
    }

    public void AddEnemy(GameObject enemy)
    {
        if(enemy.GetComponent<EnemyBase>() != null)
            enemies.Add(enemy.GetComponent<EnemyBase>());
    }

    public void removeEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<EnemyBase>() != null)
        {
            enemies.Remove(enemy.GetComponent<EnemyBase>());
        }
    }

    public bool isCanMoveNextPhase(Vector3 pos)
    {
        if (phaseHandles != null && phaseHandles.Count != 0)
        {
            for (int i = 0; i < phaseHandles.Count; i++)
            {
                if (phaseHandles[i].isInRange(pos) && enemies.Count > 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool isOver()
    {
        bool isFinalPhase = EnemySpawn.Instance.curPhase == EnemyPhase.FinalPhase;
        bool isNotEnemy = enemies.Count == 0;
        return isFinalPhase && isNotEnemy;
    }
}
