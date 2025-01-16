using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitySceneLoader : MonoBehaviour
{
    public GameObject gameManager;

    // Start is called before the first frame update
    private void Awake()
    {
        GameObject go = GameObject.FindGameObjectWithTag("GameManager");
        if (go != null) return;

        Instantiate(gameManager);
    }

    void Start()
    {
        Debug.Log("Scene Started. Invoking...");
        EventManager.Instance?.unitySceneChanged.Invoke(false);
        SceneManager.Instance?.SetSceneByIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
