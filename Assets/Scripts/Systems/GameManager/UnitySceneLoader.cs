using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitySceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    void Start()
    {
        Debug.Log("Scene Started. Invoking...");
        GameObject.FindGameObjectWithTag("EventManager")?.GetComponent<EventManager>().unitySceneChanged.Invoke();
        GameObject.FindGameObjectWithTag("SceneManager")?.GetComponent<SceneManager>().SetSceneByIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
