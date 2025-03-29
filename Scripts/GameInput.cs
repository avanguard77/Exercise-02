using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    PlayerInputAction playerInputAction;

    private void Start()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
        
    }

    public Vector2 GetInputVector2Normalized()
    {
        Vector2 input= playerInputAction.Player.Move.ReadValue<Vector2>();
        input.Normalize();
        return input;
    }
}
