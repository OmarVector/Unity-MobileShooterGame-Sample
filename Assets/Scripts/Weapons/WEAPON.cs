using UnityEngine;

// Weapon parent class that will be used to make different types of weapons behavior .
public class WEAPON : MonoBehaviour
{
    // class that hold all common weapon properties

    // Weapon Damage
    protected int damage;
    // Weapon FireRate
    protected float fireRate;
    // Weapon particle Size
    protected float weaponSize;
    // Speed values that will be added to firing rate when power up is collected
    protected float powerIncrease;


    // Declaring virtual start functions to be override depends on weapon type classes we going to add
    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    
    // This is base.OnParticleCollision that hold most common action when particle collides with enemies.
    protected virtual void OnParticleCollision(GameObject go)
    {
        var enemy = go.GetComponent<ENEMY>();
        if(enemy!=null)
            enemy.ReceiveDamage(damage);
    }

    // its virtual bec some weapons will not depends on particles emission rates like Rockets.
    public virtual void PowerUp()
    {
    }
}