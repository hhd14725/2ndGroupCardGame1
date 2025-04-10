using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    public static Board Instance;

    public GameObject Card;

    public int[] randomArr;

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
            //¹è¿­À» ·£´ýÇÏ°Ô ¼¯±â
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
}
