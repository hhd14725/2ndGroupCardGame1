using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; //½Ì±ÛÅæ
    public GameManager gameManager;
    public float time = 30.0f;
    public Text Timetxt;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
     // Start is called before the first frame update
     void Start()
    {
        Time.timeScale = 0.0f; //StartButtonÀÌ °ªÀ» 1.0f·Î ¹Ù²ãÁØ´Ù
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.0f)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }


            if (time < 0)
            {
                time = 0;

                GameManager.instance.GameOver();
            }

            Timetxt.text = time.ToString("N2");
        }
    }
}
