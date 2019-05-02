using DG.Tweening;
using UnityEngine;


public class EliteUnitController : MonoBehaviour
{
    // Reference to player path which is Player Game object 
    private GameObject playerPath;
    // Reference to Rigid body of the Elite Unit
    private Rigidbody rigidBody;
    // going to use this pool to check if the gameobject is ready or not
    private bool isReady;

    private float stopPosition;

   
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        playerPath = GameObject.FindWithTag("PlayerPath");
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
      
        rigidBody.DOMoveZ(transform.position.z - 30, 3).SetEase(Ease.OutQuad).onComplete = delegate
        {
            transform.SetParent(playerPath.transform);
            Invoke("OnTimeOut",15f);
        };
    }

    // called when player didnt destroy the enemy within certain time
    private void OnTimeOut()
    {
        transform.SetParent(null);
        var randX = Random.Range(100, 200);
        var randZ = Random.Range(100, 200);
        rigidBody.DOMove(transform.position + new Vector3(randX, 0, randZ), 10).onComplete = delegate
        {
            gameObject.SetActive(false);
        };
    }
  
}
