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
            DontDestroyOnLoad(gameObject); // 다른 씬으로 넘어가도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 인스턴스가 있으면 현재 오브젝트 삭제
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = this.clip; // audioSource의 clip을 AudioManager의 clip으로 할당
        audioSource.Play();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
