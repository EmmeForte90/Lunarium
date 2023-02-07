using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    // Variabili per memorizzare la scena attuale e la posizione del player
    public string spawnPointTag = "SpawnPoint";
    public GameObject button;
    public bool interactWithKey = true;
    public KeyCode changeSceneKey = KeyCode.E;
    public string sceneName;
    public bool needButton;
    public bool isDoor = false;

    private SceneEvent sceneEvent;
    private GameObject player;
    [Header("Audio")]
    [SerializeField] AudioSource Door;

    private void Start()
    {
        button.gameObject.SetActive(false); // Initially hide the dialogue text
        sceneEvent = GetComponent<SceneEvent>();
        sceneEvent.onSceneChange.AddListener(ChangeScene);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (player != null)
        {
            GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
            }
        }
    }

    private void Update()
    {
        
    
}

private void OnTriggerStay2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    {
        if(needButton)
        {
            button.gameObject.SetActive(true); // Initially hide the dialogue text
        }
        if (interactWithKey && Input.GetKey(changeSceneKey))
{
    if(isDoor)
    {
    Door.Play();

    }
sceneEvent.InvokeOnSceneChange();
}
       
}
}

private void OnTriggerEnter2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    {
        if(needButton)
        {
            button.gameObject.SetActive(true); // Initially hide the dialogue text
        }

         if (!interactWithKey)
        {
            sceneEvent.InvokeOnSceneChange();
        }
       
}
}

private void OnTriggerExit2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    {
        if(needButton)
        {
            button.gameObject.SetActive(false); // Initially hide the dialogue text
        }

        
       
}
}
}