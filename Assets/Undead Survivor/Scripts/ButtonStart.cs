using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStart : MonoBehaviour
{
    private Button startBtn;


    private void Awake()
    {
        startBtn = GetComponent<Button>();

        startBtn.onClick.AddListener(OnClickStart);
    }

    private void OnClickStart()
    {
        GameManager.Instance.GameStart(0);
    }

    private void OnDestroy()
    {
        startBtn.onClick.RemoveListener(OnClickStart);
    }
}
