using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject playerJointReference;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump")){
            GameObject go = new GameObject();
            go.transform.position = playerJointReference.transform.position+ Camera.main.transform.rotation* new Vector3(10,0,10);
            go.transform.rotation = Quaternion.LookRotation( go.transform.position - playerJointReference.transform.position );
            TextMeshPro text = go.AddComponent<TextMeshPro>();
            text.text = "prova";
            

        }
    }
}
