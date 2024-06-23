using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class SavedGameState
{
    public int Version = 1;

    public class SimpleSpawnerState
    {
        public class Entry
        {
            public PrimitiveType type;
            public System.Tuple<float, float, float> location;
        }

        public string ID;
        public List<Entry> spawnedObjects = new List<Entry>();
    }

    public SimpleSpawnerState spawnerState = new SimpleSpawnerState();
}

public enum ESaveSlot
{
    None,

    Slot1,
    Slot2,
    Slot3,
    Slot4,
    Slot5,
}

public enum ESaveType
{
    Manual,
    Automatic
}

public interface ISaveable
{
    void PrepareForSave(SavedGameState gameState);
}

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] float autoSaveInterval = 300f;

    
    public static SaveLoadManager instance { get; private set; } = null;

    public SavedGameState savedState { get; private set; } = null;
    ESaveSlot currentSlot = ESaveSlot.None;

    List<ISaveable> saveHandlers = new List<ISaveable>();
    float timeUntilNextAutoSave = 0f;

    bool gameInProgress = false;

    public bool HasSavedGames
    {
        get
        {
            var allSlots = System.Enum.GetValues(typeof(ESaveSlot));
            foreach (var slot in allSlots)
            {
                var slotEnum = (ESaveSlot)slot;

                if (slotEnum == ESaveSlot.None) 
                    continue;

                if (DoesSaveExist(slotEnum, ESaveType.Manual))
                    return true;
                if (DoesSaveExist(slotEnum, ESaveType.Automatic))
                    return true;

            }

            return false;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadPersistentLevel()
    {
        // check if persistent level is already present
        for(int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
        {
            if (SceneManager.GetSceneAt(sceneIndex).name == "SaveLoadPersistentLevel")
            {
                return;
            }

        }

        // no persistent level present - load it name
        SceneManager.LoadScene("SaveLoadPersistentLevel", LoadSceneMode.Additive);
    }


    private void Awake()
    {
        
        if (instance != null)
        {
            Debug.LogError($"Found a duplicate SaveLoadManager on {gameObject.name}");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetGameInProgress(bool newValue)
    {
        gameInProgress = newValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (savedState != null && gameInProgress)
        {
            timeUntilNextAutoSave -= Time.deltaTime;

            if (timeUntilNextAutoSave <= 0)
            {
                timeUntilNextAutoSave = autoSaveInterval;

                RequestSave(currentSlot, ESaveType.Automatic);
            }
        }
    }

    public void RegisterHandler(ISaveable handler)
    {
        if (!saveHandlers.Contains(handler))
            saveHandlers.Add(handler);
    }

    public void DeregisterHandler(ISaveable handler)
    {
        saveHandlers.Remove(handler);
    }

    public string GetLastSavedTIme(ESaveSlot slot, ESaveType saveType)
    {
        var lastSavedTime = File.GetLastWriteTime(GetSaveFilePath(slot, saveType));
        {
            return $"{lastSavedTime.ToLongTimeString()} @ {lastSavedTime.ToLongDateString()}";
        }
    }

    string GetSaveFilePath(ESaveSlot slot, ESaveType saveType)
    {
        return Path.Combine(Application.persistentDataPath, $"SaveFile_{(int)slot}_{saveType.ToString()}.json");
    }

    public void RequestSave(ESaveSlot slot, ESaveType saveType)
    {
        SavedGameState savedState = new SavedGameState();

        // populate the save state
        foreach (var handler in saveHandlers)
        {
            if(handler == null)
            {
                continue;
            }

            handler.PrepareForSave(savedState);
        }
        
        var filePath = GetSaveFilePath(slot, saveType);

       

        File.WriteAllText(filePath, JsonConvert.SerializeObject(savedState, Formatting.Indented));
    }

    public bool DoesSaveExist(ESaveSlot slot, ESaveType saveType)
    {
        return File.Exists(GetSaveFilePath(slot, saveType));
    }

    public void RequestLoad(ESaveSlot slot, ESaveType saveType)
    {
        var filePath = GetSaveFilePath(slot, saveType);

        currentSlot = slot;
        savedState = JsonConvert.DeserializeObject<SavedGameState>(File.ReadAllText(filePath));

        timeUntilNextAutoSave = autoSaveInterval;
    }

    public void ClearSave()
    {
        savedState = null;
        currentSlot = ESaveSlot.None;

    }

}
