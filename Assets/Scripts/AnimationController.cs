using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class AnimationController : MonoBehaviour
{
    public AnimatorController controller;
    public GameObject player;
    private PlayerController playerController;
    private ChildMotion[] motionArray;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        motionArray = ((BlendTree)controller.layers[0].stateMachine.defaultState.motion).children; 
        for (int i = 0; i < motionArray.Length; i++)
        {
            switch (motionArray[i].motion.name)
            {
                case "Idle":
                    motionArray[i].position = Vector2.zero;
                    break;
                case "Walking":
                    motionArray[i].position = new Vector2(0f, playerController.maxWalkingVelocity);
                    break;
                case "Running":
                    motionArray[i].position = new Vector2(0f, playerController.maxRunningVelocity);
                    break;
                case "Walking Backwards":
                    motionArray[i].position = new Vector2(0f, -playerController.maxWalkingVelocity);
                    break;
                case "Left Strafe Walking":
                    motionArray[i].position = new Vector2(-playerController.maxWalkingVelocity, 0f);
                    break;
                case "Right Strafe Walking":
                    motionArray[i].position = new Vector2(playerController.maxWalkingVelocity, 0f);
                    break;
                case "Left Strafe":
                    motionArray[i].position = new Vector2(-playerController.maxRunningVelocity, 0f);
                    break;
                case "Right Strafe":
                    motionArray[i].position = new Vector2(playerController.maxRunningVelocity, 0f);
                    break;
            }
        }
        ((BlendTree)controller.layers[0].stateMachine.defaultState.motion).children = motionArray;
    }
    private void Update()
    {
        
    }
}
