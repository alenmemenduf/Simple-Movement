using UnityEngine;

public class ThirdPersonMovement : IPlayerMovement
{
    private PlayerController _playerController;

    public ThirdPersonMovement(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void SetIsRunning(ref bool isRunning, ref float currentMaxVelocity)
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);
        
        if (isRunning && _playerController.VelocityZ >= -0.2f)
            currentMaxVelocity = _playerController.MaxRunningVelocity;
        else
            currentMaxVelocity = _playerController.MaxWalkingVelocity;
    }

    public void SetMovementDirection(ref Vector2 moveDirection)
    {
        moveDirection = (Input.GetAxis("Horizontal") * Vector2.right + Input.GetAxis("Vertical") * Vector2.up).normalized;
    }
}