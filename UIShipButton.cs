using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShipButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // public Texture2D cursorTexture;
    // public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    [SerializeField] SpawnShips spawnShips;
    [SerializeField] Image imageSelf;
    [SerializeField] Image imageOther1;
    [SerializeField] Image imageOther2;
    [SerializeField] Animator animatorSelf;
    [SerializeField] Animator animatorOther1;
    [SerializeField] Animator animatorOther2;
    [SerializeField] UIShipButton button2;
    [SerializeField] UIShipButton button3;
    [SerializeField] AudioSource hoverSound;
    [SerializeField] AudioSource selectSound;
    [SerializeField] StatBtn statBtn;
    public bool selected = false;
    
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData pointerEventData){
        
        selected = !selected;
        if(selected){
            selectSound.PlayOneShot(selectSound.clip);
            animatorSelf.SetInteger("state", 2);
            imageSelf.enabled = true;
            spawnShips.selected=true;
            animatorOther1.SetInteger("state", 0);
            animatorOther2.SetInteger("state", 0);
            imageOther1.enabled=false;
            imageOther2.enabled=false;
            button2.selected = false;
            button3.selected = false;

            if(gameObject.name == "Ship1"){
                spawnShips.shipId = 1;
            }
            if(gameObject.name == "Ship2"){
                spawnShips.shipId = 2;
            }
            if(gameObject.name == "Ship3"){
                spawnShips.shipId = 3;
            }
            statBtn.showStats(spawnShips.shipId);
        }else{
            animatorSelf.SetInteger("state", 0);
            imageSelf.enabled=false;

            spawnShips.selected=false;

        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData){
        imageSelf.enabled = true;
        
        animatorSelf.SetInteger("state", 1);
        hoverSound.PlayOneShot(hoverSound.clip);
        // Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        spawnShips.isUI=true;
    }
    public void OnPointerExit(PointerEventData pointerEventData){
        if(!selected){
            imageSelf.enabled = false; 
            animatorSelf.SetInteger("state", 0);
        }
        // Cursor.SetCursor(null, Vector2.zero, cursorMode);
        spawnShips.isUI=false;

    }
}
