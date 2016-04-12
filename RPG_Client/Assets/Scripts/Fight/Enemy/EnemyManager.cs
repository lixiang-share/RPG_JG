using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate bool FilterEnemyCondition(EnemyBase enemy);
public class EnemyManager : FightGOBase {

    public List<EnemyBase> enemies;
    public static EnemyManager Instance;
    public void Start()
    {
        if (Instance != null) { Destroy(Instance.gameObject); Instance = null; }
        Instance = this;
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

    public void AddEnemy(EnemyBase enemy)
    {

    }

    public void removeEnemy()
    {

    }
}
