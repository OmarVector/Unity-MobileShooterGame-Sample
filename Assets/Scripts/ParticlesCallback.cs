using UnityEngine;

public class ParticlesCallback : MonoBehaviour
{
   [SerializeField] private bool isAir;
   private void OnParticleSystemStopped()
   {
      ExplosionPoolManager.explosionPoolManager.ReturnEnemyToPool(gameObject,isAir);
   }
}
