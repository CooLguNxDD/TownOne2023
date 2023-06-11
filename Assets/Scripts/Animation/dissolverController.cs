using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
using System;

public class dissolverController : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    private PlayerUnitController Controller;

    [SerializeField]
    public event EventHandler OnDeathAnimationEnded;


    [SerializeField]
    private VisualEffect effect;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private MeshRenderer MeshRenderer;
    private List<Material> materials;

    private void Awake()
    {
        materials = new List<Material>();
    }
    void Start()
    {
        Controller.OnDeathAnimation += playAnimation;

        foreach (Material material in MeshRenderer.materials){
            materials.Add(material);
        }
        effect.Stop();

    }

    // Update is called once per frame
    private void playAnimation(object sender, System.EventArgs e){
        // Debug.Log("start!");
        StartCoroutine(StartDissolve(sender));
    }


    IEnumerator StartDissolve(object sender)
    {
        string DissolveRef = "_DissolveAmount";

        // if (animator != null)
        // {
        //     animator.SetBool("dead", true);
        // }

        // yield return new WaitForSeconds(2f);

        if (effect != null)
            effect.Play();

        yield return new WaitForSeconds(0.5f);

        foreach (var material in materials)
        {
            material.DOFloat(1f, DissolveRef, 2f);
        }

        yield return new WaitForSeconds(2f);

        //reset materials
        foreach (var material in materials)
        {
            material.SetFloat(DissolveRef, 0f);
        }
        effect.Stop();
        OnDeathAnimationEnded?.Invoke(this, EventArgs.Empty);
        yield return null;
    }
}
