using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEditor;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance; //�̱���
    public GameManager gameManager;
    public float time = 30.0f;
    public Text Timetxt;

    public float passedTime = 0.0f;
    public float shiftInterval = 5f;  // ī�� �̵� ���� ��������4
    private float shiftTimer;         // ī�� �̵� Ÿ�̸� ��������4

    public float restoreInterval = 20f;   // ī�� ���� ���� (��: 20��)
    private float restoreTimer;           // ���� Ÿ�̸�


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
               // RestoreAllCards(); // ��ü �ʱ�ȭ
            }
            shiftTimer -= Time.deltaTime;
            // ������ �ð����� ī�� ��ġ �̵�
            if (shiftTimer <= 0f)
            {
                shiftTimer = shiftInterval;
 
                Board.ShiftCardPositions(); // ī�� �б�
            }
            restoreTimer -= Time.deltaTime;
            if (restoreTimer <= 0f)
            {
                RestoreAllCards();
                restoreTimer = restoreInterval;
            }
        }

    }

    public void plusTime() // �ð� ���� �� ȭ�鿡 �ð� ���� ǥ��
    {
        time += 2.0f;
        GameManager.instance.plusText = Instantiate(GameManager.instance.plus, GameManager.instance.canvas.transform);
        Destroy(GameManager.instance.plusText.gameObject, 1f);
    }

    public void minusTime() // �ð� ���� �� ȭ�鿡 �ð� ���� ǥ��
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
                c.gameObject.SetActive(true); // ��Ȱ��ȭ�� ī�嵵 �ٽ� ������
                c.CloseCard();                // ī�� �ݱ�
            }
               
        }

        // ���� �ʱ�ȭ
        GameManager.instance.cardCount = Board.allCards.Count;
       
        shiftTimer = shiftInterval;
        restoreTimer = restoreInterval;

    }
}
