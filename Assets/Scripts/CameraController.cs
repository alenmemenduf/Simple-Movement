using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public float CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;
    private float rotX = 0.0f;
    private float rotY = 0.0f;
    private float mouseX = 0.0f;
    private float mouseY = 0.0f;
    private float minAngleY = -80f;
    private float maxAngleY = 80f;

    [Header("Aim Down Sights settings")]
    public int fovWhenNotAiming = 40;
    public int fovWhenAiming = 30;
    private float cameraZoomVelocity;
    public float aimZoomSpeed = 10f;
    private bool isAiming = false;
    private void updateAimDownSights(){
        if (isAiming)
            Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, fovWhenAiming, ref cameraZoomVelocity, 1 / aimZoomSpeed);
        else
            Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, fovWhenNotAiming, ref cameraZoomVelocity, 1 / aimZoomSpeed);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotY = rotation.y;
        rotX = rotation.x;
    }
    private void Update()
    {
        // get Input from Mouse
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        isAiming = Input.GetMouseButton(1);

        rotY -= mouseY * player.verticalMouseSensitivity * Time.deltaTime;
        rotX += mouseX * player.horizontalMouseSensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, minAngleY, maxAngleY);

        Quaternion localRotation = Quaternion.Euler(rotY, rotX, 0.0f);
        transform.rotation = localRotation;

        updateAimDownSights();
    }
    private void LateUpdate()
    {
        updateCamera();
    }
    void updateCamera(){
        Transform target = CameraFollowObj.transform;

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}

