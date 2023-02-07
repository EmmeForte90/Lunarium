using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;
using Spine.Unity.AttachmentTools;
using Spine.Unity;
using Spine;

public class GameplayManager : MonoBehaviour
{


    public static bool playerExists;
    
    private GameObject player; // Variabile per il player
    public static GameplayManager instance;
    private CinemachineVirtualCamera virtualCamera;
    [HideInInspector]
    public bool gameplayOff = false;

    [Header("Money")]
    [SerializeField] int money = 0;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI moneyTextM;
    [SerializeField] GameObject moneyObject;
    [SerializeField] GameObject moneyObjectM;
    [HideInInspector]
    public bool PauseStop = false;
    //Variabile del testo dei money

    [Header("Music")]
    [SerializeField] bool isStartGame;
    [SerializeField] bool isTImeline;
    [SerializeField] AudioSource City;
    
    [Header("Fade")]
    [SerializeField] GameObject callFadeIn;
    [SerializeField] GameObject callFadeOut;
    [SerializeField] GameObject centerCanvas;

    [Header("Pause")]
    [SerializeField] public GameObject PauseMenu;


    public int maxPotions = 6;
    public int Potions;
    public GameObject[] hudObjects;
    private int lastRemovedIndex = -1;






    private void Awake()
    {
        
            Potions = maxPotions;
        player = GameObject.FindWithTag("Player");
virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
virtualCamera.Follow = player.transform;
virtualCamera.LookAt = player.transform;
StartCoroutine(StartFadeIn());


        // Verifica se un'istanza del GameObject esiste già e distruggila se necessario
        if (playerExists) //&& gameplayOff) 
        {
            Destroy(gameObject);
        }
        else 
        {
            playerExists = true;
            DontDestroyOnLoad(gameObject);
            
        }


        if(!moneyObject.gameObject)
        {
            moneyObject.gameObject.SetActive(true);
        }
        
        moneyText.text = money.ToString();
        moneyTextM.text = money.ToString();    
        //Il testo assume il valore dello money
    }




public void StartPlay()
    {
        StartCoroutine(StartStage());
    }

#region  Score
//Funziona
    public void AddTomoney(int pointsToAdd)
    {
        money += pointsToAdd;
        //Lo money aumenta
        moneyText.text = money.ToString(); 
        moneyTextM.text = money.ToString();    
        //il testo dello money viene aggiornato
    }

//Boh

    void TakeLife()
    {
        //Il player perde 1 vita
        StartCoroutine(Restart());
    }

#endregion

public void removeOnePotion()
{
    if(Potions > 0)
    {
        Potions--;
        if (Potions < hudObjects.Length)
        {
            lastRemovedIndex = hudObjects.Length - Potions - 1;
            hudObjects[lastRemovedIndex].SetActive(false);
        }
    }
}

public void restoreOnePotion()
{
    if(Potions < 6)
    {
        Potions++;
        if (lastRemovedIndex != -1)
        {
            hudObjects[lastRemovedIndex].SetActive(true);
            lastRemovedIndex = -1;
        }
    }
}


#region Pausa
        public void Pause()
        //Funzione pausa
        {
            PauseMenu.gameObject.SetActive(true);
            PauseStop = true;
            //Time.timeScale = 0f;
        }

        public void Resume()
        {
            //Time.timeScale = 1;
            PauseStop = false;
            PauseMenu.gameObject.SetActive(false);
        }

#endregion

    
#region Processo vita e morte

public void StartDie()
    {
        StartCoroutine(CallGameSession());
        //AudioManager.instance.DieMusic();
    }

    IEnumerator CallGameSession()
    {
        yield return new WaitForSeconds(2f);
        ProcessPlayerDeath();
        //Richiama i componenti dello script gamesessione e 
        //ne attiva la funzione di processo di morte 

    }
public void ProcessPlayerDeath()
    {
            StartCoroutine(AfterDie());
            //ResetGameSession();
            //Richiama la funzione di reset
    }
    

//Funziona
    public void DeactiveGameOver()
    {
        //gameOver.gameObject.SetActive(false);
        Time.timeScale = 1;
        //playMusic();
        StartCoroutine(Restart());
    }

//Funziona
    IEnumerator AfterDie()
    {
        //gameOver.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
    }

    IEnumerator Restart()
    {
        callFadeIn.gameObject.SetActive(true);
        //Instantiate(callFadeIn, centerCanvas.transform.position, centerCanvas.transform.rotation);
        yield return new WaitForSeconds(5f);
        //Le vite del player vengono aggiornate
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Lo scenario assume il valore della build
        SceneManager.LoadScene(currentSceneIndex);
        //Lo scenario viene ricaricato
    }

IEnumerator StartFadeIn()
    {
        callFadeIn.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        callFadeIn.gameObject.SetActive(false);


    }

    IEnumerator StartStage()
    {
        if(!isStartGame)
        {
            City.Play();
        callFadeOut.gameObject.SetActive(true);
        //Instantiate(callFadeOut, centerCanvas.transform.position, centerCanvas.transform.rotation);
        }
        //FindObjectOfType<PlayerMovement>().ReactivatePlayer();
        yield return new WaitForSeconds(5f);
        callFadeOut.gameObject.SetActive(false);

    }

#endregion

}