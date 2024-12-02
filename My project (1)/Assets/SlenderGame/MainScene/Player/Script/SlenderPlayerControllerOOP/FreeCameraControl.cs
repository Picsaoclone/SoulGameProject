using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraControl : ICameraControlStrategy
{
    private SlenderPlayerController playerController;

    public FreeCameraControl(SlenderPlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void ControlCamera()
    {
        if (playerController.canMove)
        {
            float mouseY = Input.GetAxis("Mouse Y") * playerController.lookSpeed;
            playerController.SetRotationX(playerController.GetRotationX() - mouseY);
            playerController.SetRotationX(Mathf.Clamp(playerController.GetRotationX(), -playerController.lookXLimit, playerController.lookXLimit));

            playerController.playerCam.transform.localRotation = Quaternion.Euler(playerController.GetRotationX(), 0, 0); // Look up/down
            playerController.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * playerController.lookSpeed, 0); // Look left/right
        }
    }
}

