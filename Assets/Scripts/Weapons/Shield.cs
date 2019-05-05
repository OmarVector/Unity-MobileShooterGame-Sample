using UnityEngine;

// Shield class 
public class Shield : WEAPON, ISuperWeapon
{
    // Default constant parameter for shield 
    private const int BASEPOWER = 10;

    // active time length
    private float time;

    [SerializeField] private JetStatusCanvasController jetStatusCanvas;

    // calculating active time length for the shield
    private void Awake()
    {
        power = (ShipDataManager.shipDataManager.RocketLevel) + BASEPOWER;
        time = power / 10;
    }

    // once it collides with enemies it crash them
    private void OnCollisionEnter(Collision other)
    {
        var enemy = other.gameObject.GetComponent<ENEMY>();
        if (enemy != null)
            enemy.OnDeath();
    }

    // interface method to be called when we want to enable the shield.
    public void ActivateSuperWeapon()
    {
        gameObject.SetActive(true);
    }

    // On enable
    private void OnEnable()
    {
        // setting when the shield will be deactivated 
        Invoke("DeactivateSuperWeapon", time);
        // decreasing amount of shield count by 1
        ShipDataManager.shipDataManager.ShieldAmount--;
        // saving data . 
        ShipDataManager.shipDataManager.SaveShipLevelsData();
        // calling disabled function on SuperWeaponCanvas.
        jetStatusCanvas.Deactivate();
    }

    // Deactivating Shield
    public void DeactivateSuperWeapon()
    {
        gameObject.SetActive(false);
    }
}