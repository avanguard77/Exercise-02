using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event EventHandler<WakingToRunningEventArges> WakingToRunning;

    public class WakingToRunningEventArges : EventArgs
    {
        public float playerSpeed;
    }

    [Header("Movement Settings")] [SerializeField]
    private GameInput gameInput;

    [SerializeField] private float movementSpeed = 0f;

    private bool isWalking;
    private bool isShiftHold;

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

        

        isWalking = movementDir != Vector3.zero;
        if (isWalking)
        {
            if (isShiftHold  && movementSpeed <= 6)
            {
                movementSpeed += Time.deltaTime;
            }
        }
        else
        {
            if (movementSpeed >= 1)
            {
                float dicreaseMoveSpeed = 2 * Time.deltaTime;
                movementSpeed -= dicreaseMoveSpeed;
            }
        }
        
        Debug.Log(movementSpeed);
        
        WakingToRunning?.Invoke(this, new WakingToRunningEventArges { playerSpeed = movementSpeed });
        transform.position += movementDir * movementDistance;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movementDir, rotateSpeed * Time.deltaTime);
    }
}