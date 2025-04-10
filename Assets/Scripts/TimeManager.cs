using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEditor;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; //싱글톤
    public GameManager gameManager;
    public float time = 30.0f;
    public Text Timetxt;

    public float passedTime = 0.0f;
    public float shiftInterval = 5f;  // 카드 이동 간격 스테이지4
    private float shiftTimer;         // 카드 이동 타이머 스테이지4

    public float restoreInterval = 20f;   // 카드 복구 간격 (예: 20초)
    private float restoreTimer;           // 복구 타이머


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
        Time.timeScale = 0.0f; //StartButton이 값을 1.0f로 바꿔준다
        shiftTimer = shiftInterval;
        restoreTimer = restoreInterval;
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

        passedTime += Time.deltaTime;
        if (Card.instance.type == 4)
        {
            if (passedTime >= 5.0f)
            {
                passedTime = 0.0f;
                GameManager.instance.Shuffle();
            }
        }
        if (Card.instance.type == 3)
        {
           
            if (time <= 0f)
            {
                time = 0f;
               // RestoreAllCards(); // 전체 초기화
            }
            shiftTimer -= Time.deltaTime;
            // 정해진 시간마다 카드 위치 이동
            if (shiftTimer <= 0f)
            {
                shiftTimer = shiftInterval;
 
                Board.ShiftCardPositions(); // 카드 밀기
            }
            restoreTimer -= Time.deltaTime;
            if (restoreTimer <= 0f)
            {
                RestoreAllCards();
                restoreTimer = restoreInterval;
            }
        }

    }

    public void plusTime() // 시간 증가 및 화면에 시간 증가 표시
    {
        time += 2.0f;
        GameManager.instance.plusText = Instantiate(GameManager.instance.plus, GameManager.instance.canvas.transform);
        Destroy(GameManager.instance.plusText.gameObject, 1f);
    }

    public void minusTime() // 시간 감소 및 화면에 시간 감소 표시
    {
        time -= 2.0f;
        GameManager.instance.minusText = Instantiate(GameManager.instance.minus, GameManager.instance.canvas.transform);
        Destroy(GameManager.instance.minusText.gameObject, 1f);
    }
    void RestoreAllCards()
    {
        foreach (Card c in Board.allCards)
        {
            if (c != null && !c.gameObject.activeInHierarchy)
            {
                c.gameObject.SetActive(true); // 비활성화된 카드도 다시 보여줌
                c.CloseCard();                // 카드 닫기
            }
               
        }

        // 상태 초기화
        GameManager.instance.cardCount = Board.allCards.Count;
       
        shiftTimer = shiftInterval;
        restoreTimer = restoreInterval;

    }
}
