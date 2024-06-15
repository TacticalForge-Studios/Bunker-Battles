using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCrouch : MonoBehaviour
{
    public CharacterController playerControl;
    public float crouchSpeed, normHeight, crouchHeight;
    public Vector3 offsetPlayer;
    public Transform thePlayer;
    bool isCrouching;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
        }

        if(isCrouching == true) 
        {
            playerControl.height = playerControl.height - crouchSpeed * Time.deltaTime;

            if(playerControl.height <= crouchHeight)
            {
                playerControl.height = crouchHeight;
            }
        }
        if (isCrouching == false)
        {
            playerControl.height = playerControl.height + crouchSpeed * Time.deltaTime;

            if (playerControl.height < normHeight)
            {
                thePlayer.position = thePlayer.position + offsetPlayer * Time.deltaTime;
            }
            if (playerControl.height >= normHeight)
            {
                playerControl.height = normHeight;
            }
        }
    }
}
