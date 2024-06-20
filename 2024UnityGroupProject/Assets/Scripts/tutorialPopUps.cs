using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class tutorialPopUps : MonoBehaviour
{
    [SerializeField] GameObject textMessage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<playerController>())
        {
            textMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<playerController>())
        {
            textMessage.SetActive(false);
        }
    }
}
