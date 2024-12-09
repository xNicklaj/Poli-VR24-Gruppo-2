using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Movement Parameters
    public bool playerCanMove = true;
    public float movementSpeed = 10.0f;
    public float maxVelocityChange = 10f;
    #endregion

    #region Camera Parameters
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float lookSpeed = 1.0f;
    public float maxLookAngle = 50.0f;
    public float fov = 60f;
    #endregion

    #region Refs

    public Camera camera;
    private Rigidbody rb;

    #endregion

    #region Input

    private PlayerInputActions pc;
    private InputAction move;
    private InputAction look;

    #endregion

    #region Internal State Variables
    [SerializeField]
    private bool isWalking = false;
    [SerializeField]
    private bool isGrounded = false;
    #endregion

    #region Buffers

    private Vector2 _moveInputVector;
    private Vector2 _lookInputVector;
    private Vector3 _3DVectorBuff;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    #endregion

    private void Awake()
    {
        pc = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = pc.Player.Move;
        look = pc.Player.Look;

        move.Enable();
        look.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camera.fieldOfView = fov;
    }

    // Update is called once per frame
    void Update()
    {
        _lookInputVector = look.ReadValue<Vector2>();

        if (cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + _lookInputVector.x * lookSpeed;

            if (!invertCamera)
            {
                pitch -= lookSpeed * _lookInputVector.y;
            }
            else
            {
                // Inverted Y
                pitch += lookSpeed * _lookInputVector.y;
            }

            // Clamp pitch between lookAngle
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            camera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        CheckGround();
    }

    void FixedUpdate()
    {
        if (playerCanMove)
        {
            // Calculate how fast we should be moving
            _moveInputVector = move.ReadValue<Vector2>();
            Debug.Log(_moveInputVector);
            Vector3 targetVelocity = new Vector3(_moveInputVector.x, 0, _moveInputVector.y);

            // Checks if player is walking and isGrounded
            // Will allow head bob
            if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            targetVelocity = transform.TransformDirection(targetVelocity) * movementSpeed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            Debug.Log("Velocity: " + velocityChange);
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    // Sets isGrounded based on a raycast sent straigth down from the player object
    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * 1f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = 2*.75f;

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
}
