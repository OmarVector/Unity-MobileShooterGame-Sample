using UnityEngine;

public class Drop : MonoBehaviour
{
    [Tooltip("1 == HealthKit , 2 == Coins , 3 == Laser , 4 == Shield , 5 == PowerUp")]
    [SerializeField] private int dropType;

    private float time;
    private const int DROP_TIMEOUT = 10;

    private void OnEnable()
    {
        Invoke("ReturnToPool",DROP_TIMEOUT);
    }

    private void ReturnToPool()
    {
        ScoreAndDropsManager.scoreAndDropsManager.ReturnToPool(gameObject, dropType);
        Debug.Log("<color=yellow>Drops back to pool Invoked</color>");
        time = 0;
    }

    private void OnCollected()
    {
        CancelInvoke();
        time = 0;
        switch (dropType)
        {
            case 1 :
                GameObject.FindWithTag("Player").GetComponent<MainplayerClass>().health += 100;
                break;
            case 2 :
                ScoreAndDropsManager.scoreAndDropsManager.tempCoinAmount++;
                ScoreAndDropsManager.scoreAndDropsManager.ReturnToPool(gameObject, dropType);
                break;
            case 3 :
                ScoreAndDropsManager.scoreAndDropsManager.tempLaserAmount++;
                break;
            case 4 : ScoreAndDropsManager.scoreAndDropsManager.tempShieldAmount++;
                break;
            case 5 :
                ScoreAndDropsManager.scoreAndDropsManager.MainCannon.PowerUp();
                ScoreAndDropsManager.scoreAndDropsManager.WingCannonL.PowerUp();
                ScoreAndDropsManager.scoreAndDropsManager.WingCannonR.PowerUp();
                break;
        }
        
        gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        time += Time.deltaTime * Time.timeScale;

        transform.position =
            Vector3.Lerp(transform.position, other.transform.position, time);
        if (time >= 1)
        {
            OnCollected();
        }
    }
}