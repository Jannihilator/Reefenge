using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Transform ownerTransform;
    [SerializeField] Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.position = ownerTransform.position + offset;
    }

    public void updateHealthBar(float currentValue, float maxValue){
        slider.value = currentValue/maxValue;

    }
}
