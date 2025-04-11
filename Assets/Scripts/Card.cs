using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Card : MonoBehaviour
{
    //�̱���
    public static Card instance;
    public GameObject front;
    public GameObject back;
    public GameObject blind; // stage3 ����, �ι�° ī��� ������ Ʋ���� �Ⱦ˷���
    public Animator anim;
    public int slotIndex; // stage3(type==2)�� ���忡�� �Ҵ�� ���� ���� �ε��� (0 ~ 15), �����ö� ���� ī�� ��ġ Ȯ�ο�


    public SpriteRenderer frontimage;
    public int cardIndex = 0;
    AudioSource audioSource;
    public AudioClip clip;
    public int type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake() // startscene���� Card ������ �����ÿ� start���� ���� �����ؼ� null�̾ȵǵ��� 
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

    public void Setting(int number) // Card �����պ��� type�� �����ؼ� resources�� �ε����� �����Ͽ� �־��ִ� �޼���
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


        //if(GameManager.instance.isSuffling) // ������ ������
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

    private IEnumerator BlindCoroutine() //Invoke�� �ݴ� ���� (�����ð� ���� �۵�X, �����ð� ���� ���۵�O)
    { 
        blind.SetActive(true);
        back.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        back.SetActive(true);
        blind.SetActive(false);
    }
    






}
