using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public bool     isLive;
    public float    gameTime;
    public float    maxGameTime = 2 * 10f;

    [Header("Player Info")]
    public int      playerId;
    public float    health;
    public float    maxHealth = 100f;

    public int      level;
    public int      kill;
    public int      exp;
    public int[]    nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 550 };

    [Header("Game Object")]
    public Player       player;
    public PoolManager  poolManager;
    public LevelUp      uiLevelUP;
    public Result       uiResult;
    public Transform    uiJoy;
    public GameObject   enemyCleaner;


    private void Awake()
    {
        /*
        // Singleton 패턴 구현이었는데 재시작 구조 상 에러가 났고,
        // 동적 연결을 하자니 굳이 이 게임에 필요하지 않을 것 같아 주석 처리함
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
        */

        instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        uiLevelUP.Select(playerId % 2);
        Resume();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.PlayBgm(true);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        // 게임 클리어 연출로 몹을 잡았을 때 경험치가 나오지 않게 하기 위함
        if (!isLive)
            return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            uiLevelUP.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }

}
