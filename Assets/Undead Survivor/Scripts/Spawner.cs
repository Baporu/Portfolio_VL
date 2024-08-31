using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float   timer;
    private int     level;

    public Transform[]  spawnPoint;
    public SpawnData[]  spawnData;
    public float        levelTime;


    private void Awake()
    {
        spawnPoint  = GetComponentsInChildren<Transform>();
        levelTime   = GameManager.Instance.maxGameTime / spawnData.Length;
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / levelTime), spawnData.Length - 1);

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
