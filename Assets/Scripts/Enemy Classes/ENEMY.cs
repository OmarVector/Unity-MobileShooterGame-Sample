using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMY : MonoBehaviour
{
    // Enemy Unit Class that hold all properties of the enemy

    protected int Health; // Health
    protected int Damage; // Damage that will cause to main player
    private int Score; // Score amount when getting destroyed   
    private int DropAmount; // Number of coins will be dropped
    private bool IsDead; // to check if the enemy died or not.
    protected EnemyGroup enemyGroup; // Reference to enemyGroup


    // Using custom public constructor here 
    public void SetEnemyProperties(int health, int damage, int score, int dropAmount, bool isDead, EnemyGroup enemyGrp)
    {
        Health = health;
        Damage = damage;
        Score = score;
        DropAmount = dropAmount;
        IsDead = isDead;
        enemyGroup = enemyGrp;
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
    protected virtual void OnDeath()
    {
        //TODO Adding score Logic.
        //TODO Drops
        enemyGroup.ScoreBonusCheck();
        Destroy(gameObject);
    }

    // Called when the animation of TweenPath ended.
    public virtual void OnStop()
    {
    }
}