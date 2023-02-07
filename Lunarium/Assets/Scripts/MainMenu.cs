using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject continueButton;
    public string startScene;
    public float Timelife;
    public GameObject opzioni;
    public AudioMixer MSX;
    public AudioMixer SFX;

    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    //public PlayerAbilityTracker player;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("ContinueLevel"))
        {
            continueButton.SetActive(true);
        }

        resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
         string option = resolutions[i].width + "x" + resolutions[i].height;
        options.Add(option);

        if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
        {
            currentResolutionIndex = i;
        }

        }

//        resolutionDropdown.ClearOptions();
//        resolutionDropdown.value = currentResolutionIndex;
        //resolutionDropdown.RefreshValue();
//        resolutionDropdown.AddOptions(options);



        if(PlayerPrefs.HasKey(startScene + "_unlocked"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }

    }

    public void Continue()
    {
        //player.gameObject.SetActive(true);
        //PlayerMovement.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));
        StartCoroutine(fadeCont());

    }

public void SetResolution(int resolutionIndex)
{
    Resolution resolution = resolutions[resolutionIndex];

    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
}

    public void StartGame()
    {
        StartCoroutine(fade());

    }


    public void Options()
    {
        opzioni.gameObject.SetActive(true);
    }
    public void ExitOptions()
    {
        opzioni.gameObject.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        MSX.SetFloat("Volume", volume);

    }


     public void SetSFX(float volume)
    {
        SFX.SetFloat("Volume", volume);

    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

IEnumerator fade()
    {
        
        PlayerPrefs.DeleteAll();
        yield return new WaitForSeconds(Timelife);
//        GameplayManager.instance.gameplayOff = false;
        SceneManager.LoadScene(startScene);

            
    }

    IEnumerator fadeCont()
    {
        
        yield return new WaitForSeconds(Timelife);
        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
        //AudioManager.instance.PlayMFX(1);



            
    }
 

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");

    }

}
