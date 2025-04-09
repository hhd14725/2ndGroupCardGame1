using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    AudioSource audioSource;
    public AudioClip clip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �ٸ� ������ �Ѿ�� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �����ϴ� �ν��Ͻ��� ������ ���� ������Ʈ ����
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = this.clip; // audioSource�� clip�� AudioManager�� clip���� �Ҵ�
        audioSource.Play();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
