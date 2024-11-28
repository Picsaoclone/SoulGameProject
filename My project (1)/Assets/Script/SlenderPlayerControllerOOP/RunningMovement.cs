using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RunningMovement : IMovementStrategy
{
    private SlenderPlayerController playerController;

    public RunningMovement(SlenderPlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Move()
    {
        Vector3 forward = playerController.transform.TransformDirection(Vector3.forward);
        Vector3 right = playerController.transform.TransformDirection(Vector3.right);

        bool isRunning = true; // Always running in this case
        float curSpeedX = playerController.canMove ? (isRunning ? playerController.runSpeed : playerController.walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = playerController.canMove ? (isRunning ? playerController.runSpeed : playerController.walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = playerController.GetCharacterController().velocity.y;
        Vector3 moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Jumping:
        if (Input.GetButton("Jump") && playerController.canMove && playerController.GetCharacterController().isGrounded)
        {
            moveDirection.y = playerController.GetJumpPower(); // Apply jump power
        }

        // Gravity:
        if (!playerController.GetCharacterController().isGrounded)
        {
            moveDirection.y -= playerController.gravity * Time.deltaTime;
        }

        playerController.GetCharacterController().Move(moveDirection * Time.deltaTime);
    }
}
