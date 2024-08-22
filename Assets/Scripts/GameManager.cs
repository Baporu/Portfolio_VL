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

    //[Header("Player")]
    public Player       player;
    public PoolManager  poolManager;

    [Header("Game System")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;


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

    private void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

}
