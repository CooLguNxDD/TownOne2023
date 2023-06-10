using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    [SerializeField] private GameObject hasHPsGameObject;
    [SerializeField] private Image barImage;
    [SerializeField] private Image barImageBG;
    [SerializeField] private Canvas parentCanvas;


    public Camera mainCamera;


    private IHasHpBar hasHp;


    private void Start() {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        parentCanvas.worldCamera = mainCamera;
        hasHp = hasHPsGameObject.GetComponent<IHasHpBar>();
        if (hasHp == null) {
            Debug.LogError("Game Object " + hasHPsGameObject + " does not have a component that implements IHasProgress!");
        }

        hasHp.OnHpChanged += HasProgress_OnProgressChanged;

        barImage.fillAmount = 1f;

        Show();
    }
    private void Update(){
        barImage.transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back, mainCamera.transform.rotation * Vector3.up);
        barImageBG.transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back, mainCamera.transform.rotation * Vector3.up);
    //    transform.Rotate(0,180,0);
    }

    private void HasProgress_OnProgressChanged(object sender, IHasHpBar.OnHpChangedEventArgs e) {
        Debug.Log("HP bar process: " + e.HpNormalized);
        barImage.fillAmount = e.HpNormalized;

        if (e.HpNormalized >= 0f && e.HpNormalized <= 1f) {
            Show();
            
        } else {
            Hide();
            
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
