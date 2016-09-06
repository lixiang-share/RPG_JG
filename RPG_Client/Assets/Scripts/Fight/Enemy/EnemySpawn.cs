using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyPhase { FirstPhase, SecondPhase , ThirdPhase , FinalPhase}

public class EnemySpawn : FightGOBase {

    public List<GameObject> EnemyPhase01;
    public List<GameObject> EnemyPhase02;
    public List<GameObject> EnemyPhase03;
    public List<GameObject> EnemyPhase04;

    public EnemyPhase curPhase = EnemyPhase.FinalPhase;
    public static EnemySpawn Instance;
	// Use this for initialization
	void Start () {
        Instance = this;
        SpawEnemy(curPhase);
	}

    public void SpawEnemy(EnemyPhase phase)
    {
        curPhase = phase;
        List<GameObject> enemyPhase = null;
        switch (phase)
        {
            case EnemyPhase.FirstPhase:
                enemyPhase = EnemyPhase01;
                break;
            case EnemyPhase.SecondPhase:
                enemyPhase = EnemyPhase02;
                break;
            case EnemyPhase.ThirdPhase:
                enemyPhase = EnemyPhase03;
                break;
            case EnemyPhase.FinalPhase:
                enemyPhase = EnemyPhase04;
                break;
        }
        GameObject enemyPrefab = ResourceManager.Instance.LoadPrefab("Fight/Enemy/FlowerMonster-Blue") as GameObject;
        if (enemyPhase != null && enemyPhase.Count != 0)
        {
            foreach(GameObject go in enemyPhase){
                GameObject enemy = Instantiate(enemyPrefab) as GameObject;
                enemy.transform.parent = go.transform;
                enemy.transform.localPosition = Vector3.zero;
                EnemyManager.Instance.AddEnemy(enemy);
            }
        }
    }
}
