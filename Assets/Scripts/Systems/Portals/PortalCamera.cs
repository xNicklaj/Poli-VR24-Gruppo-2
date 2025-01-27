using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera;
    public Transform portal;
    public Transform otherPortal;
    [SerializeField] private bool arrival;

    // Update is called once per frame
    void LateUpdate()
    {
        //Vector3 playerOffsetFromPortal = playerCamera.position- otherPortal.position;
        Vector3 relativePosition = otherPortal.InverseTransformPoint(playerCamera.position);
        Vector3 pos = portal.TransformPoint(relativePosition);
        transform.position = pos;
        //transform.position = portal.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotation = Quaternion.Angle(portal.rotation, otherPortal.rotation);
        if (arrival){
            angularDifferenceBetweenPortalRotation*=-1;
        }
        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotation, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference*playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection,Vector3.up);
    }
}
