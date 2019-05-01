using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
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

    void SetColliderLocation()
    {
        A = unitBase.position;
        B = camTransform.position;
        height = mainPlayerTransform.position.y;
        AB_Normalized = (A - B).normalized;
        unitVector = (height - A.y) / AB_Normalized.y;
        transform.position = (AB_Normalized * unitVector) + A;
    }

    protected override void OnDeath()
    {
        ExplosionPoolManager.explosionPoolManager.PlayTerrainParticles(unitBase);
        destroyedObject.transform.SetParent(null);
        destroyedObject.transform.rotation = transform.rotation;
        destroyedObject.SetActive(true);
        unitBase.gameObject.SetActive(false);
        base.OnDeath();
    }
    
    public override void OnStop()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        
    }
}
