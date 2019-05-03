using UnityEngine;

// Homing Missile Controller.
public class HommingMissleController : MonoBehaviour
{
    // Rigidbody reference of the rocket
    [SerializeField] private Rigidbody rocketRigidBody;
    // Turn rate for facing Target
    [SerializeField] private float turnRate;
    // Velocity of the Missiles
    [SerializeField] private float velocity;
    // Enemy Target
    private Transform rocketTarget;
   


    private void Start()
    {
        // The initial target will be always the radar center itself.
        rocketTarget = RocketRadar.rocketRadar.transform;

        // Destroy the Missile after 5 sec
        Invoke("Cull", 5f);
        // Repeat checking for active target to Lock
        InvokeRepeating("CheckRadar",0.1f,0.05f);
    }

    private void FixedUpdate()
    {
        // If the target got killed before being hit my the Missile, we check the Radar once again.
        if (!rocketTarget.gameObject.activeInHierarchy)
        {
            CheckRadar();
            return;
        }

        // increasing velocity, fake acceleration.     
        velocity ++;
        
        // Adding velocity to the rocket
        rocketRigidBody.velocity = transform.forward * velocity ;

        // getting the direction where the Missile should lock toward the target.
        var rocketTargetRotation = Quaternion.LookRotation(rocketTarget.position - transform.position );
        // Rotating the missiles to face the target
        rocketRigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rocketTargetRotation, turnRate));
      
        
    }

    // Checking the radar for available targets
    private void CheckRadar()
    {
        turnRate+=0.1f;
        if (RocketRadar.rocketRadar.EnemyTransform.Count > 0)
        {
            rocketTarget = RocketRadar.rocketRadar.EnemyTransform[0];
        }
     
    }

    // Destroying the rocket
    private void Cull()
    {
        Destroy(gameObject);
    }
    
    
}