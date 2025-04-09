using UnityEngine;
using System.Collections;
public class CardFront : MonoBehaviour
{
    public Animator anim;
    

  


    public void StartTremBleFrontCard (float bonusDelay2)
    {
        if (!gameObject.activeInHierarchy) //CardFront이 비활성화, 혹은 같은 계층의 (여기선 Card)가 비활성화되어있을때
        {
            gameObject.SetActive(true); // CardFront을 활성화
        }
        StartCoroutine(FrontCardTremBle(bonusDelay2)); 
        
    }

    private IEnumerator FrontCardTremBle(float bonusDelay2) 
    {
        


        yield return new WaitForSeconds(5f+bonusDelay2);
        
        anim.SetBool("IsFront", true);

        yield return new WaitForSeconds(5f);

        anim.SetBool("IsFront", false);


    }


}
