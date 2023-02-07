using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Spine.Unity;
using Spine;

public class PlayerHealth : MonoBehaviour
{
       public float maxHealth = 100f;
    public float currentHealth;
    public Scrollbar healthBar;
    [SerializeField] public GameplayManager gM;

    public float maxMana = 100f;
    public float currentMana;
    public Scrollbar manaBar;
    private bool isHeal = false; // vero se il personaggio sta correndo

    CharacterController2D player;

    [Header("Animations")]
    private Animator anim; // componente Animator del personaggio
    public SkeletonMecanim skeletonM;

        public static PlayerHealth instance;
        //Instanza per interagire con altri script




    void Start()
    {
        if (gM == null)
        {
            gM = GetComponent<GameplayManager>();
        }
        anim = GetComponent<Animator>();
        instance = this;
        currentHealth = maxHealth;
        currentMana = maxMana;
        player = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        healthBar.size = currentHealth / maxHealth;
        manaBar.size = currentMana / maxMana;
        healthBar.size = Mathf.Clamp(healthBar.size, 0.01f, 1);
        manaBar.size = Mathf.Clamp(manaBar.size, 0.01f, 1);


    ///////////////////////////////
    //Quando il player preme C
if (Input.GetKeyDown(KeyCode.C))
{
    //Si sta curando
    isHeal = true;
    //Se le pozioni sono maggiori di 0 e gli hp non sono al massimo ne toglie una
    if(gM.Potions > 0 && currentHealth != maxHealth)
    {
        gM.removeOnePotion();
        currentHealth += 50;

    }
    //Se le pozioni sono maggiori di 0 ma gli hp sono al massimo
    else if(gM.Potions > 0 && currentHealth == maxHealth)
    {
        //non succede nulla
    }


}else
{
    //Non si sta curando
    isHeal = false;
}

//TESTBUTTON

if (Input.GetKeyDown(KeyCode.T))
{
    player.GetComponent<PlayerHealth>().Damage(50);
}
//////////


    anim.SetBool("IsHeal", isHeal);

}

    public void Damage(float damage)
    {
        player.AnmHurt();
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            player.Respawn();
            RespawnStatus();
        }
    }

    public void TakeManaDamage(float damage)
    {
        currentMana -= damage;
        if (currentMana <= 0)
        {
            OutOfMana();
        }
    }

    void RespawnStatus()
    {
        // gestione della morte del personaggio
        currentHealth = maxHealth;
        currentMana = maxMana;
    }


    void OutOfMana()
    {
        // gestione del consumo completo della mana
    }
}