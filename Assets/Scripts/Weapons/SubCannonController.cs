using UnityEngine;

// This a Sub Cannon Controller , it only controller the behavior of the particles .
// By design , I did it like that so we can attach as many sub cannon to EnemyWeaponControllers and drive them from only one script per Enemy.
public class SubCannonController : MonoBehaviour
{
    [SerializeField] private EnemyWeaponController enemyWeaponController;

   
    private void OnParticleCollision(GameObject go)
    {
        var player = go.GetComponent<MainplayerClass>();
        if(player!=null)
            player.RecieveDamage(enemyWeaponController.damage);
    }
}
