using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private LeapServiceProvider leapProvider;

    private Controller controller;

    private Frame currentFrame;

    private Frame 
    // Start is called before the first frame update
    void Start()
    {
        controller = leapProvider.GetLeapController();
        currentFrame = controller.Frame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnDestroy()
    {
        controller.ClearPolicy(Leap.Controller.PolicyFlag.POLICY_DEFAULT);
        controller.StopConnection();
    }
}
