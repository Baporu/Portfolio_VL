using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    // Realtime이 붙지 않으면 Timescale이 0일 때 계속 알림이 남게 됨
    private WaitForSecondsRealtime wait;

    public GameObject[] lockedCharacters;
    public GameObject[] unlockedCharacters;
    public GameObject   uiNotice;

    public enum Achieve { UnlockPotato, UnlockBean }
    public Achieve[] achieves;


    private void Awake()
    {
        achieves    = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait        = new WaitForSecondsRealtime(5f);

        if (!PlayerPrefs.HasKey("GameStart"))
        {
            Init();
        }
    }

    public void Init()
    {
        PlayerPrefs.SetInt("GameStart", 1);

        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    private void Start()
    {
        UnlockCharacter();
    }

    public void UnlockCharacter()
    {
        for (int index = 0; index < lockedCharacters.Length; index++)
        {
            string  achieveName = achieves[index].ToString();
            bool    isUnlocked  = PlayerPrefs.GetInt(achieveName) == 1;

            lockedCharacters[index].SetActive(!isUnlocked);
            unlockedCharacters[index].SetActive(isUnlocked);
        }
    }

    private void LateUpdate()
    {
        foreach (Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    private void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        switch (achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.Instance.kill >= 10;

                break;

            case Achieve.UnlockBean:
                isAchieve = GameManager.Instance.gameTime == GameManager.Instance.maxGameTime;

                break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achieve;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        // new WaitForSeconds(5)로 하면 실행마다 새로 만들기 때문에
        // 변수로 미리 저장하고 사용해 최적화를 하는 것
        yield return wait;

        uiNotice.SetActive(false);
    }
}
