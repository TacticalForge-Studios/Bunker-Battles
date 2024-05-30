using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] Renderer model;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gameManager.instance.playerSpawnPos.transform.position != transform.position)
        {
            gameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(displayPopUp());
        }
    }

    IEnumerator displayPopUp()
    {
        model.material.color = Color.red;
        gameManager.instance.checkpointPopUp.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        gameManager.instance.checkpointPopUp.SetActive(false);
        model.material.color = Color.white;
    }
}
