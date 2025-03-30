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

    [SerializeField] private float movementSpeed = 0f;
    [SerializeField] private float rotateSpeed = 10f;

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

        Vector3 movementDir = new Vector3(inputVector2.x, 0, inputVector2.y);
        float movementDistance = movementSpeed * Time.deltaTime;

        float decreaseSpeed = 4;
        float increaseRunningSpeed = 8;
        float increaseWalkingSpeed = 6;
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


        bool moveFront = movementDir.x != 0;
        bool moveForward = movementDir.z >= 0;
        if (moveFront&&moveForward)
        {
            transform.forward = Vector3.Slerp(transform.forward, movementDir, rotateSpeed * Time.deltaTime);
        }
        if (moveForward)
        {
            transform.forward = Vector3.Slerp(transform.forward, movementDir, rotateSpeed * Time.deltaTime);
        }
    }
}