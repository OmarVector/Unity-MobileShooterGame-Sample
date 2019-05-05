using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPoolManager : MonoBehaviour
{
    // Air Explosion Pool
    [SerializeField] private int PoolSizeAir;

    // Terrain Explosion Pool
    [SerializeField] private int PoolSizeTerrain;

    // Scriptable Object reference for air units explosion
    [SerializeField] private ExplosionsContainer AirUnitContainer;

    // Scriptable Object reference for terrain units explosion
    [SerializeField] private ExplosionsContainer TerrainUnitContainer;

    // private List<GameObject> AirUnitParticles = new List<GameObject>();
    private LinkedList<GameObject> TerrainUnitParticles = new LinkedList<GameObject>();

    private LinkedList<GameObject> AirUnitParticles = new LinkedList<GameObject>();

  //  private int airIndex;
  //  private int terrainIndex;

    public static ExplosionPoolManager explosionPoolManager = null;

    private void Awake()
    {
        // initializing SingleTone for this manager.
        if (explosionPoolManager == null)
        {
            explosionPoolManager = this;
        }
        else if (explosionPoolManager != this)
        {
            Destroy(gameObject);
        }

        // Filling up the pool
        LoadParticles();
    }

    private void LoadParticles()
    {
        // For Air Unit
        for (int i = 0; i < PoolSizeAir; ++i)
        {
            // Picking random GO from list to add to the pool
            int rand = Random.Range(0, AirUnitContainer.Particle_List.Count - 1);

            var partGO = Instantiate(AirUnitContainer.Particle_List[rand], transform.position, Quaternion.identity);
            partGO.SetActive(false);
            partGO.transform.SetParent(transform);

            AirUnitParticles.AddFirst(partGO);
        }

        // For Terrain Unit
        for (int i = 0; i < PoolSizeTerrain; ++i)
        {
            int rand = Random.Range(0, TerrainUnitContainer.Particle_List.Count - 1);
            var partGO = Instantiate(TerrainUnitContainer.Particle_List[rand], transform.position, Quaternion.identity);
            partGO.SetActive(false);
            partGO.transform.SetParent(transform);

            TerrainUnitParticles.AddFirst(partGO);
        }
    }


    // Called when Air unit get destroyed
    public void PlayAirParticle(Transform trans)
    {
        var airExplosion = AirUnitParticles.First.Value;
        AirUnitParticles.RemoveFirst();

        airExplosion.transform.position = trans.position;
        airExplosion.SetActive(true);
        airExplosion.GetComponent<ParticleSystem>().Play();
    }


    // Called when particles Terrain get destroyed
    public void PlayTerrainParticles(Transform trans)
    {
        var terrainExplosion = TerrainUnitParticles.First.Value;
        TerrainUnitParticles.RemoveFirst();

        var pos = trans.position;
        pos.y += 15;

        terrainExplosion.transform.position = pos;
        terrainExplosion.SetActive(true);
        terrainExplosion.GetComponent<ParticleSystem>().Play();
    }

    // Returning particles back to the pool once they finish playing .
    public void ReturnEnemyToPool(GameObject go, bool isAir)
    {
        go.SetActive(false);
        if (isAir)
            AirUnitParticles.AddFirst(go);
        else
            TerrainUnitParticles.AddFirst(go);
    }
}