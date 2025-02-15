using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumScene : Scene
{
    public AudioSource musicAudioSource;
    public AudioClip bgm;
    public float targetVolume = 0.35f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Setup()
    {
        EventManager.Instance.showInventory.Invoke(false);
        Debug.Log("The scene MUSEUM has been setup.");
    }

    public override void OnShow()
    {
        musicAudioSource.volume = 0;
        musicAudioSource.clip = bgm;
        DG.Tweening.Sequence seq = DOTween.Sequence();
        musicAudioSource.Play();
        seq.Append(DOTween.To(() => musicAudioSource.volume, x => musicAudioSource.volume = x, targetVolume, 4f));
    }
}
