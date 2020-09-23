using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("Player speed")]
    public float acceleration = 2f;
    public float decceleration = 1f;
    public float maxWalkingVelocity = 0.5f;
    public float maxRunningVelocity = 2f;
    public float idleThreshold = 0.1f;

    [Header("Mouse settings")]
    public float horizontalMouseSensitivity = 1f;
    public float verticalMouseSensitivity = 1f;

    private Animator animator;
    private Rigidbody rb;
    private Vector2 playerMoveDirection = Vector2.zero;
    private float velocityX = 0f;
    private float velocityZ = 0f;
    private bool isRunning = false;
    private float maxVelocity;
    /*
    
    */
    private void capVelocity()
    {
        if (Mathf.Abs(velocityX) < idleThreshold)
            velocityX = 0f;
        if (Mathf.Abs(velocityZ) < idleThreshold)
            velocityZ = 0f;
        if (Mathf.Abs(velocityX) > maxVelocity)
            velocityX = Mathf.Sign(velocityX) * maxVelocity;
        if (Mathf.Abs(velocityZ) > maxVelocity)
            velocityZ = Mathf.Sign(velocityZ) * maxVelocity;
    } // function to cap the velocity inside the boundaries
    private void changeVelocity()
    {
        if (playerMoveDirection.x != 0)
            velocityX += playerMoveDirection.x * acceleration * Time.deltaTime;
        else
            velocityX -= Mathf.Sign(velocityX) * decceleration * Time.deltaTime;

        if (playerMoveDirection.y != 0)
        {
            velocityZ += playerMoveDirection.y * acceleration * Time.deltaTime;
            velocityZ = Mathf.Max(velocityZ, -maxWalkingVelocity);
        }
        else
            velocityZ -= Mathf.Sign(velocityZ) * decceleration * Time.deltaTime;
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);
        playerMoveDirection = Input.GetAxis("Horizontal") * Vector2.right + Input.GetAxis("Vertical") * Vector2.up;

        //updateAimDownSights();
        animator.SetFloat("VelocityX", velocityX);
        animator.SetFloat("VelocityZ", velocityZ);
        transform.LookAt(transform.position + new Vector3(Camera.main.transform.forward.x,0f, Camera.main.transform.forward.z), Vector3.up);
    }

    private void FixedUpdate()
    {
        maxVelocity = (isRunning && velocityZ >= -0.2f) ? maxRunningVelocity : maxWalkingVelocity; // maxVelocity set with respect to isRunning
        changeVelocity();
        capVelocity();
        rb.velocity = Vector3.ClampMagnitude(transform.right * velocityX + transform.forward * velocityZ,maxVelocity) + rb.velocity.y * Vector3.up;
    }
}
