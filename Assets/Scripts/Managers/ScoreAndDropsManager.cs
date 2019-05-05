using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private int coinsPoolSize;

    private const int POWERUP_SIZE = 8; // its constant bec for each level only 8 powerup should be dropped.
    private const int LASER_SIZE = 3; // Limited along all level.
    private const int SHIELD_SIZE = 3; // Limited along all level
    private const int HEALTH_KIT_SIZE = 3; // Limited along all level

    // reference to text score.
    [SerializeField] private Text scoreText;
    [SerializeField] private Text endOfGameText;

    [SerializeField] private PlayerController playerController;


    /// <summary>
    /// Array for our pool, no need to use list here.
    /// </summary>
    private LinkedList<GameObject> HealthKitPool = new LinkedList<GameObject>();

    private LinkedList<GameObject> CoinsPool = new LinkedList<GameObject>();

    private LinkedList<GameObject> LaserPool = new LinkedList<GameObject>();
    private LinkedList<GameObject> ShieldPool = new LinkedList<GameObject>();
    private LinkedList<GameObject> PowerUpPool = new LinkedList<GameObject>();

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

        InitializePools();
    }


    private void InitializePools()
    {
        GameObject healthGrp = new GameObject();
        GameObject coinGrp = new GameObject();
        GameObject laserGrp = new GameObject();
        GameObject shieldGrp = new GameObject();
        GameObject powerUpGrp = new GameObject();

        healthGrp.name = "Health Grp";
        coinGrp.name = "Coins Grp";
        laserGrp.name = "Laser Grp";
        shieldGrp.name = "Shield Grp";
        powerUpGrp.name = "PowerUp Grp";

        healthGrp.transform.SetParent(transform);
        coinGrp.transform.SetParent(transform);
        laserGrp.transform.SetParent(transform);
        shieldGrp.transform.SetParent(transform);
        powerUpGrp.transform.SetParent(transform);


        for (int i = 0; i < HEALTH_KIT_SIZE; ++i)
        {
            var hk = Instantiate(healthKit, transform.position, Quaternion.identity);
            hk.transform.SetParent(healthGrp.transform);

            HealthKitPool.AddFirst(hk);
        }

        for (int i = 0; i < coinsPoolSize; ++i)
        {
            var coin = Instantiate(coins, transform.position, Quaternion.identity);
            coin.transform.SetParent(coinGrp.transform);

            CoinsPool.AddFirst(coin);
            // CoinsPool[i] = coin;
        }

        for (int i = 0; i < LASER_SIZE; ++i)
        {
            var las = Instantiate(laser, transform.position, Quaternion.identity);
            las.transform.SetParent(laserGrp.transform);

            LaserPool.AddFirst(las);
        }

        for (int i = 0; i < SHIELD_SIZE; ++i)
        {
            var shi = Instantiate(shield, transform.position, Quaternion.identity);
            shi.transform.SetParent(shieldGrp.transform);

            ShieldPool.AddFirst(shi);
        }

        for (int i = 0; i < POWERUP_SIZE; ++i)
        {
            var power = Instantiate(powerUp, transform.position, Quaternion.identity);
            power.transform.SetParent(powerUpGrp.transform);

            PowerUpPool.AddFirst(power);
        }
    }

    /// <summary>
    /// using linked list , using linked list of length 3 or even 8 is overkill, but just to use one standard for now.
    /// </summary>
    public void GetHealthKit(Vector3 pos)
    {
        if (healthKitIndex < HEALTH_KIT_SIZE)
        {
            var hk = HealthKitPool.First.Value;

            hk.transform.position = pos;
            hk.SetActive(true);

            HealthKitPool.RemoveFirst();
            healthKitIndex++;
        }
        else
        {
            GetCoin(pos);
        }
    }

    public void GetCoin(Vector3 pos)
    {
        // var coin = CoinsPool[coinsIndex];
        var coin = CoinsPool.First.Value;
        coin.transform.position = pos;
        coin.SetActive(true);

        CoinsPool.RemoveFirst();
    }

    public void GetLaser(Vector3 pos)
    {
        if (laserIndex < LASER_SIZE)
        {
            var las = LaserPool.First.Value;

            las.transform.position = pos;
            las.SetActive(true);
            
            LaserPool.RemoveFirst();
            laserIndex++;
        }
        else
        {
            GetCoin(pos);
        }
    }

    public void GetShield(Vector3 pos)
    {
        if (shieldIndex < SHIELD_SIZE)
        {
            var shi = ShieldPool.First.Value;

            shi.transform.position = pos;
            shi.SetActive(true);
            
            ShieldPool.RemoveFirst();
            shieldIndex++;
        }
    }

    public void GetPowerUP(Vector3 pos)
    {
        if (PowerUpIndex < POWERUP_SIZE)
        {
            var power = PowerUpPool.First.Value;

            power.transform.position = pos;
            power.SetActive(true);
            
            PowerUpPool.RemoveFirst();
            PowerUpIndex++;
        }
        else
        {
            GetCoin(pos);
        }
    }

    // Called when drops not collected depends on type, or only return coins to pool upon collecting.
    public void ReturnToPool(GameObject go, int DropType)
    {
        go.SetActive(false);
        
        switch (DropType)
        {
            case 1:
                HealthKitPool.AddFirst(go);
                healthKitIndex--;
                break;
            case 2:
                CoinsPool.AddFirst(go);
                break;
            case 3:
                LaserPool.AddFirst(go);
                laserIndex--;
                break;
            case 4:
                ShieldPool.AddFirst(go);
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

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        MainCannon.gameObject.SetActive(false);
        WingCannonL.gameObject.SetActive(false);
        WingCannonR.gameObject.SetActive(false);

        GameObject.FindWithTag("PlayerPath").GetComponent<DOTweenAnimation>().DOKill();

        if (condition == "WINNER")
        {
            playerController.gameObject.transform.DOMove(
                new Vector3(playerController.transform.position.x, playerController.transform.position.y,
                    playerController.transform.position.z + 200), 3).SetEase(Ease.InQuad);
           
        }
        playerController.enabled = false;

        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(3f* Time.timeScale);
        SceneManager.LoadSceneAsync(0);
    }
}