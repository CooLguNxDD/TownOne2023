using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleElement : MonoBehaviour
{
    public GameObject element;
    public void Toggle()
    {
        element.SetActive(!element.activeSelf);
        
    }
}
