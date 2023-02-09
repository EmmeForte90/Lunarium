using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Fireball : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float shotgunBullet = 3f;
    [SerializeField] float bombBullet = 10f;
    //Variabile della velocità del proiettile
    //[SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
    private float lifeTime = 0.5f;
    //Riservato allo shotgun
    PlayerHealth playerHP;

    Rigidbody2D rb;
    //Il corpo rigido
    DarkWizard Enemies;
    //Attribuscie una variabile allo script di movimento del player
    //Per permettere al proiettile di emularne l'andamento
    float xSpeed;
    float shotgunSpeed;
    //L'andatura
    float bombSpeed = 5f;
    public Vector3 LauchOffset;
    //Riservato alla bomb
private SkeletonMecanim skeletonMecanim;
 GameplayManager gM;
    private Transform player;
    [Header("Che tipo di bullet")]
    [SerializeField] bool isFireball;
    [SerializeField] bool isRapid;
    [SerializeField] bool isBomb;
    [SerializeField] bool isTarget;
    [SerializeField] bool isShotgun;
    [SerializeField] bool rightFace;
    private Animator animator;

[Header("Audio")]
[SerializeField] AudioSource SExp;
[SerializeField] AudioSource SBomb;

    void Start()
    {
        playerHP = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //Recupera i componenti del rigidbody
        Enemies = FindObjectOfType<DarkWizard>();
        //Recupera i componenti dello script
        xSpeed = Enemies.transform.localScale.x * bulletSpeed;
        shotgunSpeed = Enemies.transform.localScale.x * shotgunBullet;
        //La variabile è uguale alla scala moltiplicata la velocità del proiettile
        //Se il player si gira  anche lo spawn del proittile farà lo stesso
        
        if(isBomb)
        {        
            if(DarkWizard.instance.transform.localScale.x > 0)
            {
            var direction = transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction * bombSpeed, ForceMode2D.Impulse);
            }
            else if(DarkWizard.instance.transform.localScale.x < 0)
            {
            var direction = -transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction * bombSpeed, ForceMode2D.Impulse);
            }
        }
        transform.Translate(LauchOffset);
        
    }




 #region Update
void Update()
{
    float velocityX = 0f;
    if (isFireball)
    {
        velocityX = xSpeed;
    }
    else if (isRapid)
    {
        velocityX = xSpeed;
    }
    else if (isShotgun)
    {
        velocityX = shotgunSpeed;
    }
    rb.velocity = new Vector2(velocityX, 0f);
    FlipSprite();
}
#endregion
 

#region  FlipSprite
    void FlipSprite()
    {
        bool bulletHorSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        //se il player si sta muovendo le sue coordinate x sono maggiori di quelle e
        //di un valore inferiore a 0

        if (bulletHorSpeed) //Se il player si sta muovendo
        {
            transform.localScale = new Vector2 (Mathf.Sign(rb.velocity.x), 1f);
            //La scala assume un nuovo vettore e il rigidbody sull'asse x 
            //viene modificato mentre quello sull'asse y no. 
        }
        
        
    }

#endregion


void OnTriggerEnter2D(Collider2D other)
{
    switch (other.gameObject.tag)
    {
        case "Player":
            SExp.Play();
            //Instantiate(Explode, transform.position, transform.rotation);
            int damage = 0;

            if (isFireball)
            {
                damage = 5;
            }
            else if (isRapid)
            {
                damage = 15;
            }
            else if (isBomb)
            {
                damage = 50;
            }
            else if (isShotgun)
            {
                damage = 60;
            }else if (isTarget)
            {
                damage = 10;
            }

            playerHP.GetComponent<PlayerHealth>().Damage(damage);
            animator.SetBool("take", true);

            if (isFireball || isBomb)
            {
                Destroy(gameObject);
            }
            break;

        case "Ground":
            SExp.Play();
            //Instantiate(Explode, transform.position, transform.rotation);
            animator.SetBool("take", true);

            if (!isShotgun)
            {
                Destroy(gameObject);
            }
            else
            {
                Invoke("Destroy", lifeTime);
            }
            break;
    }
}

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);   
        //Se il proiettile tocca una superficie viene distrutto 
    }

}