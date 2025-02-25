using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;

public class Scene : MonoBehaviour
{
    public Transform spawnPoint;

    private void Awake()
    {
        Setup();
        Debug.Log("Hiding Scene on Startup");
        this.Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Override this method to setup the scene. This is called in Awake.
    virtual public void Setup()
    {
        
    }

    public void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    virtual public void OnShow()
    {

    }

    virtual public void OnHide()
    {

    }

    public void Hide()
    {
        gameObject.SetActive(false);
        OnHide();
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    // Teleport the player to the entry point of the scene
    public void TeleportTo(FirstPersonController player)
    {
        if (!isActiveAndEnabled) return;

        player.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
    }

    /* 
     * Teleport the player to the spawn point of the scene, but take into account the position of the player
     * in the current scene. Useful for scenes that have similar spawn points and are supposed to be contiguous.
     * E.g. variations of the same scene
     */
    public void TeleportToRelative(FirstPersonController player, Scene scene)
    {
        if (!isActiveAndEnabled) return;

        Transform currentSceneTransform = scene.GetSpawnPoint();
        Vector3 relativePosition = player.transform.position - scene.GetSpawnPoint().position;
        Quaternion relativeRotation = player.transform.rotation * Quaternion.Inverse(scene.GetSpawnPoint().rotation);

        player.transform.position = spawnPoint.position + relativePosition;
        player.transform.rotation = spawnPoint.rotation * relativeRotation;
    }
}
