using DG.Tweening;
using UnityEngine;

// This is EnemyCannon Class script that control the behavior of the enemy offensive .
public class EnemyWeaponController : MonoBehaviour
{
    // Array of particles that fire on player
    [SerializeField] private ParticleSystem[] CannonParticles ;
    // Rate how the enemies will turn toward the player and shot
    [SerializeField] private float lockRate;
    
    
    /// //////////////////////////////////////////////////////////////
    /* We need the following variable to align the gun to camera sight*/
    // Pivot of the gun
    [SerializeField] private Transform gunPivot;
    // Gun location
    [SerializeField] private Transform enemyGun;
    // player transform
    private Transform mainPlayer;
    // reference to camera
    private Transform cam;
    /// //////////////////////////////////////////////////////////////
    
    // damage , assigned through enemy class
    public int damage;
    // fire rate
    private float fireRate;
   
    //caching varaibles .
    private void Start()
    {
        // we have to detach from its parent to disable the particles correctly .
        transform.SetParent(null);
        
        mainPlayer = GameObject.FindWithTag("Player").transform;
        cam = Camera.main.transform;
        
        // hard coded fire rate value, should be dynamically set depends on level difficulty .
        fireRate = 1;
        for (int i = 0; i < CannonParticles.Length; ++i)
        {
            CannonParticles[i].emissionRate = fireRate;
        }
    }

    // align the gun location with camera sight .
    private void SetLocation()
    {
        enemyGun.rotation = transform.rotation;
        var A = gunPivot.position;
        var B = cam.transform.position;
        var height = mainPlayer.position.y;
        var AB_Normalized = (A - B).normalized;
        var unitVector = (height - A.y) / AB_Normalized.y;
        transform.position = (AB_Normalized * unitVector) + A;
    }

    //called each frame to set the location correctly.
    private void Update()
    {
        SetLocation();
    }

    // Once its enabled, we Invoke repeating Gun Activation where the offensive logic happen .
    private void OnEnable()
    {
        InvokeRepeating("Activate", 1, 3);
        //  Invoke("Activate",3);
    }
    
    // called before locking on player to make sure that particles not always firing. 
    private void Deactivate()
    {
        // transform.SetParent(null);
        for (int i = 0; i < CannonParticles.Length; ++i)
        {
            CannonParticles[i].Stop();
        }
    }

    // after short period, gun will point to player and then particles will start playing .
    private void Activate()
    {
        Deactivate();
        gameObject.transform.DOLookAt(mainPlayer.position, lockRate).onComplete = delegate
        {
            for (int i = 0; i < CannonParticles.Length; ++i)
            {
                CannonParticles[i].Play();
            }
        };
    }

    // Called when the enemy target get disabled .
    public void Cull()
    {
        CancelInvoke();
        gameObject.transform.DOKill();
        for (int i = 0; i < CannonParticles.Length; ++i)
        {
            CannonParticles[i].Stop();
            var particleMain = CannonParticles[i].main;
            particleMain.stopAction = ParticleSystemStopAction.Callback;
        }
    }

    // called when particles collide .
    private void OnParticleCollision(GameObject go)
    {
        var player = go.GetComponent<MainplayerClass>();
        if (player != null)
            player.RecieveDamage(damage);
    }

    // once the particle system is stopped , disable the script .
    private void OnParticleSystemStopped()
    {
        enabled = false;
    }
}