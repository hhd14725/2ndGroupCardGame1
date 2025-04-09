using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject StartBtn;

    public void TImeStart()
    {

        Time.timeScale = 1.0f;

        StartBtn.SetActive(false);
    }
}
