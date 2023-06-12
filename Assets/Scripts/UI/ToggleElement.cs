using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenuPane : MonoBehaviour
{
    public GameObject element;
    public void Toggle()
    {
        element.SetActive(!element.activeSelf);
    }
}
