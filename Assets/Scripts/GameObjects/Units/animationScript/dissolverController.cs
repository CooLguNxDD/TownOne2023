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
    private DemonKingUnitController DController;

    [SerializeField]
    public event EventHandler OnDeathAnimationEnded;


    [SerializeField]
    private VisualEffect effect;

    [SerializeField]
    private ParticleSystem deathEffect;

    [SerializeField]
    private Animator animator;


    [SerializeField]
    private MeshRenderer MeshRenderer;
    
     [SerializeField]
    private SkinnedMeshRenderer sMeshRenderer;
    private List<Material> materials;

    public enum selectController {DemonKing, Unit, Cage};

    public selectController newController;

    private void Awake()
    {
        materials = new List<Material>();
    }
    void Start()
    {
        if(newController == selectController.DemonKing){
            DController.OnDeathAnimation += playAnimation;
            foreach (Material material in sMeshRenderer.materials){
                materials.Add(material);
            }
        }
        else if(newController == selectController.Cage){
            DController.OnDeathAnimation += playAnimation;
            foreach (Material material in MeshRenderer.materials){
                materials.Add(material);
            }
        }
        else if(newController == selectController.Unit){
            Controller.OnDeathAnimation += playAnimation;
            foreach (Material material in MeshRenderer.materials){
                materials.Add(material);
            }
        }
        effect.Stop();
        deathEffect.Stop();

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
        deathEffect.Play();
        yield return new WaitForSeconds(0.5f);
        
        foreach (var material in materials)
        {
            material.DOFloat(1f, DissolveRef, 2f);
        }
        

        yield return new WaitForSeconds(2f);
        deathEffect.Stop();
        effect.Stop();

        //reset materials
        foreach (var material in materials)
        {
            material.SetFloat(DissolveRef, 0f);
        }


        OnDeathAnimationEnded?.Invoke(this, EventArgs.Empty);

        yield return null;
    }
}
