using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class ReplayBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] SpawnShips spawnShips;

    // public Texture2D cursorTexture;
    // public CursorMode cursorMode = CursorMode.Auto;
    // public Vector2 hotSpot = Vector2.zero;
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData pointerEventData){
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
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
