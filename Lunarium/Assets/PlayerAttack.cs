using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class PlayerAttack : MonoBehaviour
{
    [Header("Animations")]
    private Animator anim; // componente Animator del personaggio
    public SkeletonMecanim skeletonM;
    PlayerHealth Less;


    [Header("Attacks")]
    private float currentCooldown; // contatore del cooldown attuale
    [SerializeField] float nextAttackTime = 0f;
    [SerializeField] float attackRate = 2f;
    [SerializeField] public float attackCooldown = 0.5f; // tempo di attesa tra gli attacchi
    [SerializeField] public float comboTimer = 2f; // tempo per completare una combo
    [SerializeField] public int comboCounter = 0; // contatore delle combo
    [SerializeField] public int maxCombo = 4; // numero massimo di combo
    [SerializeField] public float shootTimer = 2f; // tempo per completare una combo
    [SerializeField] private GameObject bullet;

    public Transform slashpoint;
    private SkeletonMecanim skeletonMecanim;

    [Header("VFX")]
    // Variabile per il gameobject del proiettile
    [SerializeField] GameObject blam;
    [SerializeField] GameObject EvocationSword;
    [SerializeField] public Transform gun;
    [SerializeField] public Transform SE;



    [Header("Abilitations")]
    [SerializeField] public GameplayManager gM;

    public bool isAttacking = false;
    PlayerWeaponManager Wm;
    private Rigidbody2D rb; // componente Rigidbody2D del personaggio
    [SerializeField] public bool blockInput = false;
   
    [Header("Audio")]
    [SerializeField] AudioSource SwSl;
    [SerializeField] AudioSource Smagic;
    [SerializeField] AudioSource SRun;
    [SerializeField] AudioSource SCrash;

public static PlayerAttack instance;
public static PlayerAttack Instance
        {
            //Se non trova il componente lo trova in automatico
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<PlayerAttack>();
                return instance;
            }
        }

        private void Awake()
    { 
        Wm = GetComponent<PlayerWeaponManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

        Less = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        if (gM == null)
        {
            gM = GetComponent<GameplayManager>();
        }
        anim = GetComponent<Animator>();
        currentCooldown = attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gM.PauseStop)
        {
        // gestione dell'input dello sparo
if (Input.GetButtonDown("Fire2"))
{
    if(Less.currentMana > 0)
        {
    anim.SetTrigger("isShoot");
        }
}

if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                isAttacking = false;
                shootTimer = 0.5f;
            }
        // gestione del timer della combo
        if (comboCounter > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                isAttacking= false;
                comboCounter = 0;
                comboTimer = 0.5f;
            }
        }

        // gestione del cooldown dell'attacco
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        }
    }


    void blastAnm()
{
   
if (Time.time > nextAttackTime)
    {
    isAttacking = true;
    nextAttackTime = Time.time + 1f / attackRate;
    Smagic.Play();
    Instantiate(blam, gun.transform.position, transform.rotation);
    Instantiate(bullet, gun.transform.position, transform.rotation);
    }    
}


void evocationSword()
{
    Smagic.Play();
    Instantiate(EvocationSword, SE.transform.position, transform.rotation); 

}



void crashSlash()
{
    SCrash.Play();

}

public void slashSound()
    {
        SwSl.Play();
    } 

    void Attack()
    {
        if (currentCooldown <= 0)
        {
            isAttacking = true;
            comboCounter++;
            if (comboCounter > maxCombo)
            {
                comboCounter = 1;
            }
            anim.SetInteger("ComboCounter", comboCounter);
            anim.SetTrigger("Attack1");
            currentCooldown = attackCooldown;
            comboTimer = 0.5f;
        }
    }

   #region CambioMagia
public void SetBulletPrefab(GameObject newBullet)
//Funzione per cambiare arma
{
    bullet = newBullet;
}   
#endregion
}