using UnityEngine;
using System.Collections;
public class CardFront : MonoBehaviour
{
    public Animator anim;
    

  


    public void StartTremBleFrontCard (float bonusDelay2)
    {
        if (!gameObject.activeInHierarchy) //CardFront�� ��Ȱ��ȭ, Ȥ�� ���� ������ (���⼱ Card)�� ��Ȱ��ȭ�Ǿ�������
        {
            gameObject.SetActive(true); // CardFront�� Ȱ��ȭ
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
