using UnityEngine;

public class LaserWeapon : WEAPON, ISuperWeapon
{
    // Default constant parameter for laser 
    private const int BASETIME = 10;

    // Laser Range
    private const int RANGE = 130;

    // LineRender reference to get its width.
    [SerializeField] private LineRenderer lineRender;

    // layer mask that laser will interact with
    [SerializeField] private LayerMask layerMask;

    // reference to SuperWeaponCanvasController to disable it once its actiavted.
    [SerializeField] private SuperWeaponCanvasController superWeaponCanvas;

    // active time length
    private float time;

    // array of hit targets .
    private RaycastHit[] hits;

    // calculating the active time length 
    private void Awake()
    {
        power = (ShipDataManager.shipDataManager.RocketLevel) + BASETIME;
        time = power / 10;
    }

    // once its enabled we do damage
    protected override void Update()
    {
        var p1 = transform.position; // starting point for CapsuleCastAll
        var p2 = p1 + new Vector3(0, 0, RANGE); // ending point for CapsuleCastAll

        // CapsuleCastAll
        hits = Physics.CapsuleCastAll(p1, p2, lineRender.endWidth, Vector3.forward, 1, layerMask);

        // Debug.DrawLine(p1, p2, Color.blue); // Just for debugging.

        // applying damage for each hit target.
        for (int i = 0; i < hits.Length; ++i)
        {
            var enemy = hits[i].collider.gameObject.GetComponent<ENEMY>();
            if (enemy)
                enemy.ReceiveDamage(power * 100);
        }
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
        ShipDataManager.shipDataManager.LaserAmount--;
        // saving data . 
        ShipDataManager.shipDataManager.SaveShipLevelsData();
        // calling disabled function on SuperWeaponCanvas.
        superWeaponCanvas.Deactivate();
    }

    // Deactivating Shield
    public void DeactivateSuperWeapon()
    {
        gameObject.SetActive(false);
    }
}