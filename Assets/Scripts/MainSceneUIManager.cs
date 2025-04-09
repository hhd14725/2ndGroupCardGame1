using UnityEngine;

public class MainSceneUIManager : MonoBehaviour
{
    public GameObject clear0;
    public GameObject clear1;
    public GameObject clear2;
    public GameObject clear3;
    public GameObject clear4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCleartext();
    }

    // Update is called once per frame
    void UpdateCleartext()
    {
        clear0.SetActive(StageClearData.stageClear[0]);
        clear1.SetActive(StageClearData.stageClear[1]);
        clear2.SetActive(StageClearData.stageClear[2]);
        clear3.SetActive(StageClearData.stageClear[3]);
        clear4.SetActive(StageClearData.stageClear[4]);

    }

}
