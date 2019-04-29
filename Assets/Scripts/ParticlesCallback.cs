using UnityEngine;

public class ParticlesCallback : MonoBehaviour
{
   private void OnParticleSystemStopped()
   {
      ExplosionsPool.explosionsPool.ReturnEnemyToPool(gameObject);
   }
}
