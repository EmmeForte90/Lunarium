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
    public float speed = 5f;
    public float runSpeed = 10.0f;
    private float runThreshold = 1f;
    private float runFactor = 0.0f;
    public float maxSpeed;
    public bool isRunning;

    private float currentSpeed; // velocità corrente del personaggio
    private float accelerationSpeed = 100f;


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
    
    CapsuleCollider2D collider;
    private Vector2 initialColliderSize;
    private Vector2 crouchedColliderSize;
    private Vector2 initialColliderOffset;
    private Vector2 crouchedColliderOffset;
    public Vector2 direction;

    [Header("HP")]
    [SerializeField]public float health = 100f; // salute del personaggio
    PlayerHealth Less;
    PlayerAttack Atk;
    PlayerJump Jmp;



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
    private bool isHurt = false; // vero se il personaggio sta saltando
    private bool isLoop = false; // vero se il personaggio sta saltando
    private bool isAttacking = false; // vero se il personaggio sta attaccando
    private bool isCrouching = false; // vero se il personaggio sta attaccando
    private bool isLanding = false; // vero se il personaggio sta attaccando
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
        playerPosition = transform.position;
        HitPosition = Hit.transform.position;
        Less = GetComponent<PlayerHealth>();
        Atk = GetComponent<PlayerAttack>();
        Jmp = GetComponent<PlayerJump>();
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

        runFactor += Time.deltaTime;
        runFactor = Mathf.Min(runFactor, 5.0f);

        float currentSpeed = speed;
        if (runFactor >= runThreshold)
        {
            currentSpeed = runSpeed;
            isRunning = true;
        }
        if  (moveX == 0)
        {
            runFactor = 0;
            isRunning = false;
        }

        transform.position = transform.position + new Vector3(moveX * currentSpeed * Time.deltaTime, 0, 0);
    








if (Atk.isAttacking || Jmp.isLanding)
{
    rb.velocity = new Vector2(0f, 0f);
}
else
{
    rb.velocity = new Vector2(moveX * currentSpeed, rb.velocity.y);
}

/////////////////////////////////
if (Input.GetButton("Vertical"))
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
///////////////////////////////////

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

        }



        // gestione dell'input del Menu 
        if (Input.GetButtonDown("Pause") && !stopInput)
        {
            gM.Pause();
            stopInput = true;
            //myAnimator.SetTrigger("idle");
            //SFX.Play(0);
            rb.velocity = new Vector2(0f, 0f);
        }
        else if(Input.GetButtonDown("Pause") && stopInput)
        {
            gM.Resume();
            stopInput = false;
        }


        // gestione dell'animazione del personaggio
        anim.SetFloat("Speed", Mathf.Abs(moveX));
        anim.SetBool("IsRunning", isRunning);
        anim.SetBool("Dash", dashing);
        anim.SetBool("Crouch", isCrouching);


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