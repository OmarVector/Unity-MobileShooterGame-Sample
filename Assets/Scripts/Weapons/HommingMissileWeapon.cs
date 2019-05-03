using UnityEngine;

public class HommingMissileWeapon : WEAPON
{
    // this base damage for our Homming Missles.
    private static int BASEDAMAGE = 500;


    protected override void Start()
    {
        // Setting damage value from current RocketLevel from ShipDataManager , Its hardcoded for now .
        damage = (ShipDataManager.shipDataManager.RocketLevel * 5) + BASEDAMAGE;

        // setting power increment , different from weapon to weapon .
        powerIncrease = 0.1f;
    }

    // Called when player pick a power up during gameplay
    public override void PowerUp()
    {
        fireRate += powerIncrease;
    }

    // once the Missile hit a target it get destoryed
    private void OnCollisionEnter(Collision other)
    {
        var enemy = other.gameObject.GetComponent<ENEMY>();
        if(enemy!=null)
            enemy.ReceiveDamage(damage);
        
        Destroy(gameObject);
    }
}