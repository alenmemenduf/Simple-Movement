using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera follow settings")]
        [SerializeField] private PlayerController _player;
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _cameraFollowObj;
        [SerializeField] private float _cameraMoveSpeed = 120.0f;

    [Header("Aim down sights settings")]
        [SerializeField] private int _fovWhenNotAiming = 40;
        [SerializeField] private int _fovWhenAiming = 30;
        [SerializeField] private float _aimZoomSpeed = 10f;

        private float _mouseX = 0.0f;
        private float _mouseY = 0.0f;
        private float _rotationOnXAxis = 0.0f;
        private float _rotationOnYAxis = 0.0f;
        private float _minAngleY = -80f;
        private float _maxAngleY = 80f;
        private bool _isAiming = false;
        private float _cameraZoomVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 rotation = transform.localRotation.eulerAngles;
        _rotationOnYAxis = rotation.y;
        _rotationOnXAxis = rotation.x;
    }

    private void Update()
    {
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");
        _isAiming = Input.GetMouseButton(1);

        _rotationOnYAxis -= _mouseY * _player.VerticalMouseSensitivity * 100f * Time.deltaTime;
        _rotationOnXAxis += _mouseX * _player.HorizontalMouseSensitivity * 100f * Time.deltaTime;
        _rotationOnYAxis = Mathf.Clamp(_rotationOnYAxis, _minAngleY, _maxAngleY);

        Quaternion localRotation = Quaternion.Euler(_rotationOnYAxis, _rotationOnXAxis, 0.0f);
        transform.rotation = localRotation;
        
        UpdateAimDownSights();
    }

    private void LateUpdate()
    {
        Transform target = _cameraFollowObj.transform;
        float step = _cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    private void UpdateAimDownSights(){
        if (_isAiming)
            _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, _fovWhenAiming, ref _cameraZoomVelocity, 1 / _aimZoomSpeed);
        else
            _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, _fovWhenNotAiming, ref _cameraZoomVelocity, 1 / _aimZoomSpeed);
    }
}

