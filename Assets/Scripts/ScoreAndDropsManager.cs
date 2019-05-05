using UnityEngine;
using UnityEngine.UI;


public class ScoreAndDropsManager : MonoBehaviour
{

    [SerializeField] private GameObject healthKit;
    [SerializeField] private GameObject coins;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject shield;

    [SerializeField] private int healthKitPoolSize;
    [SerializeField] private int coinsPoolSize;
    [SerializeField] private int laserPoolSize;
    [SerializeField] private int shieldPoolSize;
    
    [SerializeField] private Text scoreText;

    private GameObject[] HealthKitPool;
    private GameObject[] CoinsPool;
    private GameObject[] LaserPool;
    private GameObject[] ShieldPool;

    private int healthKitIndex;
    private int coinsIndex;
    private int laserIndex;
    private int shieldIndex;
   
   [HideInInspector] public int Score;
   
    
    public static ScoreAndDropsManager scoreAndDropsManager;
    
    private void Awake()
    {
        if (scoreAndDropsManager == null)
        {
            scoreAndDropsManager = this;
        }
        else if (scoreAndDropsManager != this)
        {
            Destroy(gameObject);
        }
        
        HealthKitPool = new GameObject[healthKitPoolSize];
        CoinsPool = new GameObject[coinsPoolSize];
        LaserPool = new GameObject[laserPoolSize];
        ShieldPool = new GameObject[shieldPoolSize];
        
        InitializePools();
    }
    
    

    private void InitializePools()
    {
        for (int i = 0; i < healthKitPoolSize; ++i)
        {
            var hk = Instantiate(healthKit, transform.position, Quaternion.identity);
            hk.transform.SetParent(transform);
            hk.SetActive(false);
            HealthKitPool[i] = hk;
        }

        for (int i = 0; i < coinsPoolSize; ++i)
        {
            var coin = Instantiate(coins, transform.position, Quaternion.identity);
            coin.transform.SetParent(transform);
            coin.SetActive(false);
            CoinsPool[i] = coin;
        }

        for (int i = 0; i < laserPoolSize; ++i)
        {
            var las = Instantiate(laser, transform.position, Quaternion.identity);
            las.transform.SetParent(transform);
            las.SetActive(false);
            LaserPool[i] = las;
        }

        for (int i = 0; i < shieldPoolSize; ++i)
        {
            var shi = Instantiate(shield, transform.position, Quaternion.identity);
            shi.transform.SetParent(transform);
            shi.SetActive(false);
            ShieldPool[i] = shi;
        }
    }

    public void GetHealthKit(Vector3 pos)
    {
        if (healthKitIndex < healthKitPoolSize)
        {
            var hk = HealthKitPool[healthKitIndex];

            hk.transform.position = pos;
            hk.SetActive(true);
            healthKitIndex++;
        }
    }

    public void GetCoin(Vector3 pos)
    {
        if (coinsIndex < coinsPoolSize)
        {
            var coin = CoinsPool[coinsIndex];

            coin.transform.position = pos;
            coin.SetActive(true);
            coinsIndex++;
        }
    }

    public void GetLaser(Vector3 pos)
    {
        if (laserIndex < laserPoolSize)
        {
            var las = LaserPool[laserIndex];

            las.transform.position = pos;
            las.SetActive(true);
            laserIndex++;
        }
    }

    public void GetShield(Vector3 pos)
    {
        if (shieldIndex < shieldPoolSize)
        {
            var shi = ShieldPool[shieldIndex];

            shi.transform.position = pos;
            shi.SetActive(true);
            shieldIndex++;
        }
    }

    public void ReturnToPool(GameObject go, int DropType)
    {
        go.SetActive(false);
        switch (DropType)
        {
            case 1 :
                healthKitIndex--;
                break;
            case 2 :
                coinsIndex--;
                break;
            case 3 :
                laserIndex--;
                break;
            case 4:
                shieldIndex--;
                break;
        }
    }

    public void UpdateScore()
    {
        scoreText.text = Score.ToString();
    }
}























