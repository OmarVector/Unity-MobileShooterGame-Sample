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
        DynamicAirUnit, // For objects that moves along the path like tanks, Jets..etc
        DynamicTerrainUnit, // For moving object on terrain
        StaticUnit, // For objects that hold their positions, like Turrets.
        EliteUnit // For MiniBosses . //TODO
    }

    public EnemyType Type;


    // Start is called before the first frame update
    void Start()
    {
        tweenPath = GetComponent<DOTweenPath>();
        tweenPath.easeType = Ease.Linear;
        ENEMY enemy;


        switch (Type)
        {
            // If our unit is DynamicAir moving game object
            case EnemyType.DynamicAirUnit:
            {
                // Loading Enemies and add them to the list.
                for (int i = 0; i < enemySize; ++i)
                {
                    var unit =
                        Instantiate(enemyGameObject, tweenPath.tween.PathGetPoint(0),
                            Quaternion.identity) as GameObject;
                    unit.transform.SetParent(gameObject.transform);
                    enemy = unit.GetComponent<ENEMY>();
                    enemy.SetEnemyProperties(enemyHealth, enemyDamage, score, dropAmount, false, this);
                    TweenOnFinish.Add(enemy.OnStop);
                    Enemies.Add(unit);
                }

                break;
            }

            // If our unit is DynamicAir moving game object
            case EnemyType.DynamicTerrainUnit:
            {
                for (int i = 0; i < enemySize; ++i)
                {
                    var unit =
                        Instantiate(enemyGameObject, tweenPath.tween.PathGetPoint(0),
                            Quaternion.identity) as GameObject;
                    unit.transform.SetParent(gameObject.transform);
                    // By Design : Terrain unity collider's height is set dynamically , and thus we can attach the enemy class to the top of the object . Check TerrainUnitEnemy Class "SetColliderLocation()"
                    enemy = unit.GetComponentInChildren<ENEMY>();
                    enemy.SetEnemyProperties(enemyHealth, enemyDamage, score, dropAmount, false, this);
                    TweenOnFinish.Add(enemy.OnStop);
                    Enemies.Add(unit);
                }
            }
                break;

            // If our unit is Static game object like turret
            case EnemyType.StaticUnit:
            {
                // Usually only one unit .
                var unit =
                    Instantiate(enemyGameObject, tweenPath.tween.PathGetPoint(0), Quaternion.identity) as GameObject;
                unit.transform.SetParent(gameObject.transform);
                enemy = unit.GetComponentInChildren<ENEMY>();
                enemy.SetEnemyProperties(enemyHealth, enemyDamage, score, dropAmount, false, this);


                Destroy(tweenPath);
                Enemies.Add(unit);
            }
                break;
            
            case EnemyType.EliteUnit:
            {
                var unit = Instantiate(enemyGameObject, tweenPath.tween.PathGetPoint(0), Quaternion.Euler(0,180,0));
                unit.transform.SetParent(gameObject.transform);
                enemy = unit.GetComponent<ENEMY>();
                enemy.SetEnemyProperties(enemyHealth, enemyDamage, score, dropAmount, false, this);

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
            case EnemyType.DynamicAirUnit:
            {
                StartCoroutine(EnableDynamicAirUnit());
                break;
            }

            case EnemyType.DynamicTerrainUnit:
            {
                StartCoroutine(EnableDynamicTerrainUnit());
                break;
            }


            // If the enemy unit is static, Just Set it active.    
            case EnemyType.StaticUnit:
            {
                Enemies[0].SetActive(true);
                break;
            }
            // If its a MiniBoss, Just Set it active.
            case EnemyType.EliteUnit:
            {
                Enemies[0].SetActive(true);
                break;
            }
        }
    }

    // enable dynamic Enemies
    private IEnumerator EnableDynamicAirUnit()
    {
        // enable Rigid Bodies of enemies to follow the path, once it ended we call OnStop method
        for (int i = 0; i < enemySize; ++i)
        {
            Enemies[i].SetActive(true);
            var rd = Enemies[i].GetComponent<Rigidbody>();
            rd.DOPath(tweenPath.wps.ToArray(), SpeedInSeconds, PathType.CatmullRom).OnComplete(TweenOnFinish[i])
                .SetLookAt(0.01f);

            yield return new WaitForSeconds(spaceBetweenEachEnemy);
        }
    }

    private IEnumerator EnableDynamicTerrainUnit()
    {
        for (int i = 0; i < enemySize; ++i)
        {
            Enemies[i].SetActive(true);
            Enemies[i].transform.DOPath(tweenPath.wps.ToArray(), SpeedInSeconds, PathType.CatmullRom)
                .SetOptions(false, AxisConstraint.None, AxisConstraint.None).SetLookAt(0.01f)
                .OnComplete(TweenOnFinish[i]);

            yield return new WaitForSeconds(spaceBetweenEachEnemy);
        }
    }

    // Checking max score Bonus in case we destroyed all enemies in one single group
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