using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Card : MonoBehaviour
{
    //싱글톤
    public static Card instance;
    public GameObject front;
    public GameObject back;
    public Animator anim;
    public int slotIndex; // 보드에서 할당된 고유 슬롯 인덱스 (0 ~ 15), 리셔플때 남은 카드 위치 확인용


    public SpriteRenderer frontimage;
    public int cardIndex = 0;
    AudioSource audioSource;
    public AudioClip clip;
    public int type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
    }


    void Start()
    {
        audioSource = GetComponent<AudioSource>();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setting(int number)
    {
        if(type==0)
        {
            cardIndex = number;
            frontimage.sprite = Resources.Load<Sprite>($"Jin{cardIndex}");
        }
        else if(type == 1)
        {
            cardIndex = number;
            frontimage.sprite = Resources.Load<Sprite>($"Kyo{cardIndex}");
        }
        else if (type == 2)
        {
            cardIndex = number;
            frontimage.sprite = Resources.Load<Sprite>($"Back{cardIndex}");
        }
        else if (type == 3)
        {
            cardIndex = number;
            frontimage.sprite = Resources.Load<Sprite>($"Chang{cardIndex}");
        }
        else
        {
            cardIndex = number;
            frontimage.sprite = Resources.Load<Sprite>($"Sae{cardIndex}");
        }





        //board에서 생성되는 card 프리팹 16장에게 리소스 폴더내의 Jin00 ~ Jin07 이름의 8개의 이미지를 순서대로 부여, 난수가아님



    }

    public void OpenCard()
    {
        GameManager.instance.successAlert.SetBool("isSuccessAlert", false); // stage2 에서 성공 알림이 뜨지 않도록, 모든 스테이지에서도 초기화
        GameManager.instance.failAlert.SetBool("isFailAlert", false); // stage2 에서 실패 알림이 뜨지 않도록, 모든 스테이지에서도 초기화

        if(GameManager.instance.isSuffling)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
        anim.SetBool("IsOpen", true);
        
        front.SetActive(true);
        back.SetActive(false);
        if (GameManager.instance.firstcard == null)
        {
            GameManager.instance.firstcard = this;
            
        }
        else
        {
            GameManager.instance.secondcard = this;
            GameManager.instance.Matched();
            

        }

    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.5f);
    }

    public void CloseCardInvoke()
    {
        front.SetActive(false);
        back.SetActive(true);
        anim.SetBool("IsOpen", false);
        
    }


  public void StartTurnBackCorutine(float bonusDelay)
    {
        StartCoroutine(TurnBackCoroutine(bonusDelay));
    }

    private IEnumerator TurnBackCoroutine(float bonusDelay)
    {
        



        yield return new WaitForSeconds(5f+bonusDelay);
        anim.SetBool("IsOpen", false);
        anim.SetBool("IsTurnBackStart", true);
      

        yield return new WaitForSeconds(5f);

        anim.SetBool("IsTurnBackStart", false);
        
        front.SetActive(false);
        back.SetActive(true);
        
    }





    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 0.5f);
    }
    public void DestroyCardInvoke()
    {
       Destroy(this.gameObject);
    }

    // 스위치문으로 씬이름을 참고하여 frontimage.sprite = Resources.Load<Sprite>($"Jin{cardIndex}"); 의 Jin이라는 string 리터럴을 case별로 바꾸게
    //하여 Setting함수에서 if 조건문에 따라 Resources 폴더의 Jin0~Jin7, Back0~Back7 을 Scene별로 다르게 부여.

   

  




}
