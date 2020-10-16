using UnityEngine;

public interface IPlayerMovement
{
    void SetIsRunning(ref bool isRunning, ref float currentMaxVelocity);
    void SetMovementDirection(ref Vector2 moveDirection);
}