using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;    //�̱���
    public Text Timetxt;
    public float time = 0.0f;
    public Card firstcard;
    public Card secondcard;
    public int cardCount = 0;
    public GameObject retryimage;
    public Animator successAlert;
    public Animator failAlert;

    AudioSource audioSource;
    public AudioClip clip;
    private int matchPairCount = 0;
    private float lastReshuffleTime = 0f; // �����ÿ� �ð� üũ ����

    public bool isSuffling = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
       
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1.0f;
        lastReshuffleTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Timetxt.text = time.ToString("N2");



        if (Card.instance.type == 0)
        {
            if (time > 50f)
            {
                GameOver();
            }
        }
        else if (Card.instance.type == 1)
        {
            if (time > 30f)
            {
                GameOver();
            }
        }
        else if (Card.instance.type == 2)
        {
            if (time > 40f)
            {
                GameOver();
            }
            if (time - lastReshuffleTime > 5f && cardCount > 0) // 5�� �Ѿ������ üũ�� ������ �ڷ�ƾ �ǽ�
            {
                StartCoroutine(ReshuffleRoutine());
                lastReshuffleTime = time;
                
            }
        }
        else if (Card.instance.type == 3)
        {
            if (time > 150f)
            {
                GameOver();
            }
        }
        else
        {
            if (time > 180f)
            {
                GameOver();
            }
        }
    }

    public void Matched()
    {
        if (firstcard.cardIndex == secondcard.cardIndex)
        {
            //firstcard.DestroyCard();
            //secondcard.DestroyCard();
            cardCount -= 2;
            if (Card.instance.type == 0)
            {
                float bonusDelay = matchPairCount * 0.5f; // ¦���߱Ⱑ 1ȸ �����Ҷ����� ���ʽ� �����̰� 0.5�ʾ� �����ؼ� ���̵� ��ȭ
                float bonusDelay2 = matchPairCount * 0.5f;
                firstcard.StartTurnBackCorutine(bonusDelay);
                secondcard.StartTurnBackCorutine(bonusDelay);
                CardFront front1 = firstcard.GetComponentInChildren<CardFront>();
                CardFront front2 = secondcard.GetComponentInChildren<CardFront>();
                if (front1 != null)
                    front1.StartTremBleFrontCard(bonusDelay2);
                if (front2 != null)
                    front2.StartTremBleFrontCard(bonusDelay2);

                Invoke("RestoreCardCount", 10.0f+bonusDelay);
                matchPairCount++;
            }
            else if(Card.instance.type == 1)
            {
                firstcard.DestroyCard();
                secondcard.DestroyCard();
                time -= 6f;
                if(time <0f)
                {
                    time = 0f;
                }
               successAlert.SetBool("isSuccessAlert", true);
               


            }
            else
            {
                firstcard.DestroyCard();
                secondcard.DestroyCard();
            }

            if (cardCount == 0)
            {
                StageResult();
            }
            audioSource.PlayOneShot(clip);
            


        }
        else
        {
            if(Card.instance.type == 0)
            {
                firstcard.CloseCard();
                secondcard.CloseCard();
            }

            else if(Card.instance.type == 1)
            {
                firstcard.CloseCard();
                secondcard.CloseCard();
                time += 3f;
                failAlert.SetBool("isFailAlert", true);
                
            }
            else
            {
                firstcard.CloseCard();
                secondcard.CloseCard();
                
            }
            
           
        }
        firstcard = null;
        secondcard = null;
    }


    private void RestoreCardCount()
    {
        cardCount += 2;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        //���ӿ��� UI Ȱ��ȭ
        retryimage.SetActive(true);
    }

    public void StageResult()
    {
        StageClearData.stageClear[Card.instance.type] = true;
        if(Card.instance.type == 0)
        { 
            SceneManager.LoadScene("Stage1_ResultScene"); 
        }
        else if(Card.instance.type == 1)
        {
            SceneManager.LoadScene("Stage2_ResultScene");
        }
        else if (Card.instance.type == 2)
        {
            SceneManager.LoadScene("Stage3_ResultScene");
        }
        else if (Card.instance.type == 3)
        {
            SceneManager.LoadScene("Stage4_ResultScene");
        }
        else
        {
            SceneManager.LoadScene("Stage5_ResultScene");
        }

    }

    private IEnumerator ReshuffleRoutine()
    {
        if (firstcard != null) // �������� ù��° ī�带 ���� �����̵Ǿ��������, ��ī�尡 �����־ ��ġ��Ű�� �ϳ��� ������ ����
        {
            firstcard.CloseCardInvoke();
            firstcard = null;
        }
        if (secondcard != null)
        {
            secondcard.CloseCardInvoke();
            secondcard = null;
        }

        //if (shuffleMessageUI != null)
        //shuffleMessageUI.SetActive(true);

        isSuffling = true;
        Card[] allCards = Object.FindObjectsByType<Card>(FindObjectsSortMode.None); // ����Ȯ�� 2023 ���Ĺ���
        Card[] remainingCards = allCards.Where(card => card != null).ToArray(); // �ı��� ī�� ������ �迭�� �ٽ� ���


        foreach (Card card in remainingCards)
        {
            if (card != null)
            {
                card.anim.SetBool("IsOpen", true);
                card.front.SetActive(true);
                card.back.SetActive(false);
            }
        }

      
        yield return new WaitForSeconds(0.01f);

            
        List<Vector2> availablePositions = new List<Vector2>();
        foreach (Card card in remainingCards)
        {
           
            Vector2 pos = new Vector2((card.slotIndex % 4) * 1.4f - 2.1f,
                                      (card.slotIndex / 4) * 1.4f - 3.6f);
            availablePositions.Add(pos);
        }
   
        availablePositions = availablePositions.OrderBy(p => Random.value).ToList();

        for (int i = 0; i < remainingCards.Length; i++)
        {
            if (remainingCards[i] != null && remainingCards[i].gameObject != null)
            {
                remainingCards[i].transform.position = availablePositions[i];

            }
        }

        yield return new WaitForSeconds(0.05f);

        foreach (Card card in remainingCards)
        {
            if (card != null)
                card.CloseCardInvoke();
        }

        //if (shuffleMessageUI != null)
        // shuffleMessageUI.SetActive(false);
        isSuffling = false;

        yield break;
    }

}
