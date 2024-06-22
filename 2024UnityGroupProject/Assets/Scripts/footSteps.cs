using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footSteps : MonoBehaviour
{
    public AudioSource footstepSounds;


    private void Update()
    {

        
        if (transform.hasChanged)
        {
            footstepSounds.enabled = true;
            transform.hasChanged = false;
        }
        else
        {
            footstepSounds.enabled = false;

        }
    }
}
