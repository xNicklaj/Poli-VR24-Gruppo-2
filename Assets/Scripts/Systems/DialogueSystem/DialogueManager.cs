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

    private Dialogue currentDialogue;
    private DialogueLine currentLine;
    private readonly List<DialogueMesh> dialogueMeshes = new List<DialogueMesh>();

    public UnityEvent inputNameEvent;

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            StartDialogue(debugDialogue);
        }
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

    private void EndDialogue()
    {
        string dialogueName = currentDialogue.name;
        currentDialogue = null;

        foreach (var dialogueMesh in dialogueMeshes)
        {
            Destroy(dialogueMesh.gameObject);
        }

        dialogueMeshes.Clear();
        DialogueFollowUp(dialogueName);
    }

    private void DisplayLine(DialogueLine dialogueLine)
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
        sequence.Append(dialogueMesh.lineLight.DOIntensity(1f, 2f));
        sequence.Join(dialogueMesh.textReference.DOFade(1f, 2f));
        sequence.AppendCallback(dialogueMesh.toggleSelectability);
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
        dialogueMesh.textReference.text = dialogueLine.getLine();
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

    public void DialogueFollowUp(string dialogueName){
        switch (dialogueName){
            case "Intro Dialogue 1":
                inputNameEvent.Invoke();
            break;
        }
    }
}
