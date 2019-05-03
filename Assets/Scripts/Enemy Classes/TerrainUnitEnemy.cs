using UnityEngine;

public class TerrainUnitEnemy : ENEMY
{
    // Transform reference for Unit parent object since we going to use it to set collider location
    [SerializeField] private Transform unitBase;
    // A damaged version of the Enemy unit to be rendered once the object get destroyed.
    [SerializeField] private GameObject destroyedObject;
    // Reference to rigid body of the unit
    [SerializeField] private Rigidbody rigidBody;
    // Reference to camera transform, will be used beside to unitBase transform to set the collider
    private Transform camTransform;
    // Reference to Player Transform
    private Transform mainPlayerTransform;
    
    /* Those are variables will be used to get the correct unit vector between
     Camera and The unit on the same height of the player.
     
     I set them globally here to avoid GC since they had to run on every Physics frame instead
     of using var type */
    
    // Position of the unit base
    private Vector3 A;
    // Position of teh camera
    private Vector3 B;
    // A-B Normalized
    private Vector3 AB_Normalized;
    // player current height
    private float height;
    // calculated unit vector
    private float unitVector;
    
    

    // caching variables once they are instantiated 
    private void Awake()
    {
        mainPlayerTransform = GameObject.FindWithTag("Player").transform;
        camTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        SetColliderLocation();
    }

    // since our camera is prospective, setting colliders for terrain units must be dynamic to follow the line of sight of the camera
    // to do this, we must do the following :
   
    /*
         1. Calculating the line vector between Terrain Unit (A) and Camera (B) then normalizing the value
         2. Getting the height of the player to maintain the height of the collider as well.
         3. Getting the unit vector where it's Y= Jet Height.
         4. setting position of the collider.     
     */
    void SetColliderLocation()
    {
        A = unitBase.position;
        B = camTransform.position;
        height = mainPlayerTransform.position.y;
        AB_Normalized = (A - B).normalized;
        unitVector = (height - A.y) / AB_Normalized.y;
        transform.position = (AB_Normalized * unitVector) + A;
    }

    // By Design, terrain unit has parents unlike air unit + a destroyed version , so we control that here.
    protected override void OnDeath()
    {
        // play particles explosion upon destorying
        ExplosionPoolManager.explosionPoolManager.PlayTerrainParticles(unitBase);
        // Detach the destroyedObject version from its parent and then we activate it. 
        destroyedObject.transform.SetParent(null);
        destroyedObject.transform.rotation = transform.rotation;
        destroyedObject.SetActive(true);
        // then we deactivate the unitBase
        //TODO manging any fire arms that fire on mainplayer .
        unitBase.gameObject.SetActive(false);
        base.OnDeath();
    }
    
    // once the game object end its path but didnt get destroyed, it deactivate .
    public override void OnStop()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        
    }
}
