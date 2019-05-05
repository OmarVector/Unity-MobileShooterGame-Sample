using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ScoreAndDropsManager : MonoBehaviour
{
    /// <summary>
    /// Those are drops game object needed to instantiate .
    /// </summary>
    [SerializeField] private GameObject healthKit;

    [SerializeField] private GameObject coins;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject powerUp;

    /// <summary>
    /// Those are pool size for each drop
    /// </summary>
    [SerializeField] private int healthKitPoolSize;

    [SerializeField] private int coinsPoolSize;
    [SerializeField] private int laserPoolSize;
    [SerializeField] private int shieldPoolSize;
    private const int POWERUPSIZE = 8; // its constant bec for each level only 8 powerup should be dropped.

    // reference to text score.
    [SerializeField] private Text scoreText;
    [SerializeField] private Text endOfGameText;


    /// <summary>
    /// Array for our pool, no need to use list here.
    /// </summary>
    private GameObject[] HealthKitPool;

    private GameObject[] CoinsPool;
    private GameObject[] LaserPool;
    private GameObject[] ShieldPool;
    private GameObject[] PowerUpPool;

    /// <summary>
    /// those are index counters to go through pool , its better than checking each time during for lopp which item is not active
    /// this saved a lot performance when there is too much active item in the pool
    /// </summary>
    private int healthKitIndex;

    private int coinsIndex;
    private int laserIndex;
    private int shieldIndex;
    private int PowerUpIndex;

    /// <summary>
    /// per level stored coins, shield and laser
    /// only coins will be saved to ship data once the level is over, to avoid players farming lasers and shields.
    /// </summary>
    public int tempCoinAmount;

    public int tempShieldAmount;
    public int tempLaserAmount;

    // Score, public to be updated within different enemy scripts.
    [HideInInspector] public int Score;

    /// <summary>
    /// reference to weapons who will be powered up once a power up is collected.
    /// </summary>
    public WEAPON MainCannon;

    public WEAPON WingCannonL;
    public WEAPON WingCannonR;

    // to access it globally around the level.
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

        // Initializing the pools
        HealthKitPool = new GameObject[healthKitPoolSize];
        CoinsPool = new GameObject[coinsPoolSize];
        LaserPool = new GameObject[laserPoolSize];
        ShieldPool = new GameObject[shieldPoolSize];
        PowerUpPool = new GameObject[POWERUPSIZE];

        InitializePools();
    }


    private void InitializePools()
    {
        for (int i = 0; i < healthKitPoolSize; ++i)
        {
            var hk = Instantiate(healthKit, transform.position, Quaternion.identity);
            hk.transform.SetParent(transform);

            HealthKitPool[i] = hk;
        }

        for (int i = 0; i < coinsPoolSize; ++i)
        {
            var coin = Instantiate(coins, transform.position, Quaternion.identity);
            coin.transform.SetParent(transform);

            CoinsPool[i] = coin;
        }

        for (int i = 0; i < laserPoolSize; ++i)
        {
            var las = Instantiate(laser, transform.position, Quaternion.identity);
            las.transform.SetParent(transform);

            LaserPool[i] = las;
        }

        for (int i = 0; i < shieldPoolSize; ++i)
        {
            var shi = Instantiate(shield, transform.position, Quaternion.identity);
            shi.transform.SetParent(transform);

            ShieldPool[i] = shi;
        }

        for (int i = 0; i < POWERUPSIZE; ++i)
        {
            var power = Instantiate(powerUp, transform.position, Quaternion.identity);
            power.transform.SetParent(transform);

            PowerUpPool[i] = power;
        }
    }

    /// <summary>
    /// How those pools works?
    /// by default each pool index is zero
    /// 1. pick 0 index item from the pool.
    /// 2. increment the pool index by 1
    /// 3. once its collected, return it back to the pool or after short time
    /// 4. decrement the pool index by 1
    /// by this way we will always be in range .
    /// time complexity for this alogrithm is O(1), compared to for loop check which is O(N)
    /// </summary>
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

    public void GetPowerUP(Vector3 pos)
    {
        if (PowerUpIndex < POWERUPSIZE)
        {
            var power = PowerUpPool[PowerUpIndex];

            power.transform.position = pos;
            power.SetActive(true);
            PowerUpIndex++;
        }
        else
        {
            GetCoin(pos);
        }
    }

    // Returning game object to pool.
    public void ReturnToPool(GameObject go, int DropType)
    {
        go.SetActive(false);
        switch (DropType)
        {
            case 1:
                healthKitIndex--;
                break;
            case 2:
                coinsIndex--;
                break;
            case 3:
                laserIndex--;
                break;
            case 4:
                shieldIndex--;
                break;
        }
    }

    // updating score.
    public void UpdateScore()
    {
        scoreText.text = Score.ToString();
    }

    // called once the level is finished or player die. loading main menu
    public void LevelFinished(string condition)
    {
        endOfGameText.text = condition;
        endOfGameText.enabled = true;
        ShipDataManager.shipDataManager.CoinsAmount += tempCoinAmount;
        ShipDataManager.shipDataManager.SaveShipLevelsData();

        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(0);
    }
}