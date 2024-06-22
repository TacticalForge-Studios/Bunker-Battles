using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossFight : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] spawner spawn;
    [SerializeField] GameObject defeatTheGeneralText;
    public static bool deadGeneral = false;

    private void Update()
    {
        if (deadGeneral)
        {
            door.SetActive(false);
            int numberDead = spawn.getNumberToSpawn() - spawner.spawnCount;
            gameManager.instance.UpdateGameGoal(-numberDead);
            gameObject.SetActive(false);
            defeatTheGeneralText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        door.SetActive(true);


    }


}
