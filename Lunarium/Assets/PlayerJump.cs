using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class PlayerJump : MonoBehaviour
{

    [SerializeField] public float jumpForce = 5f; // forza del salto
    private int jumpCounter = 0;
    private int maxJumps = 2;
    private float jumpDuration = 0.5f;
    public float fallMultiplier = 2.5f;
    float coyoteTime = 0.1f;
    float coyoteCounter = 0f;
    bool isGrounded = true;
    [Header("HP")]
    [SerializeField]public float health = 100f; // salute del personaggio
    PlayerHealth Less;
    PlayerAttack Atk;

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
    private bool isJumping = false; // vero se il personaggio sta saltando
    private bool isFall = false; // vero se il personaggio sta saltando
    private bool isHurt = false; // vero se il personaggio sta saltando
    private bool isLoop = false; // vero se il personaggio sta saltando
    private bool isAttacking = false; // vero se il personaggio sta attaccando
    private bool isCrouching = false; // vero se il personaggio sta attaccando
    private bool isLanding = false; // vero se il personaggio sta attaccando
    private bool isRunning = false; // vero se il personaggio sta correndo
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
    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true; // vero se il personaggio sta saltando
        rb = GetComponent<Rigidbody2D>();
        if (gM == null)
        {
            gM = GetComponent<GameplayManager>();
        }
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gM.PauseStop)
        {
        if (isGrounded)
    {
        coyoteCounter = 0f;
    }
    else
    {
        coyoteCounter += Time.deltaTime;
    }

    if (Input.GetButtonDown("Jump") && (jumpCounter < maxJumps || coyoteCounter < coyoteTime))
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
     anim.SetBool("IsJumping", isJumping);
        anim.SetBool("IsFall", isFall);
        }
    }




    IEnumerator stopPlayer()
{
isLanding = true; 
yield return new WaitForSeconds(0.5f);
isLoop = false;
isLanding = false;    
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
}
