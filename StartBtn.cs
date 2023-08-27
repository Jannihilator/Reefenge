using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class StartBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] AudioSource btnSound;
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData pointerEventData){
        SceneManager.LoadScene("SampleScene");
        if(gameObject.name=="TutorialBtn"){
            Variables.isTutorial=true;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        GetComponent<Animator>().SetBool("Hover", true);
        btnSound.PlayOneShot(btnSound.clip);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
       GetComponent<Animator>().SetBool("Hover", false);
    }
    
}
