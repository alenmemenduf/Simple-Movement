using UnityEngine;
using UnityEditor.Animations;

public class AnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimatorController _controller;
    private ChildMotion[] _motionArray;

    private void Start()
    {
        _motionArray = GetMotionArrayCopy();
        ModifyMotionArrayCopy();
        SetMotionArray(_motionArray);
    }

    private void Update()
    {
        SetVelocityPropertyInAnimator();
    }
    
    private ChildMotion[] GetMotionArrayCopy()
    {
        // This returns all the animations in our Animator Controller's blend tree.
        return ((BlendTree)_controller.layers[0].stateMachine.defaultState.motion).children;
    }
    private void ModifyMotionArrayCopy()
    {
        /* Here we modify all the parameters of the animations in the blend tree
        * to the values of the maximum velocity set by us in PlayerController
        * just so we have to modify the maximum velocity in only one place.
        * */
        for(int i = 0; i < _motionArray.Length; i++)
        {
            switch(_motionArray[i].motion.name)
            {
                case "Idle":
                    _motionArray[i].position = Vector2.zero;
                    break;
                case "Walking":
                    _motionArray[i].position = new Vector2(0f, _playerController.MaxWalkingVelocity);
                    break;
                case "Running":
                    _motionArray[i].position = new Vector2(0f, _playerController.MaxRunningVelocity);
                    break;
                case "Walking Backwards":
                    _motionArray[i].position = new Vector2(0f, -_playerController.MaxWalkingVelocity);
                    break;
                case "Left Strafe Walking":
                    _motionArray[i].position = new Vector2(-_playerController.MaxWalkingVelocity, 0f);
                    break;
                case "Right Strafe Walking":
                    _motionArray[i].position = new Vector2(_playerController.MaxWalkingVelocity, 0f);
                    break;
                case "Left Strafe":
                    _motionArray[i].position = new Vector2(-_playerController.MaxRunningVelocity, 0f);
                    break;
                case "Right Strafe":
                    _motionArray[i].position = new Vector2(_playerController.MaxRunningVelocity, 0f);
                    break;
            }
        }
    }
    private void SetMotionArray( ChildMotion[] motionArray )
    {
        ((BlendTree)_controller.layers[0].stateMachine.defaultState.motion).children = motionArray;
    }

    private void SetVelocityPropertyInAnimator()
    {
        // This is how we update the Velocity parameter inside our animator.
        _animator.SetFloat("VelocityX", _playerController.VelocityX);
        _animator.SetFloat("VelocityZ", _playerController.VelocityZ);
    }
}
