// CHANGE LOG
// 
// CHANGES || version VERSION
//
// "Enable/Disable Headbob, Changed look rotations - should result in reduced camera jitters" || version 1.0.1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;

using TMPro;
using UnityEngine.EventSystems;





#if UNITY_EDITOR
using UnityEditor;
using System.Net;
#endif

public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;

    #region Animation
    public Animator anim;
    #endregion

    #region Camera Movement Variables

    public Camera playerCamera;
    public float FOV = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float gamepadSensitivity = 4f;
    public float maxLookAngle = 50f;

    // Crosshair
    public bool lockCursor = true;
    public bool crosshair = true;
    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;

    // Hint
    public GameObject hintGameObject;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;

    #region Camera Zoom Variables

    public bool enableZoom = true;
    public bool holdToZoom = false;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;

    // Internal Variables
    private bool isZoomed = false;

    #endregion
    #endregion

    #region Movement Variables
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    #region Sprint Variables

    public bool enableSprint = true;
    public float sprintSpeed = 7f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    #endregion

    #region Jump Variables

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;

    // Internal Variables
    private bool isGrounded = false;

    #endregion

    #region Crouch Variables

    public bool enableCrouch = true;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    // Internal Variables
    private bool isCrouched = false;
    private Vector3 originalScale;

    #endregion
    #endregion

    #region Input Variables

    public PlayerInputActions pc;
    private InputAction move;
    private InputAction look;
    private InputAction sprint;
    private InputAction zoom;
    private Vector2 _moveInputVector;
    private Vector2 _lookInputVector;

    #endregion

    #region Head Bob Variables

    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 5f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    // Internal Variables
    private Vector3 jointOriginalPos;
    private float timer = 0;

    #endregion

    #region Finite State Machine
    public enum PlayerStates
    {
        MOVE,
        SPRINT,
        JUMP,
        CROUCH,
        IDLE // used when player can't move
    }
    public PlayerStates playerState;
    #region Audio
    public AudioSource feetAudioSource;



    #endregion
    #endregion
    #region Awake Function
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pc = new PlayerInputActions();

        crosshairObject = GetComponentInChildren<Image>();

        // Set internal variables
        playerCamera.fieldOfView = FOV;
        originalScale = transform.localScale;
        jointOriginalPos = joint.localPosition;
    }
    #endregion
    #region OnEnable Function
    private void OnEnable()
    {
        move = pc.Player.Move;
        look = pc.Player.Look;
        sprint = pc.Player.Sprint;
        zoom = pc.Player.Zoom;

        move.Enable();
        look.Enable();
        sprint.Enable();
        zoom.Enable();
    }
    #endregion
    #region OnDisable Function

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        sprint.Disable();
        zoom.Disable();
    }
    #endregion
    #region Start Function
    void Start()
    {
        
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (crosshair)
        {
            crosshairObject.sprite = crosshairImage;
            crosshairObject.color = crosshairColor;
        }
        else
        {
            crosshairObject.gameObject.SetActive(false);
        }

        crosshairObject.gameObject.SetActive(false);

    }
    #endregion
    #region Update Function
    private void Update()
    {
        if (GameManager.Instance.IsGamePaused()) return;
        switch (playerState)
        {
            case PlayerStates.MOVE:
                //Moving function is in FixedState

                //playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, FOV, sprintFOVStepTime * Time.deltaTime);
                MoveCamera();
                CheckGround();
                HeadBob();
                if (sprint.IsPressed())
                {
                    playerState = PlayerStates.SPRINT;
                }
                if (Input.GetKeyDown(jumpKey)){
                    Jump();
                }
                break;
            case PlayerStates.SPRINT:
                if (rb.linearVelocity != Vector3.zero)
                {
                    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);
                }
                else
                {
                    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, FOV, sprintFOVStepTime * Time.deltaTime);
                }

                MoveCamera();
                CheckGround();
                HeadBob();
                if (sprint.WasReleasedThisFrame())
                {
                    playerState = PlayerStates.MOVE;
                }
                break;
            case PlayerStates.JUMP:
                MoveCamera();
                Jump();
                CheckGround();
                break;
            case PlayerStates.CROUCH:
                MoveCamera();
                Crouch();
                CheckGround();
                HeadBob();
                break;
            case PlayerStates.IDLE:
                CheckGround();
                HeadBob();
                break;
        }
    }
    #endregion
    #region FixedUpdate Function
    void FixedUpdate()
    {
        if (anim != null) anim.SetBool("IsWalking", false);
        if (anim != null) anim.SetBool("IsRunning", false);
        switch (playerState)
        {
            case PlayerStates.MOVE:
                Move();
                break;
            case PlayerStates.SPRINT:
                Sprint();
                break;
        }
    }
    #endregion
    #region MoveCamera Function
    private void MoveCamera()
    {

        // Control camera movement
        if (cameraCanMove)
        {
            _lookInputVector = look.ReadValue<Vector2>();

            var sensitivity = GameManager.Instance.currentDevice == GameManager.DeviceType.Keyboard ? mouseSensitivity : gamepadSensitivity;

            yaw = transform.localEulerAngles.y + _lookInputVector.x * sensitivity;
            


            if (!invertCamera)
            {
                pitch -= sensitivity * _lookInputVector.y;
            }
            else
            {
                // Inverted Y
                pitch += sensitivity * _lookInputVector.y;
            }

            // Clamp pitch between lookAngle
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        #region Camera Zoom

        if (enableZoom)
        {
            // Changes isZoomed when key is pressed
            // Behavior for toggle zoom
            if (zoom.IsPressed() && !holdToZoom && !(playerState == PlayerStates.SPRINT))
            {
                if (!isZoomed)
                {
                    isZoomed = true;
                }
                else
                {
                    isZoomed = false;
                }
            }

            // Changes isZoomed when key is pressed
            // Behavior for hold to zoom
            if (holdToZoom && !(playerState == PlayerStates.SPRINT))
            {
                if (zoom.IsPressed())
                {
                    isZoomed = true;
                }
                else
                {
                    isZoomed = false;
                }
            }

            // Lerps camera.fieldOfView to allow for a smooth transistion
            if (isZoomed)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            }
            else if (!isZoomed && !(playerState == PlayerStates.SPRINT))
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, FOV, zoomStepTime * Time.deltaTime);
            }
        }

        #endregion
        #endregion
    }

    private void Sprint()
    {
        #region Sprint

        if (enableSprint)
        {
            if (sprint.IsPressed())
            {
                _moveInputVector = move.ReadValue<Vector2>();
                Vector3 targetVelocity = new Vector3(_moveInputVector.x, 0, _moveInputVector.y);
                isZoomed = false;

                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.linearVelocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                if (anim != null && velocity != Vector3.zero)
                {
                    anim.SetBool("IsRunning", true);
                    anim.SetBool("IsWalking", true);
                } 

                // Player is only moving when valocity change != 0
                // Makes sure fov change only happens during movement
                rb.AddForce(velocityChange, ForceMode.VelocityChange);


            }else{
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsRunning", false);
            }
        }

        #endregion
    }
    private void Move()
    {
        #region Movement
        // Calculate how fast we should be moving
        _moveInputVector = move.ReadValue<Vector2>();
        Vector3 targetVelocity = new Vector3(_moveInputVector.x, 0, _moveInputVector.y);
        targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rb.linearVelocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        if (anim != null && velocity != Vector3.zero) {
            anim.SetBool("IsWalking", true);
            anim.SetBool("IsRunning", false);
        }else if (velocity != Vector3.zero){
        }
        rb.AddForce(velocityChange, ForceMode.VelocityChange);



        #endregion
    }
    // Sets isGrounded based on a raycast sent straigth down from the player object
    #region CheckGround Function
    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = 5 * .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    #endregion
    #region Jump Function
    private void Jump()
    {
        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;

        }

    }
    #endregion
    #region Crouch Function
    private void Crouch()
    {
        if (enableCrouch)
        {
            if (Input.GetKeyDown(crouchKey))
            {
                if (isCrouched)
                {
                    transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
                    walkSpeed /= speedReduction;

                    isCrouched = false;
                }
                else
                {
                    transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
                    walkSpeed *= speedReduction;

                    isCrouched = true;
                }
            }
        }

    }
    #endregion
    #region HeadBob Function
    private void HeadBob()
    {
        if (enableHeadBob)
        {
            switch (playerState)
            {
                case PlayerStates.MOVE or PlayerStates.IDLE:
                    if (rb.linearVelocity != Vector3.zero && pc.Player.Move.IsPressed())
                    {
                        timer += Time.deltaTime * bobSpeed;
                    }
                    else
                    {
                        timer += Time.deltaTime * bobSpeed * speedReduction * 0.2f;
                        anim.SetBool("IsWalking", false);
                        anim.SetBool("IsRunning", false);
                    }
                    break;
                case PlayerStates.SPRINT:
                    if (rb.linearVelocity != Vector3.zero)
                    {
                        timer += Time.deltaTime * (bobSpeed + sprintSpeed);
                    }
                    else
                    {
                        timer += Time.deltaTime * bobSpeed * speedReduction * 0.2f;
                    }
                    break;
                case PlayerStates.CROUCH:
                    if (rb.linearVelocity != Vector3.zero)
                    {
                        timer += Time.deltaTime * (bobSpeed * speedReduction);
                    }
                    else
                    {
                        timer += Time.deltaTime * bobSpeed * speedReduction * 0.2f;
                    }
                    break;

            }

            // Applies HeadBob movement
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            // Resets when play stops moving
            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }
    }
    #endregion

    #region Crosshair Interaction
    public void DisplayCrosshair()
    {
        crosshairObject.gameObject.SetActive(true);
        hintGameObject.SetActive(true);
    }

    public void HideCrosshair()
    {
        crosshairObject.gameObject.SetActive(false);
        hintGameObject.SetActive(false);
    }
    #endregion
}

/*
// Custom Editor
#if UNITY_EDITOR
[CustomEditor(typeof(FirstPersonController)), InitializeOnLoadAttribute]
public class FirstPersonControllerEditor : Editor
{
    FirstPersonController fpc;
    SerializedObject SerFPC;

    private void OnEnable()
    {
        fpc = (FirstPersonController)target;
        SerFPC = new SerializedObject(fpc);
    }

    public override void OnInspectorGUI()
    {
        SerFPC.Update();

        #region Camera Setup

        GUILayout.Label("Camera Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        fpc.playerCamera = (Camera)EditorGUILayout.ObjectField(new GUIContent("Camera", "Camera attached to the controller."), fpc.playerCamera, typeof(Camera), true);
        fpc.fov = EditorGUILayout.Slider(new GUIContent("Field of View", "The camera’s view angle. Changes the player camera directly."), fpc.fov, fpc.zoomFOV, 179f);
        fpc.cameraCanMove = EditorGUILayout.ToggleLeft(new GUIContent("Enable Camera Rotation", "Determines if the camera is allowed to move."), fpc.cameraCanMove);

        GUI.enabled = fpc.cameraCanMove;
        fpc.invertCamera = EditorGUILayout.ToggleLeft(new GUIContent("Invert Camera Rotation", "Inverts the up and down movement of the camera."), fpc.invertCamera);
        fpc.mouseSensitivity = EditorGUILayout.Slider(new GUIContent("Look Sensitivity", "Determines how sensitive the mouse movement is."), fpc.mouseSensitivity, .1f, 10f);
        fpc.maxLookAngle = EditorGUILayout.Slider(new GUIContent("Max Look Angle", "Determines the max and min angle the player camera is able to look."), fpc.maxLookAngle, 40, 90);
        GUI.enabled = true;

        fpc.lockCursor = EditorGUILayout.ToggleLeft(new GUIContent("Lock and Hide Cursor", "Turns off the cursor visibility and locks it to the middle of the screen."), fpc.lockCursor);

        fpc.crosshair = EditorGUILayout.ToggleLeft(new GUIContent("Auto Crosshair", "Determines if the basic crosshair will be turned on, and sets is to the center of the screen."), fpc.crosshair);

        // Only displays crosshair options if crosshair is enabled
        if (fpc.crosshair)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Crosshair Image", "Sprite to use as the crosshair."));
            fpc.crosshairImage = (Sprite)EditorGUILayout.ObjectField(fpc.crosshairImage, typeof(Sprite), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            fpc.crosshairColor = EditorGUILayout.ColorField(new GUIContent("Crosshair Color", "Determines the color of the crosshair."), fpc.crosshairColor);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        #region Camera Zoom Setup

        GUILayout.Label("Zoom", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));

        fpc.enableZoom = EditorGUILayout.ToggleLeft(new GUIContent("Enable Zoom", "Determines if the player is able to zoom in while playing."), fpc.enableZoom);

        GUI.enabled = fpc.enableZoom;
        fpc.holdToZoom = EditorGUILayout.ToggleLeft(new GUIContent("Hold to Zoom", "Requires the player to hold the zoom key instead if pressing to zoom and unzoom."), fpc.holdToZoom);
        fpc.zoomFOV = EditorGUILayout.Slider(new GUIContent("Zoom FOV", "Determines the field of view the camera zooms to."), fpc.zoomFOV, .1f, fpc.fov);
        fpc.zoomStepTime = EditorGUILayout.Slider(new GUIContent("Step Time", "Determines how fast the FOV transitions while zooming in."), fpc.zoomStepTime, .1f, 10f);
        GUI.enabled = true;

        #endregion

        #endregion

        #region Movement Setup

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Movement Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        fpc.playerCanMove = EditorGUILayout.ToggleLeft(new GUIContent("Enable Player Movement", "Determines if the player is allowed to move."), fpc.playerCanMove);

        GUI.enabled = fpc.playerCanMove;
        fpc.walkSpeed = EditorGUILayout.Slider(new GUIContent("Walk Speed", "Determines how fast the player will move while walking."), fpc.walkSpeed, .1f, fpc.sprintSpeed);
        GUI.enabled = true;

        EditorGUILayout.Space();

        #region Sprint

        GUILayout.Label("Sprint", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));

        fpc.enableSprint = EditorGUILayout.ToggleLeft(new GUIContent("Enable Sprint", "Determines if the player is allowed to sprint."), fpc.enableSprint);

        GUI.enabled = fpc.enableSprint;
        fpc.sprintSpeed = EditorGUILayout.Slider(new GUIContent("Sprint Speed", "Determines how fast the player will move while sprinting."), fpc.sprintSpeed, fpc.walkSpeed, 20f);


        fpc.sprintFOV = EditorGUILayout.Slider(new GUIContent("Sprint FOV", "Determines the field of view the camera changes to while sprinting."), fpc.sprintFOV, fpc.fov, 179f);
        fpc.sprintFOVStepTime = EditorGUILayout.Slider(new GUIContent("Step Time", "Determines how fast the FOV transitions while sprinting."), fpc.sprintFOVStepTime, .1f, 20f);

        GUI.enabled = true;

        EditorGUILayout.Space();

        #endregion

        #region Jump

        GUILayout.Label("Jump", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));

        fpc.enableJump = EditorGUILayout.ToggleLeft(new GUIContent("Enable Jump", "Determines if the player is allowed to jump."), fpc.enableJump);

        GUI.enabled = fpc.enableJump;
        fpc.jumpKey = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Jump Key", "Determines what key is used to jump."), fpc.jumpKey);
        fpc.jumpPower = EditorGUILayout.Slider(new GUIContent("Jump Power", "Determines how high the player will jump."), fpc.jumpPower, .1f, 20f);
        GUI.enabled = true;

        EditorGUILayout.Space();

        #endregion

        #region Crouch

        GUILayout.Label("Crouch", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));

        fpc.enableCrouch = EditorGUILayout.ToggleLeft(new GUIContent("Enable Crouch", "Determines if the player is allowed to crouch."), fpc.enableCrouch);

        GUI.enabled = fpc.enableCrouch;
        fpc.crouchKey = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Crouch Key", "Determines what key is used to crouch."), fpc.crouchKey);
        fpc.crouchHeight = EditorGUILayout.Slider(new GUIContent("Crouch Height", "Determines the y scale of the player object when crouched."), fpc.crouchHeight, .1f, 1);
        fpc.speedReduction = EditorGUILayout.Slider(new GUIContent("Speed Reduction", "Determines the percent 'Walk Speed' is reduced by. 1 being no reduction, and .5 being half."), fpc.speedReduction, .1f, 1);
        GUI.enabled = true;

        #endregion

        #endregion

        #region Head Bob

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Head Bob Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        fpc.enableHeadBob = EditorGUILayout.ToggleLeft(new GUIContent("Enable Head Bob", "Determines if the camera will bob while the player is walking."), fpc.enableHeadBob);


        GUI.enabled = fpc.enableHeadBob;
        fpc.joint = (Transform)EditorGUILayout.ObjectField(new GUIContent("Camera Joint", "Joint object position is moved while head bob is active."), fpc.joint, typeof(Transform), true);
        fpc.bobSpeed = EditorGUILayout.Slider(new GUIContent("Speed", "Determines how often a bob rotation is completed."), fpc.bobSpeed, 1, 20);
        fpc.bobAmount = EditorGUILayout.Vector3Field(new GUIContent("Bob Amount", "Determines the amount the joint moves in both directions on every axes."), fpc.bobAmount);
        GUI.enabled = true;

        #endregion

        //Sets any changes from the prefab
        if (GUI.changed)
        {
            EditorUtility.SetDirty(fpc);
            Undo.RecordObject(fpc, "FPC Change");
            SerFPC.ApplyModifiedProperties();
        }
    }

}

#endif
*/