using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;


public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager instance;
   [Header("Proiettili")]
    [SerializeField] private GameObject dagger;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject axe;
    [SerializeField] private GameObject lance;
    [SerializeField] private GameObject bilama;

    [Header("Animations")]
    private Animator anim; // componente Animator del personaggio
    public SkeletonMecanim skeletonM;
    public bool isDagger = false;
    public bool isSword = false;
    public bool isAxe = false; 
    public bool isLance = false;  
    public bool isBilama = false; 

    public KeyCode _button = KeyCode.F;
    private float cooldown = 0.5f;
    private float lastWeaponChangeTime;
    private int currentWeaponIndex;

    PlayerAttack playerShootScript;

    private void Awake()
    { 
        anim = GetComponent<Animator>();
        playerShootScript = GetComponent<PlayerAttack>();
        instance = this;
        currentWeaponIndex = 1;
        if(isDagger || isSword)
        {
            //playerShootScript.attackCooldown == 0.2f;
        }

    }
void Update()
{
     if (Input.GetKey(_button) && Time.time - lastWeaponChangeTime > cooldown)
    {
        OnChangeWeapon();
        lastWeaponChangeTime = Time.time;
    }
    anim.SetBool("isDagger", isDagger);
    anim.SetBool("isAxe", isAxe);
    anim.SetBool("isSword", isSword);
    anim.SetBool("isLance", isLance);
    anim.SetBool("isBilama", isBilama);

    }

#region ChangeWeapon
void OnChangeWeapon()
{
   
        int weaponIndex = (currentWeaponIndex + 1) % 5;
        SetWeapon(weaponIndex + 1);
        FindObjectOfType<ChangeWeaponAnimation>().ChangeWeapon(weaponIndex + 1);
        currentWeaponIndex = weaponIndex;
    
}

#endregion

public void SetWeapon(int WeaponID)
{
    isDagger = false;
    isSword = false;
    isAxe = false;
    isLance = false;
    isBilama = false;

    switch (WeaponID)
    {
        case 1:
            playerShootScript.SetBulletPrefab(dagger);
            isDagger = true;
            break;

        case 2:
            playerShootScript.SetBulletPrefab(sword);
            isSword = true;
            break;

        case 3:
            playerShootScript.SetBulletPrefab(axe);
            isAxe = true;
            break;

        case 4:
            playerShootScript.SetBulletPrefab(lance);
            isLance = true;
            break;

        case 5:
            playerShootScript.SetBulletPrefab(bilama);
            isBilama = true;
            break;
    }
        
    
    

 }
}

