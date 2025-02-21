using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using GLTFast.Schema;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class VoidScene : MonoBehaviour
{
    public Scene MuseumScene;
    [SerializeField] private GameObject playerReference;
    [SerializeField] private Volume volumeReference;
    [SerializeField] private GameObject canvasReference;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject dome;
    [SerializeField] private Light domeSpotlight;
    [Header("Presets")]
    [SerializeField] private GameObject nameInputFieldPreset;
    [SerializeField] private CanvasGroup InventoryCanvasGroup;

    [SerializeField] private GameObject matchBoxPreset;
    [SerializeField] private GameObject candlePreset;
    [SerializeField] private GameObject CandleFather;
    [SerializeField] private GameObject SeedPreset;
    private GameObject seedInstance;
    [SerializeField] private GameObject StoneDoorVoid;
    [SerializeField] private PortalTeleporter StoneDoorVoidTeleporter;
    [SerializeField] private GameObject StoneDoorVoidCollisionsOnly;
    [SerializeField] private AudioSource StoneDoorVoidAudioSource;
    [SerializeField] private GameObject StoneDoorHouse;
    [SerializeField] private PortalTeleporter StoneDoorHouseTeleporter;

    [SerializeField] private GameObject TripletPreset;
    [SerializeField] private GameObject TripletFather;
    [SerializeField] private GameObject DNAPrefab;
    [SerializeField] private AudioClip DNAAudio;
    private GameObject DNAInstance;
    [SerializeField] private GameObject EyesClosedPrefab;
    [SerializeField] private GameObject EyesClosedTextPrefab;
    [SerializeField] private GameObject FloatingManPrefab;
    [SerializeField] private GameObject bonsai;
    [SerializeField] private Cuckoo cuckoo;


    private GameObject FloatingManInstance;
    [Header("Color Parameters")]
    [SerializeField][ColorUsage(false)] private Color totalBlackRoomColor;
    [SerializeField][ColorUsage(false)] private Color totalBlackRoomColorCaustics;

    [SerializeField][ColorUsage(false)] private Color blackRoomColor;
    [SerializeField][ColorUsage(false)] private Color blackRoomColorCaustics;

    [SerializeField][ColorUsage(false)] private Color whiteRoomColor;
    [SerializeField][ColorUsage(false)] private Color whiteRoomColorCaustics;

    [SerializeField][ColorUsage(false)] private Color redRoomColor;
    [SerializeField][ColorUsage(false)] private Color redRoomColorCaustics;

    [Header("Audio Parameters")]
    [SerializeField] private GameObject gameSounds;
    [SerializeField] private AudioClip windSoundtrack;
    [SerializeField] private AudioClip colorChangeSound;
    [SerializeField] private AudioClip godKilledSound;
    [SerializeField] private AudioClip godKilledSoundtrack;
    [SerializeField] private AudioClip fluteSound;




    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.flagHasBeenSet.AddListener(inventorySetFlagSorting);
        floor.GetComponent<Renderer>().material.SetColor("_BaseColor", blackRoomColor);
        floor.GetComponent<Renderer>().material.SetColor("_CausticsColor", blackRoomColorCaustics);
        dome.GetComponent<Renderer>().material.color = blackRoomColor;
        loadAmbient();

    }
    public void DialogueEndedFunctions(string dialogueName)
    {
        switch (dialogueName)
        {
            case "Intro Dialogue 1":
                showNameInput();
                break;
            case "Intro Dialogue 3":
                showNameInput();
                break;
            case "Intro Dialogue 4":
                DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/God Dialogue/God Dialogue 1"));
                break;
            case "God Dialogue 1":
                changeColor();
                break;
            case "God Dialogue 3" or "God Dialogue 4":
                showGodDialogue();
                break;
            case "God Dialogue 6":
                getSilent();
                break;
            case "Self Dialogue":
                StartSeedDialogue();
                break;
            case "Seed Dialogue 1":
                MakeSeedAppear();
                break;
            case "Seed Dialogue 2":
                MakeSeedSelectable();
                break;
            case "Tree Dialogue 5" or "Tree Dialogue 6":
                GameManager.Instance.eventFlags.SetFlag(EventFlag.TreeDialogueEnded,true);
                EventManager.Instance.saveRequested.Invoke();
                break;
            case "Naked Dialogue 1":
                MakeDNAAppear();
                break;
            case "Naked Dialogue 2":
                MakeHelixAppear();
                break;
            case "Naked Dialogue 3":
                GameManager.Instance.eventFlags.SetFlag(EventFlag.NakedDialogueEnded,true);
                EventManager.Instance.saveRequested.Invoke();
                EventManager.Instance.showInventory.Invoke(false);
                closeEyes();
                break;


        }
    }
    private void showNameInput()
    {
        GameObject nameInputSystem = Instantiate(nameInputFieldPreset, Vector2.zero, new Quaternion(), canvasReference.transform);
        nameInputSystem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        nameInputSystem.GetComponent<NameInputSystem>().player = playerReference;
        nameInputSystem.GetComponent<NameInputSystem>().Appear();
    }
    private void changeColor()
    {
        gameSounds.GetComponent<AudioSource>().clip = colorChangeSound;
        gameSounds.GetComponent<AudioSource>().Play();
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(floor.GetComponent<Renderer>().material.DOColor(whiteRoomColor, "_BaseColor", 3.5f));
        sequence.Join(floor.GetComponent<Renderer>().material.DOColor(whiteRoomColorCaustics, "_CausticsColor", 3.5f));
        sequence.Join(dome.GetComponent<Renderer>().material.DOColor(whiteRoomColor, 3.5f));
        sequence.Join(GetComponent<AudioSource>().DOFade(0, 3.5f));
        sequence.Join(domeSpotlight.DOIntensity(200, 3.5f));
        sequence.AppendCallback(() => DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/God Dialogue/God Dialogue 2")));
    }

    private void showGodDialogue()
    {
        GetComponent<AudioSource>().volume = 0f;
        gameSounds.GetComponent<AudioSource>().clip = godKilledSound;
        gameSounds.GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().clip = godKilledSoundtrack;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(floor.GetComponent<Renderer>().material.DOColor(redRoomColor, "_BaseColor", 3.5f));
        sequence.Join(floor.GetComponent<Renderer>().material.DOColor(redRoomColorCaustics, "_CausticsColor", 3.5f));
        sequence.Join(dome.GetComponent<Renderer>().material.DOColor(redRoomColor, 3.5f));
        sequence.Join(domeSpotlight.DOColor(redRoomColor, 3.5f));
        sequence.JoinCallback(() => GetComponent<AudioSource>().Play());
        sequence.Join(GetComponent<AudioSource>().DOFade(1f, 4f));
        sequence.AppendCallback(() => DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/God Dialogue/God Dialogue 6")));
    }
    private void getSilent()
    {
        gameSounds.GetComponent<AudioSource>().clip = fluteSound;
        gameSounds.GetComponent<AudioSource>().Play();
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(floor.GetComponent<Renderer>().material.DOColor(totalBlackRoomColor, "_BaseColor", 3.5f));
        sequence.Join(floor.GetComponent<Renderer>().material.DOColor(totalBlackRoomColorCaustics, "_CausticsColor", 3.5f));
        sequence.Join(dome.GetComponent<Renderer>().material.DOColor(totalBlackRoomColor, 6f));
        sequence.Join(GetComponent<AudioSource>().DOFade(0f, 4f));
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() => throwMatchBox());
    }
    private void throwMatchBox()
    {
        Vector3 matchBoxPosition = playerReference.transform.position - playerReference.transform.forward * 3f + playerReference.transform.up * 3f;
        var matchBoxInstance = Instantiate(matchBoxPreset, matchBoxPosition, new Quaternion());
        matchBoxInstance.GetComponent<Rigidbody>().AddForce((playerReference.transform.forward + playerReference.transform.up) * 20f);
    }

    private void inventorySetFlagSorting(EventFlag e, bool status)
    {
        if ((e == EventFlag.HasLighter) && (status == true))
        {
            startSelfDialogue(new Vector3(0f, 0f, 1f).normalized * 7, "Self 1");
        }
        if ((e == EventFlag.HasSeed) && (status == true))
        {
            ShowPortal();
        }
    }
    private void ShowPortal()
    {
        bonsai.GetComponent<BonsaiPot>().state=BonsaiPot.plantState.PLANTED;
        Vector3 rot = new Vector3(-playerReference.transform.forward.x, StoneDoorVoidCollisionsOnly.transform.position.y, -playerReference.transform.forward.z);
        StoneDoorVoidCollisionsOnly.transform.LookAt(rot);
        StoneDoorVoid.transform.transform.LookAt(rot);
        Vector3 pos = new Vector3(playerReference.transform.position.x, 0f, playerReference.transform.position.z);
        Vector3 posNoY = new Vector3(playerReference.transform.position.x, StoneDoorVoid.transform.position.y, playerReference.transform.position.z);
        StoneDoorVoidCollisionsOnly.transform.position = pos;
        StoneDoorVoid.transform.position = posNoY;
        StoneDoorVoidAudioSource.Play();
        StoneDoorVoid.transform.DOMoveY(0, 1f).OnComplete(() => Destroy(StoneDoorVoidCollisionsOnly.gameObject));
        StoneDoorHouse.transform.position = new Vector3(StoneDoorHouse.transform.position.x, 0, StoneDoorHouse.transform.position.z);
        bonsai.GetComponent<BonsaiPot>().state = BonsaiPot.plantState.NOT_PLANTED;
        GetComponent<AudioSource>().clip= windSoundtrack;
        GetComponent<AudioSource>().DOFade(0.5f, 4f);
    }
    public void startSelfDialogue(Vector3 pos, string lineName)
    {
        GameObject candleInstance = Instantiate(candlePreset, pos, new Quaternion(), CandleFather.transform);
        candleInstance.GetComponent<Candle>().dialogueLine = Resources.Load<DialogueLine>("Dialogues/Self Dialogue/" + lineName);
        candleInstance.GetComponent<Candle>().setVoidScene(this);
    }
    public void destroyCandles()
    {
        foreach (Transform candle in CandleFather.transform)
        {
            Destroy(candle.gameObject);
        }
    }
    private void StartSeedDialogue()
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(2.5f);
        sequence.AppendCallback(() => DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/Seed Dialogue/Seed Dialogue 1")));
    }
    private void MakeSeedAppear()
    {
        seedInstance = Instantiate(SeedPreset, new Vector3(-15f, 2f, 0f), new Quaternion());
        DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/Seed Dialogue/Seed Dialogue 2"));

    }
    private void MakeSeedSelectable()
    {
        seedInstance.GetComponent<seed>().particles.Play();
        seedInstance.GetComponent<ActionSetFlag>().isSelectable = true;
    }
    private void MakeDNAAppear()
    {
        DOTween.SetTweensCapacity(1024, 50);
        float rMin = 5f;
        float rMax = 20f;
        for (int i = 0; i < 1000; i++)
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            float randomSquare = UnityEngine.Random.Range(rMin * rMin, rMax * rMax);
            float r = Mathf.Sqrt(randomSquare);
            float x = r * Mathf.Cos(angle);
            float y = UnityEngine.Random.Range(1f, 15f);
            float z = r * Mathf.Sin(angle);
            Vector3 spawnLocation = new Vector3(x, y, z);
            GameObject tripletInstance = Instantiate(TripletPreset, spawnLocation, new Quaternion(), TripletFather.transform);
            tripletInstance.transform.LookAt(new Vector3(0, spawnLocation.y, 0));
            Triplet triplet = tripletInstance.GetComponent<Triplet>();
            triplet.textMeshPro.alpha = 0;
            triplet.textMeshPro.DOFade(1, UnityEngine.Random.Range(3f, 7f));
        }
        DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/Naked Dialogue/Naked Dialogue 2"));
    }
    private void MakeHelixAppear()
    {
        DNAInstance = Instantiate(DNAPrefab, new Vector3(0, -30, 0), new Quaternion());
        foreach (Transform mesh in TripletFather.transform)
        {
            mesh.DOMove(new Vector3(0, 0, 0), 7f);
            mesh.GetComponent<Triplet>().textMeshPro.DOFade(0, 7f);
            mesh.DOScale(0, 7f);
        }
        floor.GetComponent<Renderer>().material.DOColor(redRoomColor, "_BaseColor", 7f);
        floor.GetComponent<Renderer>().material.DOColor(redRoomColorCaustics, "_CausticsColor", 7f);
        AudioSource.PlayClipAtPoint(DNAAudio,Vector3.zero,0.5f);
        DNAInstance.transform.DOMoveY(0, 15);
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(7f);
        sequence.AppendCallback(() => Destroy(TripletFather.gameObject));
        sequence.AppendCallback(() => DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/Naked Dialogue/Naked Dialogue 3")));
    }
    private void closeEyes()
    {
        playerReference.GetComponent<FirstPersonController>().playerState = FirstPersonController.PlayerStates.IDLE;
        GameObject eyesClosed = Instantiate(EyesClosedPrefab, canvasReference.transform);
        eyesClosed.GetComponent<CanvasGroup>().alpha = 0;
        eyesClosed.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        GameObject eyesClosedText = Instantiate(EyesClosedTextPrefab, canvasReference.transform);
        eyesClosedText.GetComponent<CanvasGroup>().alpha = 0;
        eyesClosedText.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        GetComponent<AudioSource>().DOFade(0,1.5f);
        DG.Tweening.Sequence seq = DOTween.Sequence();
        seq.Append(eyesClosed.GetComponent<CanvasGroup>().DOFade(0.6f, 0.75f));
        seq.Append(eyesClosed.GetComponent<CanvasGroup>().DOFade(0.2f, 1f));
        seq.Append(eyesClosed.GetComponent<CanvasGroup>().DOFade(0.8f, 0.5f));
        seq.Append(eyesClosed.GetComponent<CanvasGroup>().DOFade(0.6f, 1f));
        seq.Append(eyesClosed.GetComponent<CanvasGroup>().DOFade(1f, 0.75f));
        if (DNAInstance)
        {
            seq.AppendCallback(() => Destroy(DNAInstance));
        }
        seq.AppendInterval(3.5f);
        seq.Join(InventoryCanvasGroup.DOFade(0,1.5f));
        seq.AppendCallback(() => eyesClosedText.GetComponent<TextMeshProUGUI>().SetText("Cosa vedi?"));
        seq.Append(eyesClosedText.GetComponent<CanvasGroup>().DOFade(1f, 0.75f));
        seq.AppendInterval(2f);
        seq.Append(eyesClosedText.GetComponent<CanvasGroup>().DOFade(0f, 0.75f));
        seq.AppendInterval(2f);
        seq.AppendCallback(() => eyesClosedText.GetComponent<TextMeshProUGUI>().SetText("Dimmi, ora, cosa vedi?"));
        seq.Append(eyesClosedText.GetComponent<CanvasGroup>().DOFade(1f, 0.75f));
        seq.AppendInterval(2f);
        seq.Append(eyesClosedText.GetComponent<CanvasGroup>().DOFade(0f, 0.75f));
        seq.AppendInterval(2f);
        seq.AppendCallback(() => eyesClosedText.GetComponent<TextMeshProUGUI>().SetText("Chi sei?"));
        seq.Append(eyesClosedText.GetComponent<CanvasGroup>().DOFade(1f, 0.75f));
        seq.AppendInterval(2f);
        seq.Append(eyesClosedText.GetComponent<CanvasGroup>().DOFade(0f, 0.75f));
        seq.AppendInterval(2f);
        seq.Append(floor.GetComponent<Renderer>().material.DOColor(totalBlackRoomColor, "_BaseColor", 3.5f));
        seq.Join(floor.GetComponent<Renderer>().material.DOColor(totalBlackRoomColorCaustics, "_CausticsColor", 3.5f));
        seq.Join(dome.GetComponent<Renderer>().material.DOColor(totalBlackRoomColor, 3.5f));
        seq.JoinCallback(() => domeSpotlight.gameObject.SetActive(false));
        seq.AppendCallback(() => FloatingManInstance = Instantiate(FloatingManPrefab, new Vector3(0f, 0f, 6f), new Quaternion(0f, -180f, 0f, 1)));
        seq.AppendCallback(() => playerReference.GetComponent<FirstPersonController>().transform.position = new Vector3(0f, playerReference.GetComponent<FirstPersonController>().transform.position.y, -24f));
        seq.AppendCallback(() => playerReference.GetComponent<FirstPersonController>().transform.LookAt(new Vector3(0f, playerReference.GetComponent<FirstPersonController>().transform.position.y, 0f)));
        seq.Append(eyesClosed.GetComponent<CanvasGroup>().DOFade(0f, 4f));
        seq.AppendCallback(() => playerReference.GetComponent<FirstPersonController>().playerState = FirstPersonController.PlayerStates.MOVE);
        seq.AppendCallback(() => FloatingManInstance.GetComponent<FloatingMan>().scene = MuseumScene);
        seq.AppendCallback(() => FloatingManInstance.GetComponent<FloatingMan>().postProcessingVolume = volumeReference);
        seq.AppendCallback(() => FloatingManInstance.GetComponent<FloatingMan>().playerReference = playerReference)

        .OnComplete(() => Destroy(eyesClosed.gameObject)).OnComplete(() => Destroy(eyesClosedText.gameObject));

    }
    private void loadAmbient()
    {
        

        if (CheckFlag(EventFlag.HasLighter) && !CheckFlag(EventFlag.HasSeed) && !CheckFlag(EventFlag.HasWateringCan))
        {
            floor.GetComponent<Renderer>().material.SetColor("_BaseColor", totalBlackRoomColor);
            floor.GetComponent<Renderer>().material.SetColor("_CausticsColor", totalBlackRoomColorCaustics);
            dome.GetComponent<Renderer>().material.color = totalBlackRoomColor;
            GetComponent<AudioSource>().volume = 0f;
            DG.Tweening.Sequence introsequence = DOTween.Sequence();
            introsequence.AppendInterval(4f);
            introsequence.AppendCallback(() => startSelfDialogue(new Vector3(0f, 0f, 1f).normalized * 7, "Self 1"));
        }
        else if (CheckFlag(EventFlag.HasSeed) && !CheckFlag(EventFlag.HasWateringCan))
        {
            floor.GetComponent<Renderer>().material.SetColor("_BaseColor", totalBlackRoomColor);
            floor.GetComponent<Renderer>().material.SetColor("_CausticsColor", totalBlackRoomColorCaustics);
            dome.GetComponent<Renderer>().material.color = totalBlackRoomColor;
            GetComponent<AudioSource>().volume = 0f;
            DG.Tweening.Sequence introsequence = DOTween.Sequence();
            introsequence.AppendInterval(4f);
            introsequence.AppendCallback(() => ShowPortal());
        }
        else if (CheckFlag(EventFlag.HasWateringCan) && !CheckFlag(EventFlag.TreeDialogueEnded))
        {
            print("can "+CheckFlag(EventFlag.HasWateringCan));
            print("treeDialogue: "+CheckFlag(EventFlag.TreeDialogueEnded));
            floor.GetComponent<Renderer>().material.SetColor("_BaseColor", totalBlackRoomColor);
            floor.GetComponent<Renderer>().material.SetColor("_CausticsColor", totalBlackRoomColorCaustics);
            dome.GetComponent<Renderer>().material.color = totalBlackRoomColor;
            GetComponent<AudioSource>().volume = 0f;
            bonsai.GetComponent<BonsaiPot>().state=BonsaiPot.plantState.PLANTED;
            bonsai.GetComponent<BonsaiPot>().particlePivot.SetActive(false);
            cuckoo.isSelectable=false;
            GameManager.Instance.eventFlags.SetFlag(EventFlag.HasSeed,false);
            
            playerReference.transform.position = new Vector3(0f,0f,70f);
            StoneDoorVoidTeleporter.toggleActive();
            StoneDoorHouseTeleporter.toggleActive();
        }
        else if (CheckFlag(EventFlag.TreeDialogueEnded) && !CheckFlag(EventFlag.NakedDialogueEnded))
        {
            floor.GetComponent<Renderer>().material.SetColor("_BaseColor", totalBlackRoomColor);
            floor.GetComponent<Renderer>().material.SetColor("_CausticsColor", totalBlackRoomColorCaustics);
            dome.GetComponent<Renderer>().material.color = totalBlackRoomColor;
            GetComponent<AudioSource>().volume = 0f;
            bonsai.GetComponent<BonsaiPot>().state=BonsaiPot.plantState.GROWN;
            DG.Tweening.Sequence introsequence = DOTween.Sequence();
            introsequence.AppendInterval(4f);
            bonsai.GetComponent<BonsaiPot>().particlePivot.SetActive(false);
            bonsai.GetComponent<BonsaiPot>().setGrown();
            cuckoo.isSelectable=false;
            introsequence.AppendCallback(() => DialogueManager.Instance.dialogueEnded.Invoke("Tree Dialogue 6"));
            playerReference.transform.position = new Vector3(0f,0f,70f);
            StoneDoorVoidTeleporter.toggleActive();
            StoneDoorHouseTeleporter.toggleActive();
        }
        else if (CheckFlag(EventFlag.NakedDialogueEnded) && !CheckFlag(EventFlag.MuseumEntered))
        {
            floor.GetComponent<Renderer>().material.SetColor("_BaseColor", totalBlackRoomColor);
            floor.GetComponent<Renderer>().material.SetColor("_CausticsColor", totalBlackRoomColorCaustics);
            dome.GetComponent<Renderer>().material.color = totalBlackRoomColor;
            GetComponent<AudioSource>().volume = 0f;
            bonsai.GetComponent<BonsaiPot>().state=BonsaiPot.plantState.GROWN;
            bonsai.GetComponent<BonsaiPot>().particlePivot.SetActive(false);
            DG.Tweening.Sequence introsequence = DOTween.Sequence();
            introsequence.AppendInterval(4f);
            introsequence.AppendCallback(()=>DialogueManager.Instance.dialogueEnded.Invoke("Naked Dialogue 3"));
        }
        else if (CheckFlag(EventFlag.MuseumEntered) && !CheckFlag(EventFlag.MuseumExited))
        {
            InventoryCanvasGroup.alpha=0f;
            floor.GetComponent<Renderer>().material.SetColor("_BaseColor", totalBlackRoomColor);
            floor.GetComponent<Renderer>().material.SetColor("_CausticsColor", totalBlackRoomColorCaustics);
            dome.GetComponent<Renderer>().material.color = totalBlackRoomColor;
            GetComponent<AudioSource>().volume = 0f;
            bonsai.GetComponent<BonsaiPot>().state=BonsaiPot.plantState.GROWN;
            bonsai.GetComponent<BonsaiPot>().particlePivot.SetActive(false);
            DG.Tweening.Sequence introsequence = DOTween.Sequence();
            introsequence.AppendCallback(()=>SceneManager.Instance.SetScene(MuseumScene, false));
            introsequence.AppendInterval(4f);
        }
        else if (CheckFlag(EventFlag.MuseumEntered) && CheckFlag(EventFlag.MuseumExited))
        {
            InventoryCanvasGroup.alpha=0f;
            floor.GetComponent<Renderer>().material.SetColor("_BaseColor", totalBlackRoomColor);
            floor.GetComponent<Renderer>().material.SetColor("_CausticsColor", totalBlackRoomColorCaustics);
            dome.GetComponent<Renderer>().material.color = totalBlackRoomColor;
            GetComponent<AudioSource>().volume = 0f;
            bonsai.GetComponent<BonsaiPot>().state=BonsaiPot.plantState.GROWN;
            bonsai.GetComponent<BonsaiPot>().particlePivot.SetActive(false);
            DG.Tweening.Sequence introsequence = DOTween.Sequence();
            introsequence.AppendCallback(()=>SceneManager.Instance.SetScene(MuseumScene, false));
            introsequence.AppendInterval(4f);
        }
        else
        {
            DG.Tweening.Sequence introsequence = DOTween.Sequence();
            introsequence.AppendInterval(4f);
            introsequence.AppendCallback(() => DialogueManager.Instance.StartDialogue(Resources.Load<Dialogue>("Dialogues/Intro Dialogue/Intro Dialogue 1")));
        }
    }
    private bool CheckFlag(EventFlag e)
    {
        return GameManager.Instance.eventFlags.GetFlag(e);
    }
}
