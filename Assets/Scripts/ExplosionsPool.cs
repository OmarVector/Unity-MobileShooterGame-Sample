using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionsPool : MonoBehaviour
{
    [SerializeField] private int PoolSizeAir;
    [SerializeField] private int PoolSizeTerrain;

    [SerializeField] private ExplosionsContainer EXPOContainer;
    [SerializeField] private ExplosionsContainer EXPOContainerTerrain;

    private List<GameObject> AirUnitParticles = new List<GameObject>();
    private List<GameObject> TerrainUnitParticles = new List<GameObject>();

    public static ExplosionsPool explosionsPool = null;

    private void Awake()
    {
        if (explosionsPool == null)
        {
            explosionsPool = this;
        }
        else if (explosionsPool != this)
        {
            Destroy(gameObject);
        }
    
        LoadParticles();
    }

    private void LoadParticles()
    {
        for (int i = 0; i < PoolSizeAir; ++i)
        {
            int rand = Random.Range(0, EXPOContainer.Particle_List.Count - 1);
            var partGO = Instantiate(EXPOContainer.Particle_List[rand], transform.position, Quaternion.identity);
            partGO.SetActive(false);
            partGO.transform.SetParent(transform);
            AirUnitParticles.Add(partGO);
        }

        for (int i = 0; i < PoolSizeTerrain; ++i)
        {
            int rand = Random.Range(0, EXPOContainerTerrain.Particle_List.Count - 1);
            var partGO = Instantiate(EXPOContainerTerrain.Particle_List[rand], transform.position, Quaternion.identity);
            partGO.SetActive(false);
            partGO.transform.SetParent(transform);
            TerrainUnitParticles.Add(partGO);
        }
    }


    public void PlayAirParticle(Transform trans)
    {
        for (int i = 0; i < PoolSizeAir; ++i)
        {
            if (!AirUnitParticles[i].activeInHierarchy)
            {
                AirUnitParticles[i].transform.position = trans.position;
                AirUnitParticles[i].SetActive(true);
                AirUnitParticles[i].GetComponent<ParticleSystem>().Play();
                return;
            }
        }
    }

    public void PlayTerrainParticles(Transform trans)
    {
       
        bool isActive = false;
        while (!isActive)
        {
            int i = Random.Range(0, PoolSizeTerrain);
            if (!TerrainUnitParticles[i].activeInHierarchy)
            {
                TerrainUnitParticles[i].transform.position = trans.position;
                TerrainUnitParticles[i].SetActive(true);
                TerrainUnitParticles[i].GetComponent<ParticleSystem>().Play();
                isActive = true;
                return;
            }
        }
    }

    public void ReturnEnemyToPool(GameObject go)
    {
        go.SetActive(false);
        go.transform.position = transform.position;
    }
}