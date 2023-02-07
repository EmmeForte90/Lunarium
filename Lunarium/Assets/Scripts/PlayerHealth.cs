using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
       public float maxHealth = 100f;
    public float currentHealth;
    public Scrollbar healthBar;

    public float maxMana = 100f;
    public float currentMana;
    public Scrollbar manaBar;
    CharacterController2D player;

    
        public static PlayerHealth instance;
        //Instanza per interagire con altri script




    void Start()
    {
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