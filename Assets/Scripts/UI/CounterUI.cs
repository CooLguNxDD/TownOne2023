using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class CounterUI : MonoBehaviour
{
    // Start is called before the first frame update


    public TextMeshProUGUI SwordCounter;
    public TextMeshProUGUI ShieldCounter;
    public TextMeshProUGUI MinionCount;

    // Update is called once per frame
    void Update()
    {
        SwordCounter.text = GameManager.Instance.SwordCount.ToString();
        ShieldCounter.text = GameManager.Instance.ShieldCount.ToString();
        MinionCount.text = GameManager.Instance.MinionCount.ToString();
    }
}
