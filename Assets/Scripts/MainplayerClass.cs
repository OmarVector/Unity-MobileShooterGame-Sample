using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainplayerClass : MonoBehaviour
{
    [SerializeField] private int health;

   public void RecieveDamage(int damage)
    {
        health = health - damage;
        if (health < 0)
            OnDeath();
    }

    void OnDeath()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
    }
}