using UnityEngine;
using UnityEngine.SceneManagement;

public class GoSceneBtn : MonoBehaviour
{

 
    public void GoMain()
    {
        
        SceneManager.LoadScene("MainScene");
    }

 public void GoStagin1()
    {
        SceneManager.LoadScene("Stage1_BattleScene");
    }
 public void GoStagin2()
    {
        SceneManager.LoadScene("Stage2_BattleScene");
    }
    public void GoStagin3()
    {
        SceneManager.LoadScene("Stage3_BattleScene");
    }
    public void GoStagin4()
    {
        SceneManager.LoadScene("Stage4_BattleScene");
    }
    public void GoStagin5()
    {
        SceneManager.LoadScene("Stage5_BattleScene");
    }

    public void ResulttoMain()
    {
       
        if (Card.instance.type==0)
        {
         SceneManager.LoadScene("MainScene");
        
        }
        else if(Card.instance.type==1) 
        {
         SceneManager.LoadScene("MainScene");
       
        }
        else if (Card.instance.type == 2)
        {
            SceneManager.LoadScene("MainScene");
        
        }
        else if (Card.instance.type == 3)
        {
            SceneManager.LoadScene("MainScene");

          
        }
        else if (Card.instance.type == 4)
        {
            SceneManager.LoadScene("MainScene");
           
        }
    }

}
