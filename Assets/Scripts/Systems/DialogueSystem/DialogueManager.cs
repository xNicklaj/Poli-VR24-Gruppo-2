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
    [SerializeField] private Dialogue debugDialogue;
    void Start()
    {
        
    }
    // Update is called once per frame

    void Update(){
        if(Input.GetButtonDown("Jump")){
            Start_dialogue(debugDialogue);
        }
    }
    void Start_dialogue(Dialogue dialogue){
        currentDialogue = dialogue;
        DialogueLine firstComponent = currentDialogue.getFirstComponent();
        Vector3 linePosition = new Vector3();
        if (firstComponent.positionRelativeToPlayer){
            linePosition = playerJointReference.transform.position+ Camera.main.transform.rotation* firstComponent.getPosition();
        }
        else{
            linePosition = firstComponent.getPosition();
        }
        dialogueMesh dm = Instantiate(dialogueMeshPrefab,
        linePosition,
        Quaternion.LookRotation( linePosition - playerJointReference.transform.position ));
        dm.textReference.text = firstComponent.getLine();
        dm.lineLight.color = firstComponent.getLightColor();
        dm.textReference.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, firstComponent.getLightColor());
    }

    void Continue_dialogue(DialogueLine dialogueLine){
        DialogueLine newComponent = dialogueLine.next;
        Vector3 linePosition = new Vector3();
        if (newComponent.positionRelativeToPlayer){
            linePosition = playerJointReference.transform.position+ Camera.main.transform.rotation* newComponent.getPosition();
        }
        else{
            linePosition = newComponent.getPosition();
        }
        dialogueMesh dm = Instantiate(dialogueMeshPrefab,
        linePosition,
        Quaternion.LookRotation( linePosition - playerJointReference.transform.position ));
        dm.textReference.text = newComponent.getLine();
        dm.lineLight.color = newComponent.getLightColor();
        dm.textReference.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, newComponent.getLightColor());
    }
}
