using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float timeToSwitchShoulder = 0.5f;
    public GameObject pivot;
    [Header("Player speed")]
    public float acceleration = 2f;
    public float maxWalkingVelocity = 0.5f;
    public float maxRunningVelocity = 2f;
    public float idleThreshold = 0.1f;
    public float friction = 0.9f;
    public Vector3 pivotOffsetFromPlayer = new Vector3(0.3f, 1.8f, 0.0f);

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
    private bool isCameraOverRightShoulder = true;
    private Vector3 switchShoulderVelocity;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);
        playerMoveDirection = (Input.GetAxis("Horizontal") * Vector2.right + Input.GetAxis("Vertical") * Vector2.up).normalized;
        if (Input.GetKeyDown(KeyCode.Q))
            isCameraOverRightShoulder = isCameraOverRightShoulder ? false : true;
        pivot.transform.localPosition = Vector3.SmoothDamp(pivot.transform.localPosition,new Vector3(pivotOffsetFromPlayer.x * (isCameraOverRightShoulder ? 1f : -1f), pivot.transform.localPosition.y, pivot.transform.localPosition.z), ref switchShoulderVelocity, timeToSwitchShoulder);
        
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
        if (playerMoveDirection.y != 0){
            velocityZ += playerMoveDirection.y * acceleration * Time.deltaTime;
            velocityZ = Mathf.Max(velocityZ, -maxWalkingVelocity);
        }
        velocityX *= friction;
        velocityZ *= friction;
    }
    
}
