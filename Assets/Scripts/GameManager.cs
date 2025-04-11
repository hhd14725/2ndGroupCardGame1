using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
//using static UnityEngine.ParticleSystem;
//using Unity.Collections.LowLevel.Unsafe;

// ����, �޼���뵵 �ּ�ó���� �����߽��ϴ�. �۾��� üũ���ּ���.
public class GameManager : MonoBehaviour
{
    public static GameManager instance;    //�̱���
    public Text Timetxt;
    public Time TImeManager;
    public Card firstcard;
    public Card secondcard;
    public int cardCount = 0;

    public GameObject shuffleImage;
    public GameObject endPanel;

    public Text TurnTxt;
    public float turn = 300f;


    AudioSource audioSource;
    public AudioClip clip;
    private int matchPairCount = 0; // ��������1(type==0)�� ����, ¦�� ���������� ī��Ʈ 
    //private float lastReshuffleTime = 0f; // ��������3 (type==2) �� ����, �����ÿ� �ð� üũ ���� , ������ ������

    //public bool isSuffling = false;  // ������ ������

    public GameObject cards;
    public Transform board;
    int[] shuffleArr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 }; // ���� �̹����� ��ȣ�� �� �迭
    int matchedIdx; // ���� ī���� �̹��� ��ȣ
    int match = 8; // ��ü ī�� ���� ����
    int matchCount = 0; // ���� ī�� ���� ����
    int[] matchedArr; // ���� ī���� �̹��� ��ȣ�� ������ �迭

    public Transform canvas; // �ؽ�Ʈ �������� canvas �Ʒ��� �����ϱ� ���� ����
    public Text plus;
    public Text minus;
    public Text plusText;
    public Text minusText;

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
        Time.timeScale = 0.0f;
        //lastReshuffleTime = 0f;//������ ����

        // ���� ī���� �̹��� ��ȣ�� ������ �迭 ����
        matchedArr = new int[match];

        // �迭�� ����� �⺻������ ���� 0�� ����־� ���߿� ī���� �̹��� ��ȣ�� ���� �� ������ ������ ��� ���ڸ� -1�� ��ü
        for (int i = 0; i < matchedArr.Length; i++)
        {
            matchedArr[i] = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TurnTxt.text = turn.ToString("N0");
        if (turn < 0)
        {
            Time.timeScale = 0.0f;
            endPanel.SetActive(true);

            // if (Time.timeScale != 0.0f) // ������ ������
            // {
            // if (TimeManager.Instance.time - lastReshuffleTime > 5f && cardCount > 0) // 5�� �Ѿ������ üũ�� ������ �ڷ�ƾ �ǽ�
            //{
            //StartCoroutine(ReshuffleRoutine());
            //lastReshuffleTime = TimeManager.Instance.time;
            // }
            // }

        }
    }

    public void Matched()
    {
        if (Card.instance.type != 1)//ī��Ÿ���� 1�� �ƴҰ��
        {
            if (firstcard.cardIndex == secondcard.cardIndex)
            {
                //firstcard.DestroyCard(); //�ȽἭ �ּ�ó��
                //secondcard.DestroyCard();
                cardCount -= 2;
                if (Card.instance.type == 0) // ��������1(type==0)��
                {
                    float bonusDelay = matchPairCount * 0.5f; // ¦���߱Ⱑ 1ȸ �����Ҷ����� ���ʽ� �����̰� 0.5�ʾ� �����ؼ� ���̵� ��ȭ
                    float bonusDelay2 = matchPairCount * 0.5f;
                    firstcard.StartTurnBackCorutine(bonusDelay);
                    secondcard.StartTurnBackCorutine(bonusDelay);
                    CardFront front1 = firstcard.GetComponentInChildren<CardFront>(); //CardFront �� Card�� �ִ� �ڽ� ������Ʈ, CardFront.cs�� ����
                    CardFront front2 = secondcard.GetComponentInChildren<CardFront>();
                    if (front1 != null)
                        front1.StartTremBleFrontCard(bonusDelay2);
                    if (front2 != null)
                        front2.StartTremBleFrontCard(bonusDelay2);

                    Invoke("RestoreCardCount", 10.0f + bonusDelay);
                    matchPairCount++;
                }

                else if (Card.instance.type == 2) //��������3(type==2)��
                {
                    firstcard.DestroyCard();
                    secondcard.DestroyCard();
                }

                else if (Card.instance.type == 3)//��������4(type==3)��
                {
                    firstcard.DestroyCard();
                    secondcard.DestroyCard();
                   
                }

                   




                else if (Card.instance.type == 4) // ��������5(type==4)��
                {
                    TimeManager.Instance.plusTime();

                    matchedIdx = firstcard.GetComponent<Card>().cardIndex; // ���� ī���� �ε��� ī������

                    // ī�带 ���� ������ ���� ī���� ��ȣ�� �迭�� ����
                    matchedArr[matchCount] = matchedIdx; 
                    matchCount++;

                    firstcard.DestroyCard();
                    secondcard.DestroyCard();
                }
                else
                {
                    firstcard.DestroyCard();
                    secondcard.DestroyCard();


                }
                audioSource.PlayOneShot(clip);
            }
            else // ¦ �����������
            {
                if (Card.instance.type == 0)
                {
                    firstcard.CloseCard();
                    secondcard.CloseCard();
                }


                else if (Card.instance.type == 2)
                {
                    firstcard.CloseCard();
                    secondcard.BlindCard();
                }
                else if (Card.instance.type == 3)
                {
                    firstcard.CloseCard();
                    secondcard.CloseCard();
                }
                else if (Card.instance.type == 4)
                {
                    TimeManager.Instance.minusTime();

                    firstcard.CloseCard();
                    secondcard.CloseCard();
                }
            }
        }
        else // ī��Ÿ���� 1�ϰ��
        {
            if (firstcard.cardIndex + secondcard.cardIndex == 15f)
            {
                cardCount -= 2;

                firstcard.DestroyCard();
                secondcard.DestroyCard();

                audioSource.PlayOneShot(clip);
            }
            else
            {
                firstcard.CloseCard();
                secondcard.CloseCard();
            }

        }

        if (cardCount == 0) // ī�� ī��Ʈ�� 0�� �Ǿ������, ����Ŭ����â���� ����
        {
            StageResult();
        }
        

        firstcard = null; // ù��° ī�� �ʱ�ȭ
        secondcard = null; // �ι�° ī�� �ʱ�ȭ
    }


    private void RestoreCardCount() // stage1(cad type==0)�� ī�� ī��Ʈ �����޼��� 
    {
        cardCount += 2;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        //���ӿ��� UI Ȱ��ȭ
        endPanel.SetActive(true);
    }

    public void StageResult()
    {
        StageClearData.stageClear[Card.instance.type] = true;
        if (Card.instance.type == 0)
        {
            SceneManager.LoadScene("Stage1_ResultScene");
        }
        else if (Card.instance.type == 1)
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

    public void Shuffle()
    {
        // �迭 ���� ����(ī�� ���ġ�� ����� �迭)
        shuffleArr = shuffleArr.OrderBy(x => Random.Range(0.0f, 7.0f)).ToArray();

        // ���� �̹��� ȭ�鿡 Ȱ��ȭ �� ��� �� ��Ȱ��ȭ
        shuffleImage.SetActive(true);
        Invoke("disableShuffleImage", 0.5f);

        // ���� ī�带 ������ϸ� ������ �߻��ϱ⿡ ���� ȭ�鿡 �ִ� ī�� ����
        foreach (Transform card in board)
        {
            Destroy(card.gameObject);
        }

        // ���� ī�带 ������ ī��� ���ġ
        for (int i = 0; i < shuffleArr.Length; i++)
        {
            int num = shuffleArr[i];

            bool isMatched = false;
            for (int j = 0; j < matchedArr.Length; j++)
            {
                if (num == matchedArr[j]) // ���� ī���� �̹��� ��ȣ�� ���� �迭�� ���ڿ� ��
                {
                    // �̹� ���� ī���� isMatched ���� ������ ����
                    isMatched = true;
                    break;
                }
            }

            if (isMatched)
            {
                continue; // ���� ī��� �Ѿ
            }

            GameObject go = Instantiate(cards, board.transform);

            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.6f;
            go.transform.position = new Vector2(x, y);

            go.GetComponent<Card>().Setting(shuffleArr[i]);
        }

        firstcard = null; // ù��° ī�� �ʱ�ȭ
        secondcard = null; // �ι�° ī�� �ʱ�ȭ
    }

    void disableShuffleImage() 
    {
        shuffleImage.SetActive(false); // ���� �̹��� ��Ȱ��ȭ
    }

    



    // private IEnumerator ReshuffleRoutine() // ��������3(type==2) ������ �ڷ�ƾ ������ ����
    // {
    //     if (firstcard != null) // �������� ù��° ī�带 ���� �����̵Ǿ��������, ��ī�尡 �����־ ��ġ��Ű�� �ϳ��� ������ ����
    //    {
    //         firstcard.CloseCardInvoke();
    //         firstcard = null;
    //     }
    //     if (secondcard != null)
    //     {
    //         secondcard.CloseCardInvoke();
    //         secondcard = null;
    //     }

    //if (shuffleMessageUI != null)
    //shuffleMessageUI.SetActive(true);

    //    isSuffling = true;
    //    Card[] allCards = Object.FindObjectsByType<Card>(FindObjectsSortMode.None); // allCards�� ��������ִ� ��� Card Ÿ���� ã�Ƽ� �迭�� ��ȯ
    //    Card[] remainingCards = allCards.Where(card => card != null).ToArray(); // �ı��� ī��Ȥ�� �����ȵ� ī�� ������ �迭�� �ٽ� ���


    //    foreach (Card card in remainingCards)
    //    {
    //        if (card != null)
    //        {
    //            card.anim.SetBool("IsOpen", true);
    //            card.front.SetActive(true);
    //            card.back.SetActive(false);
    //        }
    //    }


    //    yield return new WaitForSeconds(0.01f);


    //    List<Vector2> availablePositions = new List<Vector2>();
    //    foreach (Card card in remainingCards)
    //    {

    //        Vector2 pos = new Vector2((card.slotIndex % 4) * 1.4f - 2.1f,
    //                                  (card.slotIndex / 4) * 1.4f - 3.6f);
    //        availablePositions.Add(pos);
    //    }

    //  availablePositions = availablePositions.OrderBy(p => Random.value).ToList();

    //    for (int i = 0; i < remainingCards.Length; i++)
    //    {
    //        if (remainingCards[i] != null && remainingCards[i].gameObject != null)
    //      {
    //          remainingCards[i].transform.position = availablePositions[i];

    //      }
    //  }

    //  yield return new WaitForSeconds(0.01f);

    //  foreach (Card card in remainingCards)
    //  {
    //      if (card != null)
    //          card.CloseCardInvoke();
    //  }

    //if (shuffleMessageUI != null)
    // shuffleMessageUI.SetActive(false);
    //  isSuffling = false;

    //  yield break;
    //}

}
