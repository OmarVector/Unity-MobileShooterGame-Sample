using UnityEngine;

public class ParticlesCallback : MonoBehaviour
{
   private void OnParticleSystemStopped()
   {
      ExplosionPoolManager.explosionPoolManager.ReturnEnemyToPool(gameObject);
   }
}
