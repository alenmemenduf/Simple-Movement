using UnityEngine;

public class LikeCameraLook : IPlayerLook
{
    private PlayerController _playerController;

    public LikeCameraLook(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void SetPlayerFaceDirection(Vector3 direction)
    {
        Vector3 directionWithoutYAxis = new Vector3(direction.x, 0f, direction.z);
        Vector3 lookAtPoint = _playerController.transform.position + directionWithoutYAxis;
        _playerController.transform.LookAt(lookAtPoint, Vector3.up);
    }
}