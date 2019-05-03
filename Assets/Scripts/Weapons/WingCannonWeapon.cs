using UnityEngine;

// WingCannon Class, it may seems same as MainCannon bec I didnt add much detail yet.
public class WingCannonWeapon : WEAPON
{
    // this base damage for our main cannon.
    private static int BASEDAMAGE = 200;

    protected override void Start()
    {
        // Setting damage value from current WingCannonLevel from ShipDataManager
        damage = (ShipDataManager.shipDataManager.WingCannonLevel * 10) + BASEDAMAGE;
        
        //playing the particle system of the main cannon 
        GetComponent<ParticleSystem>().Play();
        
        // setting power increment , different from weapon to weapon .
        powerIncrease = 0.1f;
    }

    // Called when player pick a power up during gameplay
    public override void PowerUp()
    {
        fireRate += powerIncrease;
        GetComponent<ParticleSystem>().emissionRate = fireRate;
        Debug.Log("<b><color=Green> Wing Cannon Fire Rate = </color></b>");
    }
}
