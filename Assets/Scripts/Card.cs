using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Card : MonoBehaviour
{
    //�̱���
    public static Card instance;
    public GameObject front;
    public GameObject back;
    public Animator anim;
    public int slotIndex; // ���忡�� �Ҵ�� ���� ���� �ε��� (0 ~ 15), �����ö� ���� ī�� ��ġ Ȯ�ο�


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





        //board���� �����Ǵ� card ������ 16�忡�� ���ҽ� �������� Jin00 ~ Jin07 �̸��� 8���� �̹����� ������� �ο�, �������ƴ�



    }

    public void OpenCard()
    {
        GameManager.instance.successAlert.SetBool("isSuccessAlert", false); // stage2 ���� ���� �˸��� ���� �ʵ���, ��� �������������� �ʱ�ȭ
        GameManager.instance.failAlert.SetBool("isFailAlert", false); // stage2 ���� ���� �˸��� ���� �ʵ���, ��� �������������� �ʱ�ȭ

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

    // ����ġ������ ���̸��� �����Ͽ� frontimage.sprite = Resources.Load<Sprite>($"Jin{cardIndex}"); �� Jin�̶�� string ���ͷ��� case���� �ٲٰ�
    //�Ͽ� Setting�Լ����� if ���ǹ��� ���� Resources ������ Jin0~Jin7, Back0~Back7 �� Scene���� �ٸ��� �ο�.

   

  




}
