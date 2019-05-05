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
    private EnemyGroup enemyGroup; // Reference to enemyGroup

    [SerializeField] private EnemyWeaponController weapon; // Reference to weapon attached if any
    
    // Using custom public constructor here 
    public void SetEnemyProperties(int health, int damage, int score, int dropAmount, bool isDead, EnemyGroup enemyGrp)
    {
        Health = health;
        Damage = damage;
        Score = score;
        DropAmount = dropAmount;
        IsDead = isDead;
        enemyGroup = enemyGrp;

        // not all units have a weapon, //TODO developing better design to avoid allocating null object for units without a weapon.
        if (weapon)
            weapon.damage = Damage;
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

        RandomDropControl();
        ScoreAndDropsManager.scoreAndDropsManager.Score += Score;
        ScoreAndDropsManager.scoreAndDropsManager.UpdateScore();
        enemyGroup.ScoreBonusCheck();
        // removing enemy from radar list
        RocketRadar.rocketRadar.EnemyTransform.Remove(transform);

        gameObject.SetActive(false);
    }

    // Called when the animation of TweenPath ended.
    public virtual void OnStop()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // once the enemy target enter the Radar trigger, it added it's Transform to Radar target list.
        RocketRadar.rocketRadar.EnemyTransform.Add(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        // Once it leave the radar trigger, it remove itself from enemy target list
        RocketRadar.rocketRadar.EnemyTransform.Remove(transform);
    }

    private void OnDisable()
    {
        // Once it become disabled, it remove itself from enemy target list
        RocketRadar.rocketRadar.EnemyTransform.Remove(transform);
        // Culling any attached weapons
        if (weapon)
            weapon.Cull();
    }

    private void RandomDropControl()
    {
        var x = Random.Range(0, 100);

        if (x < 80)
        {
            for (int i = 0; i < DropAmount; ++i)
            {
                ScoreAndDropsManager.scoreAndDropsManager.GetCoin(transform.position);
            }
            
            return;
        }

        if (x > 80 && x < 90)
        {
            for (int i = 0; i < DropAmount; ++i)
            {
                ScoreAndDropsManager.scoreAndDropsManager.GetHealthKit(transform.position);
            }
            
            return;
        }

        if (x > 90 && x < 95)
        {
            for (int i = 0; i < DropAmount; ++i)
            {
                ScoreAndDropsManager.scoreAndDropsManager.GetShield(transform.position);
            }
            
            return;
        }

        if (x > 95)
        {
            for (int i = 0; i < DropAmount; ++i)
            {
                ScoreAndDropsManager.scoreAndDropsManager.GetLaser(transform.position);
            }
        }
            
    }
   
}