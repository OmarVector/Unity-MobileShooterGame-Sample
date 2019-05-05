using UnityEngine;

public class MainplayerClass : MonoBehaviour
{
    [SerializeField] private GameObject ship;
   public int health;

   public void RecieveDamage(int damage)
    {
        health = health - damage;
        if (health < 0)
            OnDeath();
    }

    void OnDeath()
    {
        ScoreAndDropsManager.scoreAndDropsManager.LevelFinished("LOSER");
        ExplosionPoolManager.explosionPoolManager.PlayTerrainParticles(transform);
        ship.SetActive(false);
        
    }
}