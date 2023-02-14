using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;


public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager instance;
   [Header("Weapons")]
    [SerializeField] private GameObject dagger;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject axe;
    [SerializeField] private GameObject lance;
    [SerializeField] private GameObject bilama;


    [Header("Evocations")]
    [SerializeField] private GameObject Bat;
    [SerializeField] private GameObject Golem;
    [SerializeField] private GameObject Knight;
    [SerializeField] private GameObject Healer;
    [SerializeField] private GameObject Unicorn;
    [SerializeField] private GameObject Dragon;

    [Header("Animations")]
    private Animator anim; // componente Animator del personaggio
    public SkeletonMecanim skeletonM;
    public bool isDagger = false;
    public bool isSword = false;
    public bool isAxe = false; 
    public bool isLance = false;  
    public bool isBilama = false; 

    public bool isBat = false;
    public bool isGolem = false;
    public bool isKnight = false; 
    public bool isHealer = false;  
    public bool isUnicorn = false; 
    public bool isDragon = false; 


    private float cooldown = 0.5f;
    private float lastWeaponChangeTime;
    private int currentWeaponIndex;

    private float lastEVChangeTime;
    private int currentEVIndex;

    PlayerAttack playerShootScript;

    private void Awake()
    { 
        anim = GetComponent<Animator>();
        playerShootScript = GetComponent<PlayerAttack>();
        instance = this;
        currentWeaponIndex = 1;
        

    }
void Update()
{
     if (Input.GetButtonDown("Change") && Time.time - lastWeaponChangeTime > cooldown)
    {
        OnChangeWeapon();
        lastWeaponChangeTime = Time.time;
        if(isDagger || isSword)
        {
            playerShootScript.attackCooldown = 0.3f;
            playerShootScript.comboTimer = 0.3f;
            playerShootScript.comboPauseDuration = 0.5f;

        }else if(isAxe || isBilama)
        {
            playerShootScript.attackCooldown = 0.4f;
            playerShootScript.comboTimer = 0.4f;
            playerShootScript.comboPauseDuration = 2f;


        }else if(isLance)
        {
            playerShootScript.attackCooldown = 0.4f;
            playerShootScript.comboTimer = 0.5f;
            playerShootScript.comboPauseDuration = 3f;

        }
    }

    if (Input.GetButtonDown("ChangeEv") && Time.time - lastEVChangeTime > cooldown)
    {
        OnChangeEv();
        lastEVChangeTime = Time.time;
        
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

#region ChangeEv
void OnChangeEv()
{
   
        int EvIndex = (currentEVIndex + 1) % 6;
        SetEv(EvIndex + 1);
        FindObjectOfType<ChangeWeaponAnimation>().ChangeEv(EvIndex + 1);
        currentEVIndex = EvIndex;
    
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


public void SetEv(int EvID)
{
     isBat = false;
     isGolem = false;
     isKnight = false; 
     isHealer = false;  
     isUnicorn = false; 
     isDragon = false; 

    switch (EvID)
    {
        case 1:
            playerShootScript.SetEvPrefab(Bat);
            isBat = true;
            break;

        case 2:
            playerShootScript.SetEvPrefab(Golem);
            isGolem = true;
            break;

        case 3:
            playerShootScript.SetEvPrefab(Knight);
            isKnight = true;
            break;

        case 4:
            playerShootScript.SetEvPrefab(Healer);
            isHealer = true;
            break;
        case 5:
            playerShootScript.SetEvPrefab(Unicorn);
            isUnicorn = true;
            break;

        case 6:
            playerShootScript.SetEvPrefab(Dragon);
            isDragon = true;
            break;
    }
 }

}

