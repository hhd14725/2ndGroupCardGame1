using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEditor;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; } //�̱���
    public GameManager gameManager;
    public float time = 30.0f;
    public Text Timetxt;

    public float passedTime = 0.0f;
    public float limitTime = 30f;     // ���� �ð� ��������4
    public float shiftInterval = 5f;  // ī�� �̵� ���� ��������4
    private float shiftTimer;         // ī�� �̵� Ÿ�̸� ��������4

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
        Time.timeScale = 0.0f; //StartButton�� ���� 1.0f�� �ٲ��ش�
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
                Debug.Log("����");
            }
        }
        if (Card.instance.type == 3)
        {
           
            if (time <= 0f)
            {
                time = 0f;
                RestoreAllCards(); // ��ü �ʱ�ȭ
            }
            shiftTimer -= Time.deltaTime;
            // ������ �ð����� ī�� ��ġ �̵�
            if (shiftTimer <= 0f)
            {
                shiftTimer = shiftInterval;
                Board.ShiftCardPositions(); // ī�� �б�
            }
        }

    }

    public void plusTime()
    {
        time += 2.0f;
        GameManager.instance.plusText = Instantiate(GameManager.instance.plus, GameManager.instance.canvas.transform);
        Destroy(GameManager.instance.plusText.gameObject, 1f);
    }

    public void minusTime()
    {
        time -= 2.0f;
        GameManager.instance.minusText = Instantiate(GameManager.instance.minus, GameManager.instance.canvas.transform);
        Destroy(GameManager.instance.minusText.gameObject, 1f);
    }
    void RestoreAllCards()
    {
        foreach (Card c in Board.allCards)
        {
            c.gameObject.SetActive(true); // ��Ȱ��ȭ�� ī�嵵 �ٽ� ������
            c.CloseCard();                // ī�� �ݱ�
        }

        // ���� �ʱ�ȭ
        GameManager.instance.cardCount = Board.allCards.Count;
        time =limitTime;
        shiftTimer = shiftInterval;


    }
}
