using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   // Enemy Unit Class that hold all properties of the enemy
   public class EnemyUnit
   {
      public int Health; // Health
      public int Damage; // Damage that will cause to main player
      public int Score; // Score amount when getting destroyed   
      public int DropAmount; // Number of coins will be dropped
      public bool IsDead; // to check if the enemy died or not.
   }

   public EnemyUnit EnemyUnt = new EnemyUnit();

   /* Using custom public constructor here to avoid using "new Enemy.EnemyUnit()" so we can call direct
      SetEnemyProperties*/
   public void SetEnemyProperties(int health, int damage, int score, int dropAmount, bool isDead)
   {
      EnemyUnt.Health = health;
      EnemyUnt.Damage = damage;
      EnemyUnt.Score = score;
      EnemyUnt.DropAmount = dropAmount;
      EnemyUnt.IsDead = isDead;
   }

   // Called when mainplayer do damage to enemy
   public virtual void ReceiveDamage(int damage)
   {
      EnemyUnt.Health -= damage;
      if (EnemyUnt.Health <= 0 && !EnemyUnt.IsDead)
      {
         EnemyUnt.IsDead = true;
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
