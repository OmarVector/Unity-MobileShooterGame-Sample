using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCannonWeapon : WEAPON
{
    
    // this base damage for our main cannon.
    private static int BASEDAMAGE = 100;

    protected override void Start()
    {
        // Setting damage value from current MainCannonLevel from ShipDataManager
        power = (ShipDataManager.shipDataManager.MainCannonLevel * 10) + BASEDAMAGE;
        
        //playing the particle system of the main cannon 
        var part = GetComponent<ParticleSystem>();
        part.Play();
        fireRate = part.emissionRate;
        // setting power increment , different from weapon to weapon . Hardcoded for now
        powerIncrease = 3.75f;
    }

    // Called when player pick a power up during gameplay
    public override void PowerUp()
    {
        fireRate += powerIncrease;
        GetComponent<ParticleSystem>().emissionRate = fireRate;
        Debug.Log("<b><color=Green> Main Cannon Fire Rate </color></b>");
    }
}