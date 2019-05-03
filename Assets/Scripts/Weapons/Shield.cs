using UnityEngine;

public class Shield : WEAPON, ISuperWeapon
{
    // Default time in sec for shield 
    private static int BASETIME = 20;

    private float time;
    // Start is called before the first frame update
    private void Awake()
    {
        power = (ShipDataManager.shipDataManager.RocketLevel) + BASETIME;
        time = power / 10;
    }

    private void OnCollisionEnter(Collision other)
    {
        var enemy = other.gameObject.GetComponent<ENEMY>();
        if(enemy!=null)
            enemy.ReceiveDamage(power*100);
    }

    public void EnableSuperWeapon()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        Invoke("ShieldShutdown",time);
    }

    private void ShieldShutdown()
    {
        gameObject.SetActive(false);
    }
}
