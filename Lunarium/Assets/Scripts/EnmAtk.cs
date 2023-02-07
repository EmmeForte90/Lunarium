using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmAtk : MonoBehaviour
{        
    public GameObject Hitbox;
    private Transform player;
    public float attackDamage = 10; // danno d'attacco

    // Start is called before the first frame update
    void Start()
    {
                player = GameObject.FindWithTag("Player").transform;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<PlayerHealth>().Damage(attackDamage);

        }
    }
}
