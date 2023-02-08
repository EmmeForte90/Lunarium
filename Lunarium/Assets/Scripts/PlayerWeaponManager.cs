using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Usa l'input system di Unity scaricato dal packet manager


public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager instance;
   [Header("Proiettili")]
    [SerializeField] private GameObject dagger;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject axe;
    [SerializeField] private GameObject lance;
    [SerializeField] private GameObject bilama;

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
        playerShootScript = GetComponent<PlayerAttack>();
        instance = this;
        currentWeaponIndex = 1;
    }
void Update()
{
     if (Input.GetKey(_button) && Time.time - lastWeaponChangeTime > cooldown)
    {
        OnChangeWeapon();
        lastWeaponChangeTime = Time.time;
    }
    
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

