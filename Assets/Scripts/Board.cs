using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public static Board Instance;

    public GameObject Card;

    public int[] randomArr;

    public static List<Card> allCards = new List<Card>();
    public static bool isShifting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Card.GetComponent<Card>().type == 1)
        {
            int[] arr1 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };


            arr1 = arr1.OrderBy(x => Random.Range(0f, 7f)).ToArray();
            for (int i = 0; i < 16; i++)
            {
                GameObject Go = Instantiate(Card, this.transform);
                float x = (i % 4) * 1.4f - 2.1f;
                float y = (i / 4) * 1.4f - 3.6f;

                Go.transform.position = new Vector2(x, y);
                Go.GetComponent<Card>().Setting(arr1[i]);
                Go.GetComponent<Card>().slotIndex = i;
            }


            GameManager.instance.cardCount = arr1.Length;
        }
        else
        {
            int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
            //배열을 랜덤하게 섞기
            arr = arr.OrderBy(x => Random.Range(0f, 7f)).ToArray();
            for (int i = 0; i < 16; i++)
            {
                GameObject Go = Instantiate(Card, this.transform);
                float x = (i % 4) * 1.4f - 2.1f;
                float y = (i / 4) * 1.4f - 3.6f;

                Go.transform.position = new Vector2(x, y);
                Go.GetComponent<Card>().Setting(arr[i]);
                Go.GetComponent<Card>().slotIndex = i;
            }
            GameManager.instance.cardCount = arr.Length;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void ShiftCardPositions()
    {
        // 현재 활성화된 카드만 필터링
        List<Card> visibleCards = allCards.FindAll(c => c.gameObject.activeInHierarchy);
        if (visibleCards.Count <= 1) return;

        isShifting = true; // 이동 중 상태 설정

        // 마지막 카드 위치 저장
        Vector3 lastPos = visibleCards[visibleCards.Count - 1].transform.position;

        // 뒤에서부터 한 칸씩 앞 카드 위치로 이동
        for (int i = visibleCards.Count - 1; i > 0; i--)
        {
            visibleCards[i].transform.position = visibleCards[i - 1].transform.position;

            // 이동 중 카드 강제로 열었다 닫기 (이펙트용?)
            visibleCards[i].ForceOpen();
            visibleCards[i].Invoke("CloseCard", 0.01f);
        }

        // 첫 번째 카드를 마지막 위치로 이동
        visibleCards[0].transform.position = lastPos;
        visibleCards[0].ForceOpen();
        visibleCards[0].Invoke("CloseCard", 0.01f);

        isShifting = false; // 이동 끝 상태로 변경
    }
}
