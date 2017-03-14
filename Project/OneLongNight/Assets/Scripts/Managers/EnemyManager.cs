using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    [Header("Enemy Spawns")]
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private int numOfEnemiesToSpawn;
    [SerializeField]
    private Transform[] enemySpawnPoints;
    private List<GameObject> enemies;

	// Use this for initialization
	void Start ()
    {
        SpawnInEnemies();

        StartCoroutine(SetEnemiesToSpawns());
	}

    private void SpawnInEnemies()
    {
        GameObject enemyHolder = new GameObject();
        enemyHolder.name = "Enemy Holder";

        enemies = new List<GameObject>();

        for(int i = 0; i < numOfEnemiesToSpawn; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab) as GameObject;
            enemy.SetActive(false);

            enemy.transform.parent = enemyHolder.transform;

            enemies.Add(enemy);
        }
    }

    private IEnumerator SetEnemiesToSpawns()
    {
        bool spawnIn = true;

        while(spawnIn)
        {
            //Spawn In 
            for(int i = 0; i < enemies.Count; i++)
            {
                if(!enemies[i].activeInHierarchy)
                {
                    enemies[i].GetComponent<StatePatternStandardEnemy>().SetUpEnemy();

                    //Calculate Random Spawn Point
                    int spawnPoint = Random.Range(0, enemySpawnPoints.Length);

                    enemies[i].transform.position = enemySpawnPoints[spawnPoint].position;
                    enemies[i].SetActive(true);
                    break;
                }
            }
            //Calculate Wait Time For Next Enemy
            float waitTime = Random.Range(1.0f, 5.0f);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
