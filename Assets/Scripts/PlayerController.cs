using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Storing player finger position
    private Vector3 fingerPos;
    // Storing Player jet position
    private Vector3 playerPos;
    // Reference to main Camera
    private Camera cam;
    
    //List of Weapons that will be enabled once the player jet on start position
   [SerializeField] private List<GameObject> Weapons = new List<GameObject>();
    
    private void Start()
    {
        enabled = false;
        cam = Camera.main;
      
        // Moving the jet to start location then enable the controls .
        transform.DOMove(new Vector3(0,120,-13),2 ).SetEase(Ease.OutQuad).SetUpdate(true).onComplete = delegate
        {
            // enable player controls
            enabled = true;
            // Once the ship land on the start location, Start DO Animation of the main level.
            transform.parent.GetComponent<DOTweenAnimation>().DOPlay();
            
            // enable player weapons
            for (int i = 0; i < Weapons.Count; ++i)
            {
                Weapons[i].SetActive(true);
            }
            
        };
    }


    // Update is called once per frame
    void Update()
    {
        // Setting Up Controllers for Jet to follow player fingers 
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            //
            var camerapos = cam.transform.position; 
            playerPos = transform.position; 
           
            var normalized = (camerapos - playerPos).normalized;

            var unitVector = (playerPos.y) / normalized.y;


            fingerPos =cam.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x,
                Input.GetTouch(0).position.y, unitVector));
            
            fingerPos.y = 120f;
            fingerPos.z += 1;

            transform.DOMove(fingerPos, 0.3f);
            
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        else
        {
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
#endif

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var camerapos = cam.transform.position;

            playerPos = transform.position; //A
            var normalized = (camerapos - playerPos).normalized;

            float unitVector =(playerPos.y) / normalized.y;

            fingerPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, unitVector));
            fingerPos.y = 120;
            fingerPos.z += 1;
            transform.DOMove(fingerPos, 0.3f);

            //assign the 'newTimeScale' to the current 'timeScale'  
            Time.timeScale = 1;  
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
           
        
        }
        else
        {
            Time.timeScale = 0.2f;  
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

#endif
    }
}