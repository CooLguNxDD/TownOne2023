using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public static MouseController Instance { get; set; } //need to be private

    public event EventHandler MouseOnRightClickEvent;
    public event EventHandler MouseOnLeftClickEvent;

    public Vector3 mouseClickPosition;

    public float ClickingCoolDown;
    public float ClickingCoolDownSetting;
    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("only one MouseController instance available");
        }
        Instance = this;
        ClickingCoolDown = ClickingCoolDownSetting;
    }
    // Update is called once per frame
    void Update()
    {
        ClickingCoolDown -= Time.deltaTime;
        if (Input.GetMouseButton(0) && ClickingCoolDown < 0f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MouseOnLeftClickEvent?.Invoke(this, EventArgs.Empty);
                mouseClickPosition = hit.point;
                ClickingCoolDown = ClickingCoolDownSetting - GameManager.Instance.FB_Buff;
            }
        }
        else if (Input.GetMouseButton(1) && ClickingCoolDown < 0f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MouseOnRightClickEvent?.Invoke(this, EventArgs.Empty);
                mouseClickPosition = hit.point;
                ClickingCoolDown = ClickingCoolDownSetting - GameManager.Instance.FB_Buff;
            }
        }
        
    }
}
