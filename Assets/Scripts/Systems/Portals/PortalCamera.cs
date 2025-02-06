using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera;
    public Transform portal;
    public Transform fence;

    public Transform otherPortal;
    public Transform otherFence;

    [SerializeField] private bool arrival;

    private Camera playerCam;
    private Camera portalCam;

    void Start()
    {
        playerCam = playerCamera.GetComponent<Camera>();
        portalCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Sincronizza il FOV della camera del portale con quello della camera del giocatore
        portalCam.fieldOfView = playerCam.fieldOfView;

        Vector3 relativePosition = otherPortal.InverseTransformPoint(playerCamera.position);
        Vector3 pos = portal.TransformPoint(relativePosition);
        transform.position = pos;

        float angularDifferenceBetweenPortalRotation = Vector3.Angle(fence.forward, otherFence.forward);
        if (arrival)
        {
            print("rotazione di " + fence + " è " + fence.rotation);
            print("rotazione di " + otherFence + " è " + otherFence.rotation);
            print("rotazione di " + otherPortal + " è " + otherPortal.rotation);

            angularDifferenceBetweenPortalRotation *= -1;
        }
        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotation, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
