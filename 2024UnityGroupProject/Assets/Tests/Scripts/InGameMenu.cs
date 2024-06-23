using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] GameObject loadGameButton;


    // Start is called before the first frame update
    void Start()
    {
        loadGameButton.SetActive(SaveLoadManager.instance.HasSavedGames);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1");
    }
}
