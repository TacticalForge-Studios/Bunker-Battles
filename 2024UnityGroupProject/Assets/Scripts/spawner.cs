using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int numberToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPos;

    public static int spawnCount;
    bool isSpawning;
    bool startSpawning;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        gameManager.instance.UpdateGameGoal(numberToSpawn);
        Debug.Log(spawnCount);
    }

    // Update is called once per frame
    void Update()
    {
        if(startSpawning && !isSpawning && spawnCount < numberToSpawn)
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;

        if (objectToSpawn.Length == 3)
        {


            for (int i = 0; i < numberToSpawn; i++)
            {
                int randomObjectSpawn = Random.Range(0, objectToSpawn.Length);
                int arrayPosition = Random.Range(0, spawnPos.Length);
                switch (randomObjectSpawn)
                {
                    case 0:
                        Instantiate(objectToSpawn[0], spawnPos[arrayPosition].position, spawnPos[arrayPosition].rotation);
                        yield return new WaitForSeconds(spawnTimer);
                        spawnCount++;
                        break;
                    case 1:
                        Instantiate(objectToSpawn[1], spawnPos[arrayPosition].position, spawnPos[arrayPosition].rotation);
                        yield return new WaitForSeconds(spawnTimer);
                        spawnCount++;
                        break;
                    case 2:
                        Instantiate(objectToSpawn[2], spawnPos[arrayPosition].position, spawnPos[arrayPosition].rotation);
                        yield return new WaitForSeconds(spawnTimer);
                        spawnCount++;
                        break;
                }

            }
        }
        else if(objectToSpawn.Length == 2)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                int randomObjectSpawn = Random.Range(0, objectToSpawn.Length);
                int arrayPosition = Random.Range(0, spawnPos.Length);
                switch (randomObjectSpawn)
                {
                    case 0:
                        Instantiate(objectToSpawn[0], spawnPos[arrayPosition].position, spawnPos[arrayPosition].rotation);
                        yield return new WaitForSeconds(spawnTimer);
                        spawnCount++;
                        break;
                    case 1:
                        Instantiate(objectToSpawn[1], spawnPos[arrayPosition].position, spawnPos[arrayPosition].rotation);
                        yield return new WaitForSeconds(spawnTimer);
                        spawnCount++;
                        break;
                }
            }
        }
        else if (objectToSpawn.Length == 1)
        {
            for(int i = 0; i < numberToSpawn; i++)
            {
                int arrayPosition = Random.Range(0, spawnPos.Length);
                Instantiate(objectToSpawn[0], spawnPos[arrayPosition].position, spawnPos[arrayPosition].rotation);
                spawnCount++;
                yield return new WaitForSeconds(spawnTimer);
            }
                
            
        }
        isSpawning = false;
        spawnCount = 0;
        gameObject.SetActive(false);


    }

    public int getNumberToSpawn()
    {
        return numberToSpawn;
    }




}
