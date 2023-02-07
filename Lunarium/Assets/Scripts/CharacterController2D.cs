using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class CharacterController2D : MonoBehaviour
{
    [Header("Move")]

    [SerializeField] public float moveSpeed = 5f; // velocità di movimento
    [SerializeField] public float jumpForce = 5f; // forza del salto
    [SerializeField] public float runMultiplier = 2f; // moltiplicatore di velocità per la corsa
    private int jumpCounter = 0;
    private int maxJumps = 2;
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;
    public float fallMultiplier = 2.5f;
    Vector2 playerPosition;
    Vector2 HitPosition;
    public GameObject Hit;

    public float dashForce = 50f;
    public float dashDuration = 0.5f;
    private float dashTime;
    private bool dashing;
    public float dashCoolDown = 1f;
    private float coolDownTime;
    public float crouchSpeed = 2f;
    
    CapsuleCollider2D collider;
    private Vector2 initialColliderSize;
    private Vector2 crouchedColliderSize;
    private Vector2 initialColliderOffset;
    private Vector2 crouchedColliderOffset;

    [Header("HP")]
    [SerializeField]public float health = 100f; // salute del personaggio
    PlayerHealth Less;


    [Header("Respawn")]
    //[HideInInspector]
    private Transform respawnPoint; // il punto di respawn del giocatore
    public string sceneName; // il nome della scena in cui si trova il punto di respawn
    
    [Header("Animations")]
    private Animator anim; // componente Animator del personaggio

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

    [Header("VFX")]
    // Variabile per il gameobject del proiettile
    [SerializeField] GameObject blam;
    [SerializeField] GameObject EvocationSword;
    [SerializeField] public Transform gun;
    [SerializeField] GameObject Circle;
    [SerializeField] public Transform circlePoint;

    [Header("Abilitations")]
    [SerializeField] public GameplayManager gM;
    private bool IsKnockback = false;
    private bool stopInput = false;
    private bool isGrounded = false; // vero se il personaggio sta saltando
    private bool isJumping = false; // vero se il personaggio sta saltando
    private bool isFall = false; // vero se il personaggio sta saltando
    private bool isHurt = false; // vero se il personaggio sta saltando
    private bool isLoop = false; // vero se il personaggio sta saltando
    private bool isAttacking = false; // vero se il personaggio sta attaccando
    private bool isCrouching = false; // vero se il personaggio sta attaccando
    private bool isLanding = false; // vero se il personaggio sta attaccando
    private bool isRunning = false; // vero se il personaggio sta correndo
    private bool isHeal = false; // vero se il personaggio sta correndo
    private float currentSpeed; // velocità corrente del personaggio
    private Rigidbody2D rb; // componente Rigidbody2D del personaggio
    [SerializeField] public static bool playerExists;
    [SerializeField] public bool blockInput = false;
   
    public SkeletonMecanim skeletonM;
    public float moveX;
[Header("Audio")]
[SerializeField] AudioSource SwSl;
[SerializeField] AudioSource Smagic;
[SerializeField] AudioSource SRun;
[SerializeField] AudioSource SCrash;




public static CharacterController2D instance;
public static CharacterController2D Instance
        {
            //Se non trova il componente lo trova in automatico
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<CharacterController2D>();
                return instance;
            }
        }

    void Start()
    {
        collider = GetComponent<CapsuleCollider2D>();
        initialColliderSize = collider.size;
        crouchedColliderSize = new Vector2(initialColliderSize.x, initialColliderSize.y / 2);
        initialColliderOffset = collider.offset;
        crouchedColliderOffset = new Vector2(initialColliderOffset.x, initialColliderOffset.y - 1f);
        isGrounded = true; // vero se il personaggio sta saltando
        playerPosition = transform.position;
        HitPosition = Hit.transform.position;
        Less = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        if (gM == null)
        {
            gM = GetComponent<GameplayManager>();
        }
        anim = GetComponent<Animator>();
        currentCooldown = attackCooldown;
        
        if (playerExists) {
        Destroy(gameObject);
    }
    else {
        playerExists = true;
        DontDestroyOnLoad(gameObject);
    }


    }

    void Update()
    {
         

        if (Less.currentHealth <= 0)
        {
            Respawn();
        }

        if(!gM.PauseStop || IsKnockback)
        {

#region Move
moveX = Input.GetAxis("Horizontal");
currentSpeed = moveSpeed;
if (isRunning && !isAttacking)
{
    if (moveX != 0)
    {
        if (isCrouching)
        {
            currentSpeed = moveSpeed * crouchSpeed;
        }
        else if (isRunning)
        {
            currentSpeed = moveSpeed * runMultiplier;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
    }
    else
    {
        currentSpeed = 0;
    }
}
if (isAttacking || isLanding)
{
    rb.velocity = new Vector2(0f, 0f);
}
else
{
    rb.velocity = new Vector2(moveX * currentSpeed, rb.velocity.y);
}

/////////////////////////////////
if (Input.GetKey(KeyCode.S))
{
    isCrouching = true;
    collider.size = crouchedColliderSize;
    collider.offset = crouchedColliderOffset;


}
else
{
    isCrouching = false;
    collider.size = initialColliderSize;
    collider.offset = initialColliderOffset;

}
///////////////////////////////
if (Input.GetKeyDown(KeyCode.C))
{
    isHeal = true;

    if(gM.Potions > 0)
    {
        gM.removeOnePotion();
    }

}else
{
    isHeal = false;
}


if (moveX < 0)
{
    moveX = -1;
    transform.localScale = new Vector2(-1f, 1f);
}
else if (moveX > 0)
{
    moveX = 1;
    transform.localScale = new Vector2(1f, 1f);
}
#endregion



// gestione dell'input dello sparo
if (Input.GetButtonDown("Fire2"))
{
    if(Less.currentMana > 0)
        {
    anim.SetTrigger("isShoot");
        }
}


        // gestione dell'input del salto
  if (Input.GetButtonDown("Jump") && jumpCounter < maxJumps)
{
    //SetState(3);
    isJumping = true;
    isGrounded = false; // vero se il personaggio sta saltando
    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    jumpCounter++;
    if(jumpCounter == 2)
    {
        Smagic.Play();
        Instantiate(Circle, circlePoint.position, transform.rotation);
    }
}

if (isJumping)
{
    // Se il personaggio sta cadendo, aumenta la velocità di discesa
    if (rb.velocity.y < 0)
    {
        isFall = true;            
        isLoop = true;
        rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

    }

    if (Input.GetButtonDown("Fire1"))
    {
        Attack();
        isAttacking = true;
        isJumping = false;
        isFall = true;
        isLoop = true;

    }
    
   
    }


        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                isAttacking= false;
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

if (Input.GetButton("Fire3")&& !dashing && coolDownTime <= 0)
        {
            dashing = true;
            coolDownTime = dashCoolDown;
            dashTime = dashDuration;
        }

        if (coolDownTime > 0)
        {
            coolDownTime -= Time.deltaTime;
        }



/*

        // gestione dell'input della corsa
        if (Input.GetButton("Fire3"))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        
        }*/

        // gestione dell'input del Menu 
        if (Input.GetKeyDown(KeyCode.Escape) && !stopInput)
        {
            gM.Pause();
            stopInput = true;
            //myAnimator.SetTrigger("idle");
            //SFX.Play(0);
            rb.velocity = new Vector2(0f, 0f);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && stopInput)
        {
            gM.Resume();
            stopInput = false;
        }
        // gestione dell'animazione del personaggio
        anim.SetFloat("Speed", Mathf.Abs(moveX));
        anim.SetBool("IsJumping", isJumping);
        anim.SetBool("IsFall", isFall);
        anim.SetBool("IsRunning", isRunning);
        anim.SetBool("Dash", dashing);
        anim.SetBool("Crouch", isCrouching);
        anim.SetBool("IsHeal", isHeal);


    }
    }

    
    private void FixedUpdate()
    {
        if (dashing)
        {
            if (moveX < 0)
        {

           rb.AddForce(-transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (moveX > 0)
        {
            //anim.SetTrigger("Dash");

            rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (moveX == 0)
        {
            
        }

            if (dashTime <= 0)
            {
                dashing = false;
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
    Instantiate(blam, gun.position, transform.rotation);
    Instantiate(bullet, gun.position, transform.rotation);
    }    
}


void evocationSword()
{
    Smagic.Play();
    Instantiate(EvocationSword, gun.position, transform.rotation);
    
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
            if (comboCounter == 1)
            {
            }else if (comboCounter == 2)
            {
            }else if (comboCounter == 3)
            {

            }
            currentCooldown = attackCooldown;
            comboTimer = 0.5f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // vero se il personaggio sta saltando
            isFall = false;
            isJumping = false;
            jumpCounter = 0;
            StartCoroutine(stopPlayer());

        }

    }

private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            RespawnObject respawnObject = collision.GetComponent<RespawnObject>();
            if (respawnObject != null)
            {
                respawnPoint = respawnObject.respawnPoint;
                sceneName = respawnObject.sceneName;
            }
        }


//Test per gestire il respawn
        if (collision.CompareTag("EditorOnly"))
        {
           Respawn();
        }
    }



IEnumerator stopPlayer()
{
isLanding = true; 
yield return new WaitForSeconds(0.5f);
isLoop = false;
isLanding = false;    
}

public void AnmHurt()
{
            anim.SetTrigger("TakeDamage");
}

#region CambioMagia
    public void SetBulletPrefab(GameObject newBullet)
    //Funzione per cambiare arma
    {
       bullet = newBullet;
    }    
    
#endregion


    



public void Respawn()
{
    // Cambia la scena
    SceneManager.LoadScene(sceneName);

    // Aspetta che la nuova scena sia completamente caricata
    StartCoroutine(WaitForSceneLoad());
}

IEnumerator WaitForSceneLoad()
{
    yield return new WaitForSeconds(0);

    // Trova l'oggetto con il tag "respawn" nella nuova scena
    GameObject respawnPoint = GameObject.FindWithTag("Respawn");

    // Teletrasporta il giocatore alla posizione dell'oggetto "respawn"
    transform.position = respawnPoint.transform.position;
}
}



           


