using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class NextBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // public Texture2D cursorTexture;
    // public CursorMode cursorMode = CursorMode.Auto;
    // public Vector2 hotSpot = Vector2.zero;
    [SerializeField] GameObject Tutorial;
    [SerializeField] GameObject greenArrow;
    [SerializeField] GameObject blueArrow;

    [SerializeField] GameObject redArrow;
    [SerializeField] SpawnShips spawnShips;

    public int progress=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        
        // Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        spawnShips.isUI=true;

    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        
        // Cursor.SetCursor(null, Vector2.zero, cursorMode);
        spawnShips.isUI=false;

    }
    public void OnPointerClick(PointerEventData pointerEventData){
        progress++;
        gameObject.SetActive(false);
        Tutorial.GetComponent<Animator>().SetBool("PopTut", false);

        if(progress==1){
            greenArrow.SetActive(false);
            Tutorial.GetComponent<Text>().text="Resource is limited";
            blueArrow.SetActive(true);
        }
        if(progress==2){
            blueArrow.SetActive(false);
            Tutorial.GetComponent<Text>().text="beat the \"player\" ";
            redArrow.SetActive(true);
            redArrow.GetComponent<Animator>().SetBool("Red", true);
        }
        if(progress==3){
            redArrow.SetActive(false);
            gameObject.SetActive(false);
            Tutorial.SetActive(false);
        }
        Invoke("ResetAnimation", 0.1f);
    }

    void ResetAnimation(){
        Tutorial.GetComponent<Animator>().SetBool("PopTut", true);

    }
}
