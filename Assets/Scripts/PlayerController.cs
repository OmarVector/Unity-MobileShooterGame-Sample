using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // Storing player finger position
    private Vector3 fingerPos;

    // Storing Player jet position
    private Vector3 playerPos;

    // Reference to main Camera
    private Camera cam;

    //List of Weapons that will be enabled once the player jet on start position
    public List<GameObject> Weapons = new List<GameObject>();

    // Enable a small menu when player left his fingers from the screen to use super weapons.
    [SerializeField] private JetStatusCanvasController jetStatusCanvasController;

    private bool isSuperMenu = false;

    private void Start()
    {
        enabled = false;
        cam = Camera.main;
        Time.timeScale = 1.0f;
        // Moving the jet to start location then enable the controls .
        transform.DOMove(new Vector3(0, 120, -13), 2).SetEase(Ease.OutQuad).SetUpdate(true).onComplete = delegate
        {
            // enable player controls
            StartCoroutine(Wait(1));
            // Once the ship land on the start location, Start DO Animation of the main level.
            transform.parent.GetComponent<DOTweenAnimation>().DOPlay();
            enabled = true;
        };
    }

    public IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        // enable player weapons
        for (int i = 0; i < Weapons.Count; ++i)
        {
            Weapons[i].SetActive(true);
        }
    }

    public IEnumerator WaitToEnableCanvasAgain(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        if (isSuperMenu)
            isSuperMenu = false;
    }


    // Update is called once per frame
    void Update()
    {
        // Setting Up Controllers for Jet to follow player fingers 
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            //
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            var camerapos = cam.transform.position;
            playerPos = transform.position;

            var normalized = (camerapos - playerPos).normalized;

            var unitVector = (playerPos.y) / normalized.y;


            fingerPos = cam.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x,
                Input.GetTouch(0).position.y, unitVector));

            fingerPos.y = 120f;
            fingerPos.z += 5;

            transform.DOMove(fingerPos, 0.3f);

            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;


            if (isSuperMenu)
            {
                jetStatusCanvasController.Deactivate(); // it will be called once so no problem
                isSuperMenu = false;
            }
        }
        else
        {
           
            // Un comment this when doing build for android.
            /*{
                  Time.timeScale = 0.2f;
                  Time.fixedDeltaTime = Time.timeScale * 0.02f;
      
                  if (!isSuperMenu)
                  {
                      jetStatusCanvasController.Activate();
                      isSuperMenu = true;
                  }
              */
            
            // Comment this if condition when doing build for android.
            if (!Application.isEditor)
            {
                Time.timeScale = 0.2f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;

                if (!isSuperMenu)
                {
                    jetStatusCanvasController.Activate();
                    isSuperMenu = true;
                }
            }
        }
#endif

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            var camerapos = cam.transform.position;

            playerPos = transform.position; //A
            var normalized = (camerapos - playerPos).normalized;

            float unitVector = (playerPos.y) / normalized.y;

            fingerPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, unitVector));
            fingerPos.y = 120;
            fingerPos.z += 5;
            transform.DOMove(fingerPos, 0.3f);

            //assign the 'newTimeScale' to the current 'timeScale'  
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            if (isSuperMenu)
            {
                jetStatusCanvasController.Deactivate();
                // it will be called once so no problem
                isSuperMenu = false;
            }
        }
        else
        {
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            if (!isSuperMenu)
            {
                jetStatusCanvasController.Activate();
                isSuperMenu = true;
            }
        }

#endif
    }
}