using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxPlayer : MonoBehaviour
{
    [SerializeField] GameObject Clang;
    [SerializeField] GameObject Sdeng;
    [SerializeField] public Transform Pos;
    public CharacterController2D player;
    [SerializeField] public int attackDamage = 10;


void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        //Se il proiettile tocca il nemico
        {       
            Instantiate(Sdeng, Pos.transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(attackDamage);
        }
        if(other.gameObject.tag == "Ground")
        //Se il proiettile tocca il nemico
        {            
            Instantiate(Clang, Pos.transform.position, transform.rotation);
        }
        }
        

        
}