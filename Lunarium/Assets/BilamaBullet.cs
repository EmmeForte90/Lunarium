using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilamaBullet : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float gravity = 9.8f;
    public int numberOfBounces = 3;
    public float rotationSpeed = 360f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private float startTime;
    private int bounceCount = 0;
    [SerializeField] float MPCost = 30f;
    PlayerHealth Less;
    CharacterController2D player;
    private float lifeTime = 0.5f;
    [SerializeField] GameObject Explode;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        startTime = Time.time;
        player = FindObjectOfType<CharacterController2D>();
        Less = FindObjectOfType<PlayerHealth>();
        if(player.transform.localScale.x > 0)
        {
                    rb.velocity = new Vector2(initialSpeed, initialSpeed);

        } else if(player.transform.localScale.x < 0)
        {
                    rb.velocity = new Vector2(-initialSpeed, initialSpeed);

        }
        CostMP();


    }

private void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            startPosition = transform.position;
            startTime = Time.time;
            bounceCount++;
            initialSpeed = -initialSpeed;
            rb.velocity = new Vector2(initialSpeed, initialSpeed);
        }

        if (bounceCount >= numberOfBounces)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }


#region  MP
    void CostMP()
    {
    
        Less.TakeManaDamage(MPCost);

        
    }

#endregion



void OnTriggerEnter2D(Collider2D other)
{
    if(other.gameObject.tag == "Enemy")
        //Se il proiettile tocca il nemico
        {  
            Instantiate(Explode, transform.position, transform.rotation);
            int damage = 10;
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);
        }

}
}
