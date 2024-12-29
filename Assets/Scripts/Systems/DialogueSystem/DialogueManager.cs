using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private FirstPersonController playerReference;
    [SerializeField] private GameObject playerJointReference;
    [SerializeField] private DialogueMesh dialogueMeshPrefab;
    [SerializeField] private UnityEvent<string> dialogueEnded;
    [SerializeField] private Dialogue debugDialogue;

    private Dialogue currentDialogue;
    private DialogueLine currentLine;
    private readonly List<DialogueMesh> dialogueMeshes = new List<DialogueMesh>();

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            StartDialogue(debugDialogue);
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
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
        dialogueEnded.Invoke(currentDialogue.name);
        currentDialogue = null;

        foreach (var dialogueMesh in dialogueMeshes)
        {
            Destroy(dialogueMesh.gameObject);
        }

        dialogueMeshes.Clear();
    }

    private void DisplayLine(DialogueLine dialogueLine)
    {
        Vector3 linePosition = dialogueLine.positionRelativeToPlayer
            ? playerJointReference.transform.position + Camera.main.transform.rotation * dialogueLine.getPosition()
            : dialogueLine.getPosition();

        var dialogueMesh = Instantiate(dialogueMeshPrefab, linePosition, Quaternion.LookRotation(linePosition - playerJointReference.transform.position));
        dialogueMeshes.Add(dialogueMesh);

        SetupDialogueMesh(dialogueMesh, dialogueLine);
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
        AdjustCollider(dialogueMesh);
        ApplyLightingAndAudio(dialogueMesh, dialogueLine.getLightColor(), dialogueLine.GetAudio());
    }

    private void SetupDialogueAnswerMesh(DialogueMesh dialogueMesh, DialogueAnswer answer)
    {
        dialogueMesh.dialogueManager = this;
        dialogueMesh.textReference.text = answer.getAnswerText();
        dialogueMesh.dc = answer;
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

    private void ApplyLightingAndAudio(DialogueMesh dialogueMesh, Color? lightColor, AudioSource audioSource)
    {
        if (lightColor.HasValue)
        {
            dialogueMesh.lineLight.color = lightColor.Value;
            var particles = dialogueMesh.particles.main;
            particles.startColor = lightColor.Value;
        }

        if (audioSource != null)
        {
            dialogueMesh.audioSource = audioSource;
            dialogueMesh.audioSource.Play();
        }
    }
}
