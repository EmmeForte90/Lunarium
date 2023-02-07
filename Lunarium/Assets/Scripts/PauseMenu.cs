using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu instance;

    public string levelSelect, mainMenu;

    private void Awake()
    {
        instance = this;
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);
        //FindObjectOfType<ScenePersist>().ResetScenePersist();
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        GameplayManager.instance.gameplayOff = true;
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1;
    }

}