using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossFight : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] spawner spawn;
    public static bool deadGeneral = false;

    private void Update()
    {
        if (deadGeneral)
        {
            door.SetActive(false);
            int numberDead = spawn.getNumberToSpawn() - spawner.spawnCount;
            gameManager.instance.UpdateGameGoal(-numberDead);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        door.SetActive(true);


    }


}
