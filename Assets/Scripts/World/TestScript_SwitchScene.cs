using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestScript_SwitchScene : IInteractable
{
    public Scene scene;
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
        _sm.SetScene(scene, isRelative);
    }

}
