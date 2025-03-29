using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float movementSpeed;
    private int velocityHash;

    private void Start()
    {
        Player.Instance.WakingToRunning += InstanceOnWakingToRunning;

        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Speed");
    }

    private void InstanceOnWakingToRunning(object sender, Player.WakingToRunningEventArges e)
    {
        movementSpeed = e.playerSpeed;
    }

    private void Update()
    {
        animator.SetFloat(velocityHash, movementSpeed);
    }
}