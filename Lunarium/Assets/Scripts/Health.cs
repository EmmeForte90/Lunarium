using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    
        public static Health instance;
    //public int ID;
[Header("Sistema Di HP")]
    Enemy enemy;
   public float maxHealth = 100f;
    public float currentHealth = 100f;

void Start()
    {
        //EnemyManager.RegisterEnemy(this);
        enemy = FindObjectOfType<Enemy>();
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        enemy.anmHurt();
        
    }


}
