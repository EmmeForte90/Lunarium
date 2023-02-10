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

    public float moveX;
    [SerializeField] public float moveSpeed = 5f; // velocità di movimento
    [SerializeField] public float jumpForce = 5f; // forza del salto
    [SerializeField] public float runMultiplier = 2f; // moltiplicatore di velocità per la corsa
    private int jumpCounter = 0;
    private int maxJumps = 2;
    private float jumpDuration = 0.5f;
    public float fallMultiplier = 2.5f;

    private float accelerationSpeed = 100f;

    private float maxSpeed = 100f;

    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;

    Vector2 playerPosition;
    Vector2 HitPosition;
    public GameObject Hit;
    public GameObject DashEff;
    [SerializeField] public Transform dash;

    public float dashForce = 50f;
    public float dashDuration = 0.5f;
    private float dashTime;
    private bool dashing;
    private bool Atkdashing;
    private float dashForceAtk = 40f;
    private bool attackNormal;


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
    PlayerAttack Atk;



    [Header("Respawn")]
    //[HideInInspector]
    private Transform respawnPoint; // il punto di respawn del giocatore
    public string sceneName; // il nome della scena in cui si trova il punto di respawn
    
    [Header("Animations")]
    private Animator anim; // componente Animator del personaggio
    public SkeletonMecanim skeletonM;


    [Header("VFX")]
    // Variabile per il gameobject del proiettile
    [SerializeField] GameObject Circle;
        [SerializeField] GameObject dust;
        [SerializeField] GameObject DustImp;

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
    private float currentSpeed; // velocità corrente del personaggio
    public Rigidbody2D rb; // componente Rigidbody2D del personaggio
    [SerializeField] public static bool playerExists;
    [SerializeField] public bool blockInput = false;
   
[Header("Audio")]
[SerializeField] AudioSource SwSl;
[SerializeField] AudioSource Smagic;
[SerializeField] AudioSource Srun;
[SerializeField] AudioSource Swalk;
[SerializeField] AudioSource SGrab;
[SerializeField] AudioSource SCrash;
[SerializeField] AudioSource Sdash;
[SerializeField] AudioSource Shop;
[SerializeField] AudioSource Surg;






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
        Atk = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        if (gM == null)
        {
            gM = GetComponent<GameplayManager>();
        }
        anim = GetComponent<Animator>();
        
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

        if(!gM.PauseStop)
        {

        #region Move
        moveX = Input.GetAxis("Horizontal");
        currentSpeed = moveSpeed;
        if (isRunning && !Atk.isAttacking)
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
if (Atk.isAttacking || isLanding)
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


/*
// gestione dell'input dello sparo
if (Input.GetButtonDown("Fire2"))
{
    if(Less.currentMana > 0)
        {
    anim.SetTrigger("isShoot");
        }
}*/


        // gestione dell'input del salto
  if (Input.GetButtonDown("Jump") && jumpCounter < maxJumps)
    {
        isJumping = true;
        isGrounded = false;
        jumpCounter++;
        rb.velocity = Vector2.up * jumpForce;

        if (jumpCounter == 2)
        {
            Smagic.Play();
            Instantiate(Circle, circlePoint.transform.position, transform.rotation);
        }

        StartCoroutine(JumpDurationCoroutine(jumpDuration));
    }

    if (rb.velocity.y < 0)
    {
        isFall = true;
        isLoop = true;
        rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }

    if (Input.GetButtonDown("Fire1") && isJumping)
    {
        Atk.isAttacking = true;
        isJumping = false;
        isFall = true;
        isLoop = true;
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


    }
    }

    
    private void FixedUpdate()
    {
        if (dashing || Atkdashing)
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
            if (rb.transform.localScale.x == -1)
        {

           rb.AddForce(-transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (rb.transform.localScale.x == 1)
        {
            //anim.SetTrigger("Dash");

            rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
                //dashing = false;
                //Atkdashing = false;
        }

            if (dashTime <= 0)
            {
                dashing = false;
                Atkdashing = false;

            }
        }

        if (attackNormal)
        {
            if (moveX < 0)
        {

           rb.AddForce(-transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (moveX > 0)
        {
            //anim.SetTrigger("Dash");

            rb.AddForce(transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (moveX == 0)
        {
                dashing = false;
                attackNormal = false;
        }

            if (dashTime <= 0)
            {
                dashing = false;
                attackNormal = false;

            }
        }
    }

private IEnumerator JumpDurationCoroutine(float duration)
{
    float timer = 0f;
    while (timer < duration)
    {
        timer += Time.deltaTime;
        yield return null;
    }

    isJumping = false;
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

public void dustImpact()
{
       SGrab.Play();
   Instantiate(DustImp, circlePoint.transform.position, transform.rotation);
}

public void Dust()
{
       Srun.Play();
   Instantiate(dust, circlePoint.transform.position, transform.rotation);
}

public void Hop()
{
       Shop.Play();
}



public void SoundUrgh()
{
       Surg.Play();
}

public void walk()
{   
    Swalk.Play();
   Instantiate(dust, circlePoint.transform.position, transform.rotation);
}

public void dashEff()
{
   Sdash.Play();
   Instantiate(DashEff, dash.position, transform.rotation);
}

public void MovingAtk() {
       
    //attackNormal = true;
    //coolDownTime = dashCoolDown;
    //dashTime = dashDuration;

    /*if (moveX < 0)
        {

           rb.AddForce(-transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (moveX > 0)
        {
            //anim.SetTrigger("Dash");

            rb.AddForce(transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (moveX == 0)
        {
            if (rb.transform.localScale.x == -1)
        {

           rb.AddForce(-transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (rb.transform.localScale.x == 1)
        {
            //anim.SetTrigger("Dash");

            rb.AddForce(transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
                //dashing = false;
                //Atkdashing = false;
        }*/
   Instantiate(DashEff, dash.position, transform.rotation);
    currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, accelerationSpeed * Time.deltaTime);
    transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
}

public void movinglong()
{
    Atkdashing = true;
    coolDownTime = dashCoolDown;
    dashTime = dashDuration;
   Instantiate(DashEff, dash.position, transform.rotation);

    
}

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



           


