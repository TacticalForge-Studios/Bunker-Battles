using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_MainMenu : MonoBehaviour
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

    public void NewGame()
    {
        SaveLoadManager.instance.ClearSave();

        LoadMainLevel();
    }


    public void LoadMainLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Jon's test");
    }
}
