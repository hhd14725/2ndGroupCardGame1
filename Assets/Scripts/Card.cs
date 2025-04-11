using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Card : MonoBehaviour
{
    //싱글톤
    public static Card instance;
    public GameObject front;
    public GameObject back;
    public GameObject blind; // stage3 변수, 두번째 카드는 깠을때 틀리면 안알려줌
    public Animator anim;
    public int slotIndex; // stage3(type==2)의 보드에서 할당된 고유 슬롯 인덱스 (0 ~ 15), 리셔플때 남은 카드 위치 확인용


    public SpriteRenderer frontimage;
    public int cardIndex = 0;
    AudioSource audioSource;
    public AudioClip clip;
    public int type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake() // startscene에서 Card 프리팹 생성시에 start보다 먼저 생성해서 null이안되도록 
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

    public void Setting(int number) // Card 프리팹별로 type을 구분해서 resources의 인덱스를 구분하여 넣어주는 메서드
    {
        if(type==0)
        {
            cardIndex = number;
            frontimage.sprite = Resources.Load<Sprite>($"Jin{cardIndex}");
        }
        else if(type == 1)
        {
            cardIndex = number;
            frontimage.sprite = Resources.Load<Sprite>($"Chang{cardIndex}"); 

            GameManager.instance.turn -= 17.5f;
            TimeManager.Instance.time += 50f;


        }
        else if (type == 2)
        {
            cardIndex = number;
            frontimage.sprite = Resources.Load<Sprite>($"Back{cardIndex}");
        }
        else if (type == 3)
        {
            cardIndex = number;
            frontimage.sprite = Resources.Load<Sprite>($"Kyo{cardIndex}");
        }
        else
        {
            cardIndex = number;
            frontimage.sprite = Resources.Load<Sprite>($"Sae{cardIndex}");
        }





        



    }

    public void OpenCard()
    {


        //if(GameManager.instance.isSuffling) // 리셔플 구버전
        //{
        //return;
        // }
        if (Time.timeScale > 0.0f)
        {
            GameManager.instance.turn -= 0.5f;

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


        if (Card.instance.type == 3)
        {
            Invoke("HideCard", 0.5f);
        }
        else
        {
            Invoke("DestroyCardInvoke", 0.5f);
        }

    }

    public void HideCard()
    {
        gameObject.SetActive(false);
    }

    public void DestroyCardInvoke()
    {
       Destroy(this.gameObject);
    }

    public void BlindCard() //Stage 3 
    {
        StartCoroutine(BlindCoroutine());
        front.SetActive(false);
        anim.SetBool("IsOpen", false);
    }
    public void ForceOpen()
    {
        anim.SetBool("IsOpen", true);
        front.SetActive(true);
        back.SetActive(false);
    }

    private IEnumerator BlindCoroutine() //Invoke의 반대 역할 (설정시간 이후 작동X, 설정시간 이후 비작동O)
    { 
        blind.SetActive(true);
        back.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        back.SetActive(true);
        blind.SetActive(false);
    }
    






}
