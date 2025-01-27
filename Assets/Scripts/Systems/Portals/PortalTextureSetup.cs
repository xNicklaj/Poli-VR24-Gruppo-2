using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera cameraHouse;
    public Camera cameraVoid;

    public Material cameraMaterialHouse;
    public Material cameraMaterialVoid;

    void Start()
    {
        if(cameraHouse.targetTexture != null){
            cameraHouse.targetTexture.Release();
        }
        cameraHouse.targetTexture = new RenderTexture(Screen.width,Screen.height,24);
        cameraMaterialHouse.mainTexture = cameraHouse.targetTexture;

        if(cameraVoid.targetTexture != null){
            cameraVoid.targetTexture.Release();
        }
        cameraVoid.targetTexture = new RenderTexture(Screen.width,Screen.height,24);
        cameraMaterialVoid.mainTexture = cameraVoid.targetTexture;
    }
}
