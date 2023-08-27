using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] SpawnShips spawnShips;
    [SerializeField] Image pause;
    [SerializeField] Image play;
    [SerializeField] AudioSource bgm;
    // public Texture2D cursorTexture;
    // public CursorMode cursorMode = CursorMode.Auto;
    // public Vector2 hotSpot = Vector2.zero;
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData pointerEventData){
        if(gameObject.name=="Play"){
            Time.timeScale=1;
            bgm.Play();
        }else{
            Time.timeScale = 0;
            bgm.Pause();
        }
        pause.enabled=!pause.enabled;
        play.enabled=!play.enabled;
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
        //Output the following message with the GameObject's name
        // Cursor.SetCursor(null, Vector2.zero, cursorMode);
        spawnShips.isUI=false;


    }

    
}
