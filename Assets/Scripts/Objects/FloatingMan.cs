using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class FloatingMan : IInteractable
{
    [SerializeField] private AudioClip transitionAudio;
    public GameObject playerReference;
    public Scene scene;
    public Volume postProcessingVolume;
    public bool isRelative;

    private SceneManager _sm;


    // Start is called before the first frame update
    void Start()
    {
        _sm = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        postProcessingVolume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
        playerReference.GetComponent<FirstPersonController>().playerState=FirstPersonController.PlayerStates.IDLE;
        isSelectable=false;
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(()=>AudioSource.PlayClipAtPoint(transitionAudio,playerReference.transform.position));
        seq.Join(DOTween.To(()=>colorAdjustments.postExposure.value, x=> colorAdjustments.postExposure.value=x,10f,4.5f));
        seq.AppendInterval(1f);
        seq.AppendCallback(()=>_sm.SetScene(scene, isRelative));
        seq.Append(DOTween.To(()=>colorAdjustments.postExposure.value, x=> colorAdjustments.postExposure.value=x,0f,3.5f));
        seq.AppendCallback(()=>playerReference.GetComponent<FirstPersonController>().playerState=FirstPersonController.PlayerStates.MOVE);
        GameManager.Instance.eventFlags.SetFlag(EventFlag.MuseumEntered,true);
        
    }

}
