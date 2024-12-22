using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private FirstPersonController playerReference;
    [SerializeField] private GameObject playerJointReference;
    [SerializeField] private dialogueMesh dialogueMeshPrefab;
    private Dialogue currentDialogue;
    void Start()
    {
        
    }
    // Update is called once per frame

    void Start_dialogue(Dialogue dialogue){
        DialogueLine firstComponent = dialogue.getFirstComponent();
        dialogueMesh dm = Instantiate(dialogueMeshPrefab,
        playerJointReference.transform.position+ Camera.main.transform.rotation* firstComponent.getPosition(),
        Quaternion.LookRotation( playerJointReference.transform.position+ Camera.main.transform.rotation* firstComponent.getPosition() - playerJointReference.transform.position ));
        dm.textReference.text = firstComponent.getLine();
    }

    void Continue_dialogue(DialogueLine dialogueLine){
        DialogueLine newComponent = dialogueLine.next;
        dialogueMesh dm = Instantiate(dialogueMeshPrefab,
        playerJointReference.transform.position+ Camera.main.transform.rotation* newComponent.getPosition(),
        Quaternion.LookRotation( playerJointReference.transform.position+ Camera.main.transform.rotation* newComponent.getPosition() - playerJointReference.transform.position ));
        dm.textReference.text = newComponent.getLine();
    }
}
