using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public static MouseController Instance { get; set; } //need to be private

    public event EventHandler MouseOnClickEvent;

    public Vector3 mouseClickPosition;
    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("only one MouseController instance available");
        }
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MouseOnClickEvent?.Invoke(this, EventArgs.Empty);
                mouseClickPosition = hit.point;
            }
        }
    }
}
