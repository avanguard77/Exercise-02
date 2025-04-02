using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public event EventHandler<WakingToRunningEventArges> WakingToRunning;

    public class WakingToRunningEventArges : EventArgs
    {
        public float playerSpeed;
    }

    [Header("Movement Settings")] [SerializeField]
    private GameInput gameInput;

    [SerializeField] private Camera playerCam;
    [SerializeField] private float movementSpeed = 0f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] float increaseRunningSpeed = 8;
    [SerializeField] float increaseWalkingSpeed = 6;

    [Header("Camera Setting")] [SerializeField]
    Transform CameraTransform;
    [SerializeField] float decreaseSpeed = 4;
   
    
    private bool isWalking;
    private bool isShiftHold;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.ShiftSpeedStart += InstanceOnShiftSpeedStart;
        GameInput.Instance.ShiftSpeedEnd += InstanceOnShiftSpeedEnd;
    }

    private void InstanceOnShiftSpeedEnd(object sender, EventArgs e)
    {
        isShiftHold = false;
    }

    private void InstanceOnShiftSpeedStart(object sender, EventArgs e)
    {
        isShiftHold = true;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector2 = gameInput.GetInputVector2Normalized();
        gameInput.GetInputMouseNormalized();

        Vector3 camForwardXZ = new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z);
        Vector3 camRightXZ = new Vector3(playerCam.transform.right.x, 0, playerCam.transform.right.z);

        Vector3 movementDir = camRightXZ * inputVector2.x + camForwardXZ * inputVector2.y;

        float movementDistance = movementSpeed * Time.deltaTime;

        isWalking = movementDir != Vector3.zero;
        if (isWalking)
        {
            if (!isShiftHold)
            {
                float maxWalkingSpeed = 2;
                if (movementSpeed < maxWalkingSpeed)
                {
                    movementSpeed += Time.deltaTime * increaseWalkingSpeed;
                }
                else if (movementSpeed >= maxWalkingSpeed)
                {
                    movementSpeed -= Time.deltaTime * decreaseSpeed;
                }
            }
            else
            {
                float maxRunningSpeed = 6;

                if (movementSpeed <= maxRunningSpeed)
                {
                    movementSpeed += Time.deltaTime;
                }
            }
        }
        else
        {
            if (movementSpeed >= 0)
            {
                movementSpeed -= Time.deltaTime * decreaseSpeed;
            }
        }
        WakingToRunning?.Invoke(this, new WakingToRunningEventArges { playerSpeed = movementSpeed });
        transform.position += movementDir * movementDistance;

        transform.forward = Vector3.Slerp(transform.forward, movementDir, rotateSpeed * Time.deltaTime);
    }
}