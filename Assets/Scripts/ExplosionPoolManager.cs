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

    private List<GameObject> AirUnitParticles = new List<GameObject>();
    private List<GameObject> TerrainUnitParticles = new List<GameObject>();

    private int airIndex;
    private int terrainIndex;

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
            AirUnitParticles.Add(partGO);
        }

        // For Terrain Unit
        for (int i = 0; i < PoolSizeTerrain; ++i)
        {
            int rand = Random.Range(0, TerrainUnitContainer.Particle_List.Count - 1);
            var partGO = Instantiate(TerrainUnitContainer.Particle_List[rand], transform.position, Quaternion.identity);
            partGO.SetActive(false);
            partGO.transform.SetParent(transform);
            TerrainUnitParticles.Add(partGO);
        }
    }


    // Called when Air unit get destroyed
    public void PlayAirParticle(Transform trans)
    {
        if (airIndex < PoolSizeAir)
        {
            AirUnitParticles[airIndex].transform.position = trans.position;
            AirUnitParticles[airIndex].SetActive(true);
            AirUnitParticles[airIndex].GetComponent<ParticleSystem>().Play();
            airIndex++;
        }
    }
   

    // Called when particles Terrain get destroyed
    public void PlayTerrainParticles(Transform trans)
    {
        // Pick a random particle from pool to play when the air unit get destroyed 
        if (terrainIndex < PoolSizeTerrain)
        {
            var pos = trans.position;
            pos.y += 15;
            TerrainUnitParticles[terrainIndex].transform.position = pos;
            TerrainUnitParticles[terrainIndex].SetActive(true);
            TerrainUnitParticles[terrainIndex].GetComponent<ParticleSystem>().Play();
            terrainIndex++;
        }
    }

    // Returning particles back to the pool once they finish playing .
    public void ReturnEnemyToPool(GameObject go, bool isAir)
    {
        go.SetActive(false);
        if (isAir)
            airIndex--;
        else
            terrainIndex--;
    }
}