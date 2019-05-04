using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class AirUnitEnemy : ENEMY
{
    // player air explosion particles when air unit get destroyed.
    public override void OnDeath()
    {
        ExplosionPoolManager.explosionPoolManager.PlayAirParticle(gameObject.transform);
        base.OnDeath();
    }

    // destroy air unit when it collides when player and cause damage to player
    private void OnCollisionEnter(Collision collision)
    {
        var mainPlayer = collision.gameObject.GetComponent<MainplayerClass>();
        if (mainPlayer != null)
        {
            mainPlayer.RecieveDamage(Damage);
            OnDeath();
        }
    }

    // called when dotween animation path ends
    public override void OnStop()
    {
        gameObject.SetActive(false);
    }
}
