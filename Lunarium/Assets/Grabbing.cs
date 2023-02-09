using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.Audio;

public class Grabbing : MonoBehaviour
{

    BoxCollider2D collider;
    public CharacterController2D player;
    [Header("Animations")]
    public Animator anim; // componente Animator del personaggio
    public SkeletonMecanim skeletonM;
    private bool isGrabbingEdge = false;
    [Header("Audio")]
[SerializeField] AudioSource Grab;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) {
        Debug.LogError("Object reference not set to an instance of an object");
        return;
    }
        if (collision.tag == "Edge") {
        player.rb.velocity = Vector2.zero;
        player.rb.gravityScale = 0f;
        isGrabbingEdge = true;
        anim.SetBool("Grab", isGrabbingEdge);
        Grab.Play();


    }
    }

private void OnTriggerExit2D(Collider2D collider) {
    if (collider == null) {
        Debug.LogError("Object reference not set to an instance of an object");
        return;
    }
    if (collider.tag == "Edge") {
        player.rb.gravityScale = 1f;
        isGrabbingEdge = false;
        anim.SetBool("Grab", isGrabbingEdge);
    }
}

}

