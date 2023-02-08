using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GhoulKnight : Health, IDamegable
{
    public Transform pointA, pointB;
    public float moveSpeed = 2f;
    public float attackRadius = 1f;
    public GameObject Brain;
    private int currentWaypointIndex = 0;
    private Transform target;
    private Animator animator;
    private bool movingToA = false;
    Health health;
    Rigidbody2D rb;
    GameplayManager gM;
    private Transform player;
public float attackrange = 2f;
private float attackTimer;
public float attackCooldown = 2f; // durata del cooldown dell'attacco
private SkeletonMecanim skeletonMecanim;
    private Color originalColor;

    [SerializeField] GameObject DeathBack;
    [SerializeField] GameObject Death;
 private float pauseDuration = 1f; // durata della pausa
private float pauseTimer; // timer per la pausa
  private bool IsAttacking = false; // indica se il nemico sta inseguendo il player
    [SerializeField] bool isLittle = false; // indica se il nemico sta inseguendo il player

[Header("Audio")]
[SerializeField] AudioSource SwSl;
[SerializeField] AudioSource SDie;



private enum State { Move, Attack, Dead }
private State currentState;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        skeletonMecanim = GetComponent<SkeletonMecanim>();
        originalColor = skeletonMecanim.Skeleton.GetColor();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        

    if (gM == null)
    {
        gM = GameObject.FindObjectOfType<GameplayManager>();
        //gM = GameObject.FindObjectOfType<GameplayManager>();
    }
    }

    void Update()
    {
         if (!gM.PauseStop)
        {
           CheckState();
            switch (currentState)
            {
                case State.Move:
                    Move();
                    break; 
                case State.Attack:
                    Attack();
                    break;
                case State.Dead:
                    break;
            }

        
        }
    }

    void Move()
    {

        animator.SetBool("isMove", true);
    //animator.SetBool("isChasing", false); // imposta la variabile booleana "IsChasing" dell'animatore a true
        if (movingToA)
        {
            transform.localScale = new Vector2(-1f, 1f);
            if (pauseTimer > 0)
            {
                animator.SetBool("isMove", false);
                //animator.SetBool("isChasing", false);
                pauseTimer -= Time.deltaTime;
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, pointA.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
            {
                pauseTimer = pauseDuration;
                movingToA = false;
            }
        }
        else
        {
            transform.localScale = new Vector2(1f, 1f);
            if (pauseTimer > 0)
            {
                animator.SetBool("isMove", false);
                //animator.SetBool("isChasing", false);
                pauseTimer -= Time.deltaTime;
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, pointB.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
            {
                pauseTimer = pauseDuration;
                movingToA = true;
            }
        }
    }

    void LookAtPlayer()
    {
        if (player.transform.position.x > transform.position.x)
    {
        transform.localScale = new Vector2(1f, 1f);
    }
    else if (player.transform.position.x < transform.position.x)
    {
        transform.localScale = new Vector2(-1f, 1f);
    }
    }

private void CheckState()
{
    if (health.currentHealth == 0)
    {
        currentState = State.Dead;
        return;
    }

    if (Vector2.Distance(transform.position, player.position) < attackrange)
    {
        currentState = State.Attack;
        return;
    }

    currentState = State.Move;
}

private void Attack()
{
    LookAtPlayer();
    animator.SetTrigger("attack");
    // gestione dell'attacco del nemico
    if (attackTimer > 0)
    {
        attackTimer -= Time.deltaTime;
        return;
    }
    attackTimer = attackCooldown;
}

    #region Gizmos
private void OnDrawGizmos()
    {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackrange);
    }
#endregion


public void DeathAnim()
{
           SDie.Play();


    // animazione di morte
    // determina la direzione della morte in base alla posizione del player rispetto al nemico
        if (movingToA)//transform.position.x < player.position.x)
        {
            Instantiate(Death, transform.position, transform.rotation);
            //EnemyManager.UnregisterEnemy(this);
            Destroy(Brain);

            //animator.SetTrigger("Die"); // attiva il trigger "DieFront" dell'animatore per la morte davanti al player
        }
        else if (!movingToA)
        {
            Instantiate(DeathBack, transform.position, transform.rotation);
            //EnemyManager.UnregisterEnemy(this);
            Destroy(Brain);

            //animator.SetTrigger("Die_1"); // attiva il trigger "DieBack" dell'animatore per la morte dietro al player
        }
}
public void Damage(int damage)
    {
        health.currentHealth -= damage;
        anmHurt();
        
    }

public void anmHurt()
    {
        TemporaryChangeColor(Color.red);
        if(isLittle)
        {
        animator.SetTrigger("TakeDamage"); // attiva il trigger "TakeDamage" dell'animatore
        }
        if (rb != null)
            {
                //StartCoroutine(JumpBackCo(rb)); // avvia la routine per il salto
            }
        if (health.currentHealth <= 0f) // se la salute è uguale o inferiore a 0
        {
            DeathAnim();
        }
    }

public void TemporaryChangeColor(Color color)
    {
        skeletonMecanim.Skeleton.SetColor(color);
        Invoke("ResetColor", 0.5f);
    }

    private void ResetColor()
    {
        skeletonMecanim.Skeleton.SetColor(originalColor);
    }


public void Sword()
    {
        SwSl.Play();
    } 
}