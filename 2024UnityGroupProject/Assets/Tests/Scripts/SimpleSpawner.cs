using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//using static UnityEditor.FilePathAttribute;

public class SimpleSpawner : MonoBehaviour, ISaveable
{
    [SerializeField] string ID = "SimpleSpawner-1";
    [SerializeField] int numObjects = 5;

    List<System.Tuple<GameObject, PrimitiveType>> spawnedObjects = new List<System.Tuple<GameObject, PrimitiveType>>();
    

    public void PrepareForSave(SavedGameState gameState)
    {
        gameState.spawnerState.ID = ID;

        foreach (var spawnedGOInfo in spawnedObjects)
        {
            var location = spawnedGOInfo.Item1.transform.position;
            
            gameState.spawnerState.spawnedObjects.Add(new SavedGameState.SimpleSpawnerState.Entry()
            {
                location = new System.Tuple<float, float, float>(location.x, location.y, location.z),
                type = spawnedGOInfo.Item2
            });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveLoadManager.instance.RegisterHandler(this);
        
        // are we loading from a save file?
        if (SaveLoadManager.instance.savedState != null)
        {
            var spawnerState = SaveLoadManager.instance.savedState.spawnerState;
            if (spawnerState.ID == ID)
            {
                foreach (var entry in spawnerState.spawnedObjects)
                {
                    PrimitiveType typeToSpawn = entry.type;
                    Vector3 location = new Vector3(entry.location.Item1, entry.location.Item2, entry.location.Item3);

                    SpawnObject(typeToSpawn, location);
                }
                
                return;
            }
        }
        
        var availableTypes = System.Enum.GetValues(typeof(PrimitiveType));
        for (int i = 0; i < numObjects; i++)
        {
            // pick a random type
            int typeIndex = Random.Range(0, availableTypes.Length);
            PrimitiveType typeToSpawn = (PrimitiveType)availableTypes.GetValue(typeIndex);
            Vector3 location = new Vector3(Random.Range(0, 5f), Random.Range(0, 5f), Random.Range(0, 5f));

            SpawnObject(typeToSpawn, location);
            
        }

    }

    void SpawnObject(PrimitiveType typeToSpawn, Vector3 location)
    {
        var newGO = GameObject.CreatePrimitive(typeToSpawn);
        newGO.transform.position = location;

        spawnedObjects.Add(new System.Tuple<GameObject, PrimitiveType>(newGO, typeToSpawn));
    }

    private void OnDestroy()
    {
        SaveLoadManager.instance.DeregisterHandler(this);
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
