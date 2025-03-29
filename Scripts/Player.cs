using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private GameInput gameInput;
    [SerializeField]private float movementSpeed = 5f;

    private void Update()
    {
        Vector2 inputVector2 = gameInput.GetInputVector2Normalized();

        Vector3 movementDir = new Vector3(inputVector2.x, 0, inputVector2.y);

        float movementDistance = movementSpeed * Time.deltaTime;

        transform.position += movementDir * movementDistance;
        
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movementDir, rotateSpeed * Time.deltaTime);
    }
}