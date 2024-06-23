using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamemanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaveLoadManager.instance.SetGameInProgress(true);
    }

    private void OnDestroy()
    {
        SaveLoadManager.instance.SetGameInProgress(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
