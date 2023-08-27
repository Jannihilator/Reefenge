using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myManaBar : MonoBehaviour {
    [SerializeField] RectTransform barMaskRectTransform;
    [SerializeField] RawImage barRawImage;
    public int manaValue=10;
    public int manaMaxValue=10;
    float ratio;
    float width;
    float y;
    float x;
    float orignalSizeDeltaX;
    void Start(){
        orignalSizeDeltaX = barMaskRectTransform.sizeDelta.x;
        ratio = barMaskRectTransform.sizeDelta.x/manaMaxValue;
        width = barMaskRectTransform.sizeDelta.y;
        y = barMaskRectTransform.anchoredPosition.y;
        x = barMaskRectTransform.anchoredPosition.x;
    }
    private void Update() {
        Rect uvRect = barRawImage.uvRect;
        uvRect.x += .2f * Time.deltaTime;
        barRawImage.uvRect = uvRect;
    }

    public void updateManaBar(int manaSpent){
        manaValue -= manaSpent;
        barMaskRectTransform.sizeDelta = new Vector2(manaValue*ratio, width);
        barMaskRectTransform.anchoredPosition = new Vector2(x, y-(manaMaxValue-manaValue)*ratio/2);
    }

    public void setManaBar(int max){
        manaMaxValue=max;
        manaValue=manaMaxValue;
        ratio = orignalSizeDeltaX/manaMaxValue;
        barMaskRectTransform.sizeDelta = new Vector2(manaValue*ratio, width);
        barMaskRectTransform.anchoredPosition = new Vector2(x, y-(manaMaxValue-manaValue)*ratio/2);

    }

    

}

