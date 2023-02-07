using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    public Transform respawnPoint; // il punto di respawn del giocatore
    public string sceneName; // il nome della scena in cui si trova il punto di respawn
    [SerializeField] GameObject Sdeng;
    [SerializeField] public Transform Pos;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        //Se il proiettile tocca il nemico
        {       
            
            Instantiate(Sdeng, Pos.transform.position, transform.rotation);
           

            

            
        }
}
}
