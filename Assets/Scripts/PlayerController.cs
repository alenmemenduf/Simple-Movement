using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera settings")]
        [SerializeField] private float _switchShoulderDuration = 0.5f;
        [SerializeField] private GameObject _cameraPivot;
        [SerializeField] private Vector3 _pivotOffsetFromPlayer = new Vector3(0.3f, 1.8f, 0.0f);

    [Header("Player speed")]
        [SerializeField] private float _acceleration = 100f;
        [SerializeField] private float _maxWalkingVelocity = 4f;
        [SerializeField] private float _maxRunningVelocity = 6f;
        [SerializeField] private float _friction = 0.9f;
        [SerializeField] private float _idleThreshold = 0.1f;

    [Header("Mouse settings")]
        [SerializeField] private float _horizontalMouseSensitivity = 1f;
        [SerializeField] private float _verticalMouseSensitivity = 1f;


        public float VelocityX { get; private set; } = 0f;
        public float VelocityZ { get; private set; } = 0f;
        public float MaxWalkingVelocity { get => _maxWalkingVelocity; private set => _maxWalkingVelocity = value; }
        public float MaxRunningVelocity { get => _maxRunningVelocity; private set => _maxRunningVelocity = value; }
        public float HorizontalMouseSensitivity { get => _horizontalMouseSensitivity; private set => _horizontalMouseSensitivity = value; }
        public float VerticalMouseSensitivity { get => _verticalMouseSensitivity; private set => _verticalMouseSensitivity = value; }

        private Rigidbody _rigidbody;
        private Camera _camera;
        private Vector2 _playerMoveDirection = Vector2.zero;
        private float _maxVelocity;
        private bool _isRunning = false;
        private Vector3 _switchShoulderVelocity;
        private bool _isCameraOverRightShoulder = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleInputs();
        _cameraPivot.transform.localPosition = Vector3.SmoothDamp(_cameraPivot.transform.localPosition, new Vector3(_pivotOffsetFromPlayer.x * (_isCameraOverRightShoulder ? 1f : -1f), _cameraPivot.transform.localPosition.y, _cameraPivot.transform.localPosition.z), ref _switchShoulderVelocity, _switchShoulderDuration);
        transform.LookAt(transform.position + new Vector3(_camera.transform.forward.x, 0f, _camera.transform.forward.z), Vector3.up);
    }

    

    private void FixedUpdate()
    {
        SwitchBetweenWalkAndRun();
        ChangeVelocity();
        CapVelocity();
        _rigidbody.velocity = Vector3.ClampMagnitude(transform.right * VelocityX + transform.forward * VelocityZ, _maxVelocity) + _rigidbody.velocity.y * Vector3.up;
    }

    private void SwitchBetweenWalkAndRun()
    {
        if(_isRunning && VelocityZ >= -0.2f)
            _maxVelocity = MaxRunningVelocity;
        else
            _maxVelocity = MaxWalkingVelocity;
    }

    private void HandleInputs()
    {
        _isRunning = Input.GetKey(KeyCode.LeftShift);
        _playerMoveDirection = (Input.GetAxis("Horizontal") * Vector2.right + Input.GetAxis("Vertical") * Vector2.up).normalized;
        if(Input.GetKeyDown(KeyCode.Q))
            _isCameraOverRightShoulder = _isCameraOverRightShoulder ? false : true;
    }
    private void ChangeVelocity()
    {
        if(_playerMoveDirection.x != 0)
            VelocityX += _playerMoveDirection.x * _acceleration * Time.deltaTime;
        if(_playerMoveDirection.y != 0)
        {
            VelocityZ += _playerMoveDirection.y * _acceleration * Time.deltaTime;
            VelocityZ = Mathf.Max(VelocityZ, -MaxWalkingVelocity);
        }
        VelocityX *= _friction;
        VelocityZ *= _friction;
    }

    // function to cap the velocity inside the boundaries
    private void CapVelocity()
    {
        if(Mathf.Abs(VelocityX) < _idleThreshold)
            VelocityX = 0f;
        if(Mathf.Abs(VelocityZ) < _idleThreshold)
            VelocityZ = 0f;
        if(Mathf.Abs(VelocityX) > _maxVelocity)
            VelocityX = Mathf.Sign(VelocityX) * _maxVelocity;
        if(Mathf.Abs(VelocityZ) > _maxVelocity)
            VelocityZ = Mathf.Sign(VelocityZ) * _maxVelocity;
    } 

}