using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
//using static UnityEngine.ParticleSystem;
//using Unity.Collections.LowLevel.Unsafe;

// 변수, 메서드용도 주석처리로 설명했습니다. 작업전 체크해주세요.
public class GameManager : MonoBehaviour
{
    public static GameManager instance;    //싱글톤
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
    private int matchPairCount = 0; // 스테이지1(type==0)용 변수, 짝이 맞춰졌는지 카운트 
    //private float lastReshuffleTime = 0f; // 스테이지3 (type==2) 용 변수, 리셔플용 시간 체크 변수 , 리셔플 구버전

    //public bool isSuffling = false;  // 리셔플 구버전

    public GameObject cards;
    public Transform board;
    int[] shuffleArr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 }; // 섞일 이미지의 번호가 들어갈 배열
    int matchedIdx; // 맞춘 카드의 이미지 번호
    int match = 8; // 전체 카드 쌍의 개수
    int matchCount = 0; // 맞춘 카드 쌍의 개수
    int[] matchedArr; // 맞춘 카드의 이미지 번호를 저장할 배열

    public Transform canvas; // 텍스트 프리팹을 canvas 아래에 생성하기 위한 변수
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
        //lastReshuffleTime = 0f;//구버전 셔플

        // 맞춘 카드의 이미지 번호를 저장할 배열 선언
        matchedArr = new int[match];

        // 배열을 만들면 기본적으로 숫자 0이 들어있어 나중에 카드의 이미지 번호와 비교할 때 문제가 없도록 모든 숫자를 -1로 교체
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

            // if (Time.timeScale != 0.0f) // 리셔플 구버전
            // {
            // if (TimeManager.Instance.time - lastReshuffleTime > 5f && cardCount > 0) // 5초 넘어갈때마다 체크후 리셔플 코루틴 실시
            //{
            //StartCoroutine(ReshuffleRoutine());
            //lastReshuffleTime = TimeManager.Instance.time;
            // }
            // }

        }
    }

    public void Matched()
    {
        if (Card.instance.type != 1)//카드타입이 1이 아닐경우
        {
            if (firstcard.cardIndex == secondcard.cardIndex)
            {
                //firstcard.DestroyCard(); //안써서 주석처리
                //secondcard.DestroyCard();
                cardCount -= 2;
                if (Card.instance.type == 0) // 스테이지1(type==0)용
                {
                    float bonusDelay = matchPairCount * 0.5f; // 짝맞추기가 1회 성공할때마다 보너스 딜레이가 0.5초씩 증가해서 난이도 완화
                    float bonusDelay2 = matchPairCount * 0.5f;
                    firstcard.StartTurnBackCorutine(bonusDelay);
                    secondcard.StartTurnBackCorutine(bonusDelay);
                    CardFront front1 = firstcard.GetComponentInChildren<CardFront>(); //CardFront 는 Card에 있는 자식 오브젝트, CardFront.cs로 관리
                    CardFront front2 = secondcard.GetComponentInChildren<CardFront>();
                    if (front1 != null)
                        front1.StartTremBleFrontCard(bonusDelay2);
                    if (front2 != null)
                        front2.StartTremBleFrontCard(bonusDelay2);

                    Invoke("RestoreCardCount", 10.0f + bonusDelay);
                    matchPairCount++;
                }

                else if (Card.instance.type == 2) //스테이지3(type==2)용
                {
                    firstcard.DestroyCard();
                    secondcard.DestroyCard();
                }

                else if (Card.instance.type == 3)//스테이지4(type==3)용
                {
                    firstcard.DestroyCard();
                    secondcard.DestroyCard();
                   
                }

                   




                else if (Card.instance.type == 4) // 스테이지5(type==4)용
                {
                    TimeManager.Instance.plusTime();

                    matchedIdx = firstcard.GetComponent<Card>().cardIndex; // 맞춘 카드의 인덱스 카져오기

                    // 카드를 맞출 때마다 맞춘 카드의 번호를 배열에 저장
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
            else // 짝 못맞췄을경우
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
        else // 카드타입이 1일경우
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

        if (cardCount == 0) // 카드 카운트가 0이 되어버리면, 게임클리어창으로 보냄
        {
            StageResult();
        }
        

        firstcard = null; // 첫번째 카드 초기화
        secondcard = null; // 두번째 카드 초기화
    }


    private void RestoreCardCount() // stage1(cad type==0)의 카드 카운트 복구메서드 
    {
        cardCount += 2;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        //게임오버 UI 활성화
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
        // 배열 랜덤 정렬(카드 재배치에 사용할 배열)
        shuffleArr = shuffleArr.OrderBy(x => Random.Range(0.0f, 7.0f)).ToArray();

        // 셔플 이미지 화면에 활성화 후 잠시 뒤 비활성화
        shuffleImage.SetActive(true);
        Invoke("disableShuffleImage", 0.5f);

        // 섞은 카드를 재생성하면 문제가 발생하기에 원래 화면에 있던 카드 제거
        foreach (Transform card in board)
        {
            Destroy(card.gameObject);
        }

        // 맞춘 카드를 제외한 카드들 재배치
        for (int i = 0; i < shuffleArr.Length; i++)
        {
            int num = shuffleArr[i];

            bool isMatched = false;
            for (int j = 0; j < matchedArr.Length; j++)
            {
                if (num == matchedArr[j]) // 맞춘 카드의 이미지 번호와 섞인 배열의 숫자와 비교
                {
                    // 이미 맞춘 카드라면 isMatched 값을 참으로 변경
                    isMatched = true;
                    break;
                }
            }

            if (isMatched)
            {
                continue; // 다음 카드로 넘어감
            }

            GameObject go = Instantiate(cards, board.transform);

            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.6f;
            go.transform.position = new Vector2(x, y);

            go.GetComponent<Card>().Setting(shuffleArr[i]);
        }

        firstcard = null; // 첫번째 카드 초기화
        secondcard = null; // 두번째 카드 초기화
    }

    void disableShuffleImage() 
    {
        shuffleImage.SetActive(false); // 셔플 이미지 비활성화
    }

    



    // private IEnumerator ReshuffleRoutine() // 스테이지3(type==2) 리셔플 코루틴 구버전 셔플
    // {
    //     if (firstcard != null) // 셔플전에 첫번째 카드를 고르고 셔플이되어버렸을때, 고른카드가 열려있어서 매치시키면 하나만 삭제됌 수정
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
    //    Card[] allCards = Object.FindObjectsByType<Card>(FindObjectsSortMode.None); // allCards는 현재씬에있는 모든 Card 타입을 찾아서 배열로 반환
    //    Card[] remainingCards = allCards.Where(card => card != null).ToArray(); // 파괴된 카드혹은 지정안된 카드 제외후 배열에 다시 담기


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
