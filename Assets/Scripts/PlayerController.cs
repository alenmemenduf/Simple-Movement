using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
        private ThirdPersonMovement _thirdPersonMovement;
        private IPlayerLook _playerLook;
        private float _currentMaxVelocity;
        private bool _isRunning = false;

        private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
        _playerLook = new LikeCameraLook(this);
        _thirdPersonMovement = new ThirdPersonMovement(this);
    }

    private void Update()
    {
        _playerLook.SetPlayerFaceDirection(_camera.transform.forward);
    }

    private void FixedUpdate()
    {
        _thirdPersonMovement.SetIsRunning(ref _isRunning,ref _currentMaxVelocity);
        _thirdPersonMovement.SetMovementDirection(ref _playerMoveDirection);
        ChangeVelocity();
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
        
        CapVelocity();
        _rigidbody.velocity = Vector3.ClampMagnitude(transform.right * VelocityX + transform.forward * VelocityZ, _currentMaxVelocity) + _rigidbody.velocity.y * Vector3.up;
    }

    // function to cap the velocity inside the boundaries
    private void CapVelocity()
    {
        if(Mathf.Abs(VelocityX) < _idleThreshold)
            VelocityX = 0f;
        if(Mathf.Abs(VelocityZ) < _idleThreshold)
            VelocityZ = 0f;
        if(Mathf.Abs(VelocityX) > _currentMaxVelocity)
            VelocityX = Mathf.Sign(VelocityX) * _currentMaxVelocity;
        if(Mathf.Abs(VelocityZ) > _currentMaxVelocity)
            VelocityZ = Mathf.Sign(VelocityZ) * _currentMaxVelocity;
    } 
    
}