using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDetect : MonoBehaviour
{
    private bool isCursorOverButton = false;

    
    public void OnPointerEnter()
    {
        Debug.Log("dentro");
        isCursorOverButton = true;
    }

    public void OnPointerExit()
    {
        Debug.Log("fuori");
        isCursorOverButton = false;
    }

    public bool IsCursorOverButton()
    {
        return isCursorOverButton;
    }
}
