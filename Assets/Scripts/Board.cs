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
            //�迭�� �����ϰ� ����
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
        // ���� Ȱ��ȭ�� ī�常 ���͸�
        List<Card> visibleCards = allCards.FindAll(c => c.gameObject.activeInHierarchy);
        if (visibleCards.Count <= 1) return;

        isShifting = true; // �̵� �� ���� ����

        // ������ ī�� ��ġ ����
        Vector3 lastPos = visibleCards[visibleCards.Count - 1].transform.position;

        // �ڿ������� �� ĭ�� �� ī�� ��ġ�� �̵�
        for (int i = visibleCards.Count - 1; i > 0; i--)
        {
            visibleCards[i].transform.position = visibleCards[i - 1].transform.position;

            // �̵� �� ī�� ������ ������ �ݱ� (����Ʈ��?)
            visibleCards[i].ForceOpen();
            visibleCards[i].Invoke("CloseCard", 0.01f);
        }

        // ù ��° ī�带 ������ ��ġ�� �̵�
        visibleCards[0].transform.position = lastPos;
        visibleCards[0].ForceOpen();
        visibleCards[0].Invoke("CloseCard", 0.01f);

        isShifting = false; // �̵� �� ���·� ����
    }
}
