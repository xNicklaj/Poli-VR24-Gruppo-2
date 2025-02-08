using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private FirstPersonController playerReference;
    [SerializeField] private GameObject playerJointReference;
    [SerializeField] private DialogueMesh dialogueMeshPrefab;
    [SerializeField] private Dialogue debugDialogue;
    private Dictionary<string, Func<string>> format_data;
    private Dialogue currentDialogue;
    private DialogueLine currentLine;
    private readonly List<DialogueMesh> dialogueMeshes = new List<DialogueMesh>();
    public UnityEvent<string> dialogueEnded;
    public void Start()
    {
        format_data = new Dictionary<string, Func<string>>
        {
            { "player_name", ()=> GameManager.Instance.player_name }
        };
    }
    public void StartDialogue(Dialogue dialogue)
    {

        //_em = EventManager.Instance;
        currentDialogue = dialogue;
        currentLine = currentDialogue.getFirstComponent();
        DisplayLine(currentLine);
    }

    public void ContinueDialogue(DialogueComponent dialogueComponent)
    {
        DisableDialogueMeshes();

        if (dialogueComponent.next == null)
        {
            if (!currentLine.hasAnswers())
            {
                EndDialogue();
                return;
            }

            CreateDialogueAnswers();
            return;
        }

        currentLine = dialogueComponent.next;
        DisplayLine(currentLine);
    }

    public void EndDialogue()
    {
        string dialogueName = null;
        if (currentDialogue != null)
        {
            dialogueName = currentDialogue.name;
        }
        currentDialogue = null;

        foreach (var dialogueMesh in dialogueMeshes)
        {
            DestroyLine(dialogueMesh);
        }

        dialogueMeshes.Clear();
        DialogueFollowUp(dialogueName);
    }

    public DialogueMesh DisplayLine(DialogueLine dialogueLine)
    {
        Vector3 linePosition = dialogueLine.positionRelativeToPlayer
            ? playerJointReference.transform.position + Camera.main.transform.rotation * dialogueLine.getPosition()
            : dialogueLine.getPosition();

        var dialogueMesh = Instantiate(dialogueMeshPrefab, linePosition, Quaternion.LookRotation(linePosition - playerJointReference.transform.position));
        dialogueMeshes.Add(dialogueMesh);

        SetupDialogueMesh(dialogueMesh, dialogueLine);
        dialogueMesh.lineLight.intensity = 0f;
        dialogueMesh.textReference.alpha = 0f;
        dialogueMesh.isSelectable = false;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(dialogueMesh.lineLight.DOIntensity(1f, 1.5f));
        sequence.Join(dialogueMesh.textReference.DOFade(1f, 1.5f)).onComplete = dialogueMesh.toggleSelectability;
        return dialogueMesh;
    }
    public DialogueMesh DisplayCandleLine(DialogueLine dialogueLine, Vector3 position)
    {
        currentLine = dialogueLine;
        var dialogueMesh = Instantiate(dialogueMeshPrefab, position, Quaternion.LookRotation(position - playerJointReference.transform.position));
        dialogueMeshes.Add(dialogueMesh);
        SetupDialogueMesh(dialogueMesh, dialogueLine);
        dialogueMesh.lineLight.intensity = 0f;
        dialogueMesh.textReference.alpha = 0f;
        dialogueMesh.isSelectable = false;
        if (dialogueLine.name == "Self 9")
        {
            dialogueMesh.isSelectable = true;
            currentDialogue = Resources.Load<Dialogue>("Dialogues/Self Dialogue/Self Dialogue");
        }
        Sequence sequence = DOTween.Sequence();
        sequence.Append(dialogueMesh.lineLight.DOIntensity(1f, 1.5f));
        sequence.Join(dialogueMesh.textReference.DOFade(1f, 1.5f));
        return dialogueMesh;
    }
    public void DestroyLine(DialogueMesh dialogueMesh)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(dialogueMesh.lineLight.DOIntensity(0f, 1.5f));
        sequence.Join(dialogueMesh.textReference.DOFade(0f, 1.5f)).OnComplete(() => Destroy(dialogueMesh.gameObject));
    }

    private void CreateDialogueAnswers()
    {
        foreach (var answer in currentLine.getAnswers())
        {
            var position = answer.getPosition();
            var rotation = Quaternion.LookRotation(position - playerJointReference.transform.position);

            var dialogueAnswer = Instantiate(dialogueMeshPrefab, position, rotation);
            dialogueMeshes.Add(dialogueAnswer);

            SetupDialogueAnswerMesh(dialogueAnswer, answer);
        }
    }

    private void SetupDialogueMesh(DialogueMesh dialogueMesh, DialogueLine dialogueLine)
    {
        dialogueMesh.dialogueManager = this;
        dialogueMesh.dc = dialogueLine;
        dialogueMesh.textReference.text = FormatString(dialogueLine.getLine());
        dialogueMesh.textReference.fontSize=dialogueLine.getSize();
        dialogueMesh.fpc = playerReference;
        AdjustCollider(dialogueMesh);
        ApplyLightingAndAudio(dialogueMesh, dialogueLine.getLightColor(), dialogueLine.GetAudio());
    }

    private void SetupDialogueAnswerMesh(DialogueMesh dialogueMesh, DialogueAnswer answer)
    {
        dialogueMesh.dialogueManager = this;
        dialogueMesh.textReference.text = answer.getAnswerText();
        dialogueMesh.dc = answer;
        dialogueMesh.fpc = playerReference;
        AdjustCollider(dialogueMesh);
        ApplyLightingAndAudio(dialogueMesh, answer.getLightColor(), answer.GetAudio());
    }

    private void DisableDialogueMeshes()
    {
        foreach (var mesh in dialogueMeshes)
        {
            mesh.isSelectable = false;
        }
    }

    private void AdjustCollider(DialogueMesh dialogueMesh)
    {
        var collider = dialogueMesh.GetComponent<CapsuleCollider>();
        collider.height = dialogueMesh.textReference.preferredWidth;
        collider.radius = dialogueMesh.textReference.preferredHeight / 2;
    }

    private void ApplyLightingAndAudio(DialogueMesh dialogueMesh, Color? lightColor, AudioClip audioSource)
    {
        if (lightColor.HasValue)
        {
            dialogueMesh.lineLight.color = lightColor.Value;
            var particles = dialogueMesh.particles.main;
            particles.startColor = lightColor.Value;
        }

        if (audioSource != null)
        {
            dialogueMesh.audioSource.clip = audioSource;
            dialogueMesh.audioSource.Play();
        }
    }
    private string FormatString(string template)
    {
        foreach (var entry in format_data)
        {
            template = template.Replace($"{{{entry.Key}}}", entry.Value.Invoke());
        }
        return template;
    }

    public void DialogueFollowUp(string dialogueName)
    {
        print(dialogueName);
        dialogueEnded.Invoke(dialogueName);
        
    }

}
