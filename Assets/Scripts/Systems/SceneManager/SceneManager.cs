using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    public FirstPersonController player;
    public List<Scene> scenes;

    [SerializeField]
    private Scene _currentScene;

    private void Awake()
    {
        //scenes = new List<Scene>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScene(Scene scene, bool isRelative = false)
    {
        Scene cscene = _currentScene;

        if (_currentScene != null)
        {
            _currentScene.Hide();
        }
        _currentScene = scene;
        _currentScene.Show();
        if(isRelative)
            _currentScene.TeleportToRelative(player, cscene);
        else
            _currentScene.TeleportTo(player);
    }

    public void SetSceneByIndex(int i, bool isRelative = false)
    {
        if(i < 0 || i >= scenes.Count)
        {
            Debug.LogError("Scene index " + i + " out of bounds");
            return;
        }

        Scene cscene = _currentScene;

        if (_currentScene != null)
        {
            _currentScene.Hide();
        }
        _currentScene = scenes[i];
        _currentScene.Show();
        if (isRelative)
            _currentScene.TeleportToRelative(player, cscene);
        else
            _currentScene.TeleportTo(player);
    }
}