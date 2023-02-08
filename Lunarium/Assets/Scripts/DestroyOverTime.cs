using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class DestroyOverTime : MonoBehaviour
{
    [Header("Parametri")]
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject thisObj;
    [SerializeField] private bool isDash;
    private CharacterController2D player;

    private void Awake()
    {
        player = FindObjectOfType<CharacterController2D>();
    }

    private void Update()
    {
        Destroy(thisObj, lifeTime);

        if (isDash)
        {
             Vector2 direction = player.transform.localScale.x < 0 ? new Vector2(-3f, 3f) : new Vector2(3f, 3f);
        transform.localScale = direction;
        }
    }

    void StopAnm()
    {
        
    }
}
