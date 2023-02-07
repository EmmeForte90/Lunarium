using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
   public float knockbackForce; // la forza del knockback
    public float knockbackTime; // il tempo di knockback
    public float jumpHeight; // l'altezza del salto
    public float fallTime; // il tempo di caduta
    CharacterController2D player; // l'oggetto del player
Rigidbody2D rb;
void Awake()
{
            player = FindObjectOfType<CharacterController2D>();

}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hitbox")) // se l'oggetto colpito ha la tag "Hitbox"
        {
             rb = GetComponent<Rigidbody2D>(); // prende il rigidbody del nemico
            if (rb != null)
            {
                StartCoroutine(JumpBackCo(rb)); // avvia la routine per il salto
            }
        }
    }

    private IEnumerator JumpBackCo(Rigidbody2D rb)
    {
        if (rb != null)
        {
            Vector2 knockbackDirection = new Vector2(0f, jumpHeight); // direzione del knockback verso l'alto
            if (rb.transform.position.x < player.transform.position.x) // se la posizione x del nemico è inferiore a quella del player
                knockbackDirection = new Vector2(-1, jumpHeight); // la direzione del knockback è verso sinistra
            else if (rb.transform.position.x > player.transform.position.x) // se la posizione x del nemico è maggiore a quella del player
                knockbackDirection = new Vector2(1, jumpHeight); // la direzione del knockback è verso destra
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // applica il knockback
            yield return new WaitForSeconds(knockbackTime); // aspetta il tempo di knockback
        }
    }
}
