using System.Collections.Generic;
using UnityEngine;

// Radar System
public class RocketRadar : MonoBehaviour
{
    // Reference to HommingMissile GameObject that we going to use to Instantiate
    [SerializeField] private GameObject HommingMissile;
    // Reference to left rocket location
    [SerializeField] private Transform LRocket;
    // Reference to right rocket location
    [SerializeField] private Transform RRocket;
    // publis static of this class to use is globally in the level, doesnt need to be a single tone.
    public static RocketRadar rocketRadar;
   
    // List of available targets. its updated once enemies enter the Radar Trigger.
    public List<Transform>EnemyTransform = new List<Transform>();

    

    // Start is called before the first frame update

    private void Awake()
    {
        if (rocketRadar == null)
            rocketRadar = this;
        else if (rocketRadar != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        // the fire rate here is hard coded for now.
        InvokeRepeating("RocketLaunch",0,2f);
    }



    void RocketLaunch()
    {
        Instantiate(HommingMissile, LRocket.position, Quaternion.Euler(0,90,0));
        Instantiate(HommingMissile, RRocket.position, Quaternion.Euler(0,-90,0));
    }
   
}