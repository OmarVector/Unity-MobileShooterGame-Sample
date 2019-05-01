using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// Class that hold properties of MiniBosses
public class EliteEnemy : ENEMY
{
    // Canvas that has the health bar and text
    [SerializeField] private RectTransform healthCanvas;

    // Reference to health bar image to check how many hp left.
    [SerializeField] private Image healthBar;

    // Reference to Text to render out Hp in % .
    [SerializeField] private Text healthText;

    private Camera mainCamera;

    private int initHealth;

    // caching our reference
    void Start()
    {
        mainCamera = Camera.main;
        initHealth = Health; // we need to store the initial health to set the % of the health bar
    }

    private void Update()
    {
        // updating the healthbar canvas to keep looking to the camera
        healthCanvas.LookAt(mainCamera.transform,Vector3.forward);
    }

    public override void ReceiveDamage(int damage)
    {
        // updating health bar whenever mini boss get damage
        var amount = (float) Health / initHealth;
        healthBar.DOFillAmount(amount, 0.1f);
        //healthBar.fillAmount = (float) Health / initHealth;
        healthText.text = (healthBar.fillAmount * 100) + "%";

        base.ReceiveDamage(damage);
    }

    // called when mini boss get destroyed
    protected override void OnDeath()
    {
        ExplosionPoolManager.explosionPoolManager.PlayTerrainParticles(transform);
        base.OnDeath();
    }

    // called when mini boss colliding with main player
    private void OnCollisionEnter(Collision collision)
    {
        var x = collision.gameObject.GetComponent<MainplayerClass>();
        if(x!=null)
            x.RecieveDamage(Damage);
        
        OnDeath();
    }
}