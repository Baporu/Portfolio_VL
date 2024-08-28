using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager  instance = null;
    public static GameManager   Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    [Header("Game System")]
    public float    gameTime;
    public float    maxGameTime = 2 * 10f;

    [Header("Player Info")]
    public int      health;
    public int      maxHealth = 100;

    public int      level;
    public int      kill;
    public int      exp;
    public int[]    nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 550 };

    [Header("Game Object")]
    public Player       player;
    public PoolManager  poolManager;


    private void Awake()
    {
        // Singleton 패턴 구현
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }

}
