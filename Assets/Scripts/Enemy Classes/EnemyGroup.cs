using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// This class to initialize a full group formation of enemy and initializing their properties
public class EnemyGroup : MonoBehaviour
{
    // Enemy gameobject that will be instantiated in the group
    [SerializeField] private GameObject enemyGameObject;

    // Size of enemy group
    [SerializeField] private int enemySize;

    // Health of the enemies
    [SerializeField] private int enemyHealth;

    // Damage of the enemies
    [SerializeField] private int enemyDamage;

    // Score amount
    [SerializeField] private int score;

    // amount of drops they will give players when they get destroyed 
    [SerializeField] private int dropAmount;

    // Since we are using DOTWEEN Path along the level on Linear path
    // Everything will be enabled within certain time of the level 

    // Speed of the enemies along the path in seconds.
    [Tooltip("Time to move along the path in seconds")] [SerializeField]
    private float SpeedInSeconds;

    // Time for enabling enemies
    [Tooltip("Time needed to set the group Active in game")] [SerializeField]
    private float timerToEnable;

    /* while we looping through each enemy in the group to set it active and go alone the path
       we need to set a simple separate distance between each other*/
    [Tooltip("Distance that separate between each enemy in the group in second")] [SerializeField]
    private float spaceBetweenEachEnemy;

    // A Check to see if all enemies in the group got destroyed or not to provide extra score.
    private int EnemyScoreCounter;

    // Reference to our custom Path
    private DOTweenPath tweenPath;

    // List for our enemies
    private List<GameObject> Enemies = new List<GameObject>();

    // Callback for our finished tween to return our enemies to their pool.
    private List<TweenCallback> TweenOnFinish = new List<TweenCallback>();

    // enum we going to use to select desired enemy type while building our level.
    public enum EnemyType
    {
        DynamicUnit, // For objects that moves along the path like tanks, Jets..etc
        StaticUnit, // For objects that hold their positions, like Turrets.
        MiniBoss // For MiniBosses . //TODO
    }

    public EnemyType Type;


    // Start is called before the first frame update
    void Start()
    {
        tweenPath = GetComponent<DOTweenPath>();
        tweenPath.easeType = Ease.Linear;
        Enemy enemy;


        switch (Type)
        {
            // If our unit is Dynamic moving game object
            case EnemyType.DynamicUnit:
            {
                // Loading Enemies and add them to the list.
                for (int i = 0; i < enemySize; ++i)
                {
                    var unit = Instantiate(enemyGameObject, tweenPath.tween.PathGetPoint(0), Quaternion.identity);
                    unit.transform.SetParent(gameObject.transform);
                    enemy = unit.GetComponent<Enemy>();
                    enemy.SetEnemyProperties(enemyHealth, enemyDamage, score, dropAmount, false);

                    Enemies.Add(unit);
                }

                break;
            }

            // If our unit is Static game object like turret
            case EnemyType.StaticUnit:
            {
                var unit = Instantiate(enemyGameObject, transform.position, Quaternion.identity);
                enemyGameObject.transform.SetParent(gameObject.transform);
                enemy = enemyGameObject.GetComponentInChildren<Enemy>();

                enemy.SetEnemyProperties(enemyHealth, enemyDamage, score, dropAmount, false);

                Destroy(tweenPath);
                Enemies.Add(unit);
            }
                break;
        }

        // Set enemy active within given time
        Invoke("EnableEnemy", timerToEnable);
    }

    // Enable Enemy based on their type 
    private void EnableEnemy()
    {
        switch (Type)
        {
            // If the enemy is dynamic , StartCoroutine EnableDynamicEnemies.
            case EnemyType.DynamicUnit:
            {
                StartCoroutine(EnableDynamicEnemies());
                break;
            }
            // If the enemy unit is static, Just Set it active.    
            case EnemyType.StaticUnit:
            {
                Enemies[0].SetActive(true);
                break;
            }
            // If its a MiniBoss, Just Set it active.
            case EnemyType.MiniBoss:
            {
                Enemies[0].SetActive(true);
                break;
            }
        }
    }

    private IEnumerator EnableDynamicEnemies()
    {
        for (int i = 0; i < enemySize; ++i)
        {
            Enemies[i].SetActive(true);
            var rd = Enemies[i].GetComponent<Rigidbody>();
            rd.DOPath(tweenPath.wps.ToArray(), SpeedInSeconds, PathType.CatmullRom);

            yield return new WaitForSeconds(spaceBetweenEachEnemy);
        }
    }

    public void ScoreBonusCheck()
    {
        EnemyScoreCounter++;
        if (EnemyScoreCounter == enemySize)
        {
            Debug.Log("MAX SCORE"); //TODO Adding mini celebration
            Destroy(gameObject);
        }
    }
}