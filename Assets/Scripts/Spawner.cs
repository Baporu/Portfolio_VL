using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float   timer;
    private int     level;

    public Transform[] spawnPoint;
    public SpawnData[] spawnData;


    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
    {
        GameObject enemy = GameManager.Instance.poolManager.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;

    public int      spriteType;
    public int      health;
    public float    speed;
}
