using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MuseumDoors : IInteractable
{
    public Scene scene;
    public Volume postProcessingVolume;
    public bool isRelative;
    private SceneManager _sm;
    public GameObject playerReference;
    [SerializeField] private GameObject museumExitText;
    [SerializeField] private Canvas canvasReference;


    [SerializeField] private AudioClip transitionAudio;
    [SerializeField] private AudioClip outroSong;


    [SerializeField] private InGameUIManager inGameUIManagerReference;
    void Start()
    {
        _sm = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
    }
    public override void Interact()
    {
        inGameUIManagerReference.ExitMuseum();
    }
    public void backToVoid(){
        
        inGameUIManagerReference.ResumeGame();
        postProcessingVolume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
        playerReference.GetComponent<FirstPersonController>().playerState=FirstPersonController.PlayerStates.IDLE;
        isSelectable=false;
        
        GameObject t =Instantiate(museumExitText,canvasReference.transform);
        TextMeshProUGUI tmp = t.GetComponent<TextMeshProUGUI>();
        CanvasGroup tcg = t.GetComponent<CanvasGroup>();
        tcg.alpha=0;
        
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(()=>AudioSource.PlayClipAtPoint(transitionAudio,playerReference.transform.position));
        seq.Join(DOTween.To(()=>colorAdjustments.postExposure.value, x=> colorAdjustments.postExposure.value=x,15f,5f));
        seq.AppendInterval(1f);
        seq.AppendCallback(()=>playerReference.GetComponent<AudioSource>().clip=outroSong);
        seq.AppendCallback(()=>playerReference.GetComponent<AudioSource>().Play());
        
        SequenceText(seq,"[...]Ora dobbiamo dare un nome al nuovo replicatore, un nome che dia l'idea di un'unità di trasmissione culturale o un'unità di imitazione.[...]",tmp,tcg);
        SequenceText(seq,"[...]«Mimeme» deriva da una radice greca che sarebbe adatta, ma io preferirei un bisillabo dal suono affine a «gene».[...]",tmp,tcg);
        SequenceText(seq,"[...]Lo si potrebbe considerare correlato a «memoria» o alla parola francese même.[...]",tmp,tcg);
        SequenceText(seq,"[...]Esempi di memi sono melodie, idee, frasi, mode, modi di modellare vasi o costruire archi.[...]",tmp,tcg);
        SequenceText(seq,"[...]I memi si propagano nel pool memico saltando di cervello in cervello tramite un processo che, in senso lato, si può chiamare imitazione.[...]",tmp,tcg);
        SequenceText(seq,"L'hai vista lì dentro, non è vero?\nLa potenza che avete voi esseri umani?\nNon ti tremano le mani all'idea di potere influenzare gli altri?",tmp,tcg);
        SequenceText(seq,"E se gli altri sono umani quanto te, cosa puoi fare per loro?\nNon lo vuoi avere anche tu un posto nel mondo?",tmp,tcg);
        SequenceText(seq,"Non vuoi provare senso di appartenenza?\nNon lo volete un po'tutti?",tmp,tcg);
        SequenceText(seq,"Cosa vuoi dare al prossimo?\nDove finisce il tuo odio?\n Dove finisce il tuo egoismo?",tmp,tcg);
        SequenceText(seq,"Quanto è grande la tua bolla?\nQuanto è grande quella degli altri?",tmp,tcg);
        SequenceText(seq,"Sai cosa c'è nella bolla degli altri?\nQuanto odio?\nQuanta rabbia?\nQuanta tristezza?\nQuanta felicità?",tmp,tcg);
        seq.AppendCallback(()=>_sm.SetScene(scene, isRelative));
        seq.AppendCallback(()=>playerReference.transform.position=new Vector3(1.5f,2f,78f));
        seq.AppendCallback(()=>playerReference.transform.LookAt(new Vector3(1.5f,2f,79f)));
        seq.Append(DOTween.To(()=>colorAdjustments.postExposure.value, x=> colorAdjustments.postExposure.value=x,0f,5f));
        seq.JoinCallback(()=>tmp.color=Color.white);
        seq.AppendCallback(()=>tmp.fontSize=64);
        seq.AppendInterval(7f);
        SequenceText(seq,"Va'.",tmp,tcg);
        SequenceText(seq,"Sii una tela bianca.\nLascia che la gente ti dipinga.",tmp,tcg);
        SequenceText(seq,"Sii una persona libera.\nSii un essere umano.",tmp,tcg);
        SequenceText(seq,"Vivi, "+GameManager.Instance.player_name+", e vivi al massimo.",tmp,tcg);
        seq.Append(DOTween.To(()=>colorAdjustments.postExposure.value, x=> colorAdjustments.postExposure.value=x,10f,5f));
        seq.AppendCallback(()=>GameManager.Instance.eventFlags.SetFlag(EventFlag.MuseumExited,true));
        seq.AppendCallback(()=>EventManager.Instance.saveRequested.Invoke());
        seq.OnComplete(()=>GameManager.Instance.QuitGame());
    }

    private void SequenceText(Sequence s, string text, TextMeshProUGUI t, CanvasGroup cg){
        s.AppendCallback(()=>t.SetText(text));
        s.Append(cg.DOFade(1f,0.75f));
        s.AppendInterval(4f);
        s.Append(cg.DOFade(0f,0.75f));
        s.AppendInterval(0.5f);
    }

}
