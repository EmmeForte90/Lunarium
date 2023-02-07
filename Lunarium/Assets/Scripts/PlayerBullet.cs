using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float shotgunBullet = 3f;
    [SerializeField] float lanceSpeed = 10f;
    //Variabile della velocità del proiettile
    [SerializeField] GameObject Explode;
    [SerializeField] float MPCost = 10f;
    [SerializeField] Transform prefabExp;
    private float lifeTime = 0.5f;
    //Riservato allo shotgun
    PlayerHealth Less;
    Rigidbody2D myRigidbody;
    //Il corpo rigido
    CharacterController2D player;
    Enemy Enemy;
    //Attribuscie una variabile allo script di movimento del player
    //Per permettere al proiettile di emularne l'andamento
    float xSpeed;
    float shotgunSpeed;
    public Vector3 LaunchOffset;
    public string targetTag = "Enemy";
    private GameObject target;
    //Riservato alla bomb

    [Header("Che tipo di bullet")]
    [SerializeField] bool Dagger;
    [SerializeField] bool Sword;
    [SerializeField] bool Lance;
    [SerializeField] bool Bilama;
    [SerializeField] bool Axe;
    [SerializeField] bool rightFace;

    [Header("Audio")]
[SerializeField] AudioSource SExp;
[SerializeField] AudioSource SBomb;


   
        void Start()
    {
        target = GameObject.FindWithTag(targetTag);
        myRigidbody = GetComponent<Rigidbody2D>();
        //Recupera i componenti del rigidbody
        player = FindObjectOfType<CharacterController2D>();
        Less = FindObjectOfType<PlayerHealth>();
        Enemy = FindObjectOfType<Enemy>();
        //Recupera i componenti dello script
        xSpeed = player.transform.localScale.x * bulletSpeed;
        shotgunSpeed = player.transform.localScale.x * shotgunBullet;
        //La variabile è uguale alla scala moltiplicata la velocità del proiettile
        //Se il player si gira  anche lo spawn del proittile farà lo stesso
        CostMP();

        if(Lance)
        {        
            if(player.transform.localScale.x > 0)
            {
            var direction = transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction * lanceSpeed, ForceMode2D.Impulse);
            }
            else if(player.transform.localScale.x < 0)
            {
            var direction = -transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction * lanceSpeed, ForceMode2D.Impulse);
            }
        }
        transform.Translate(LaunchOffset);

        if(Bilama)
        {        
            if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            transform.Translate(direction.normalized * xSpeed * Time.deltaTime, Space.World);
        }
    }
        }
        
    




#region Update
    void Update()
    {
        
        if(Dagger || Sword && !Axe && !Lance)
        {
         myRigidbody.velocity = new Vector2 (xSpeed, 0f);

        }
        else if(!Dagger && !Sword && Axe && !Lance)
        {
        myRigidbody.velocity = new Vector2 (shotgunSpeed, 0f);
        }
        else if(!Dagger && !Sword && !Axe && Lance)
        {
        }
        //La velocità e la direzione del proiettile
        FlipSprite();
        
        
    }
#endregion
 

#region  FlipSprite
    void FlipSprite()
    {
        bool bulletHorSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //se il player si sta muovendo le sue coordinate x sono maggiori di quelle e
        //di un valore inferiore a 0

        if (bulletHorSpeed) //Se il player si sta muovendo
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
            //La scala assume un nuovo vettore e il rigidbody sull'asse x 
            //viene modificato mentre quello sull'asse y no. 
        }
        
        
    }

#endregion

#region  MP
    void CostMP()
    {
        if(Dagger || Sword && !Axe && !Lance)
        {
         Less.TakeManaDamage(MPCost);
        }
        else if(!Dagger && !Sword && Axe && !Lance)
        {
        Less.TakeManaDamage(MPCost);
        }
        else if(!Dagger && !Sword && !Axe && Lance)
        {
            Less.TakeManaDamage(MPCost);
        }
        
        
    }

#endregion
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        //Se il proiettile tocca il nemico
        {            
            SExp.Play();

            Instantiate(Explode, transform.position, transform.rotation);
            if(Dagger && !Sword )
            //Se è un proiettile normale e non rapido
            {
            Destroy(gameObject);
            //Il nemico subisce danno
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(10);
            //Viene distrutto quando colpisce il nemico

            }
            else if(!Dagger && Sword)
            //Quando è un proiettile rapido e non normale
            {
                IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(5);
            //Non viene distrutto
            }
            else if(Lance)
            //Quando è un proiettile rapido e non normale
            {
            Destroy(gameObject);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(50);
            //Non viene distrutto
            }
            else if(Axe)
            //Quando è un proiettile rapido e non normale
            {
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(25);
            //Non viene distrutto
            }
        }

        if(other.gameObject.tag == "Ground" && !Axe)
        //Se il proiettile tocca il nemico
        {     
            SExp.Play();       
            Instantiate(Explode, transform.position, transform.rotation);
            Destroy(gameObject);
            //Viene distrutto
        }
        else if(Axe)
        {
            Invoke("Destroy", lifeTime);
        }
        
    }

    void Destroy()
    {
        Destroy(gameObject); 
        if(!Axe)
        {
        Instantiate(Explode, transform.position, transform.rotation);
        }
  
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if(!Axe)
        {
        Instantiate(Explode, transform.position, transform.rotation);
        }
        Destroy(gameObject); 

        //Se il proiettile tocca una superficie viene distrutto 
    }

}
