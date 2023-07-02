using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CounterUI : MonoBehaviour
{
    // Start is called before the first frame update


    public TextMeshProUGUI SwordCounter;
    public TextMeshProUGUI ShieldCounter;
    public TextMeshProUGUI MinionCount;

    public TextMeshProUGUI RateCount;

    // Update is called once per frame
    void Update()
    {
        SwordCounter.text = GameManager.Instance.SwordCount.ToString();
        ShieldCounter.text = GameManager.Instance.ShieldCount.ToString();
        MinionCount.text = GameManager.Instance.MinionCount.ToString();
        float rate = MouseController.Instance.ClickingCoolDownSetting - GameManager.Instance.FB_Buff;
        RateCount.text = rate.ToString("0.00") + "S";
    }
}
