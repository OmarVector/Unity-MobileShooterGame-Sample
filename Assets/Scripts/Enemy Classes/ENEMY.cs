using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMY : MonoBehaviour
{
    // Enemy Unit Class that hold all properties of the enemy

    private int Health; // Health
    private int Damage; // Damage that will cause to main player
    private int Score; // Score amount when getting destroyed   
    private int DropAmount; // Number of coins will be dropped
    private bool IsDead; // to check if the enemy died or not.


    // Using custom public constructor here 
    public void SetEnemyProperties(int health, int damage, int score, int dropAmount, bool isDead)
    {
        Health = health;
        Damage = damage;
        Score = score;
        DropAmount = dropAmount;
        IsDead = isDead;
    }

    // Called when mainplayer do damage to enemy
    public virtual void ReceiveDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0 && !IsDead)
        {
            IsDead = true;
            OnDeath();
        }
    }

    // Called when enemy get destroyed.
    public virtual void OnDeath()
    {
        //TODO Adding score Logic.
        //TODO Drops
        // Checking Group Score
        Destroy(gameObject);
    }

    // Called when the animation of TweenPath ended.
    public virtual void OnStop()
    {
    }
}