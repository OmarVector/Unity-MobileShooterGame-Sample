using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

public class SuperWeaponCanvasController : MonoBehaviour
{
    // reference to shield button 
    public Button ShieldButton;

    // reference to laser button
    public Button LaserButton;

    [SerializeField] private GameObject laserWeapon;
    [SerializeField] private GameObject shield;

    // reference to text to render out how many shields are available .
    [SerializeField] private Text shieldCount;

    // reference to text to render out how many lasers are available .
    [SerializeField] private Text laserCount;
    // reference to PlayerController .
    [SerializeField] private PlayerController playerController;

    // Will use it for simple animation
    private RectTransform shieldT;
    private RectTransform laserT;
    
    // reference to canvas
    private Canvas canvas;

    // caching variables.
    private void Start()
    {
        shieldT = ShieldButton.GetComponent<RectTransform>();
        laserT = LaserButton.GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();

        shieldT.localScale = Vector3.zero;
        laserT.localScale = Vector3.zero;
    }

    // custom activation , we dont want to disable the game object to avoid rebuilding the canvas UI.
    public void Activate()
    {
        canvas.enabled = true;

        // number of shield + number of shield we collect during the level , notice that those extra we collected are not saved to prevent players from farming them, same for laser
        var sCount = ShipDataManager.shipDataManager.ShieldAmount + ScoreAndDropsManager.scoreAndDropsManager.tempShieldAmount;
        // number of laser, I cached them bec we need to check also if they are > 0
        var lCount = ShipDataManager.shipDataManager.LaserAmount + ScoreAndDropsManager.scoreAndDropsManager.tempLaserAmount;    
        
        // render the count of super weapon to text.
        shieldCount.text = sCount.ToString();
        laserCount.text = lCount.ToString();

        // setting button to be interactable based on if there is any available super weapon for each.
        ShieldButton.interactable = sCount > 0 && !shield.activeInHierarchy;
        LaserButton.interactable = lCount > 0 && !laserWeapon.activeInHierarchy;

        // adding simple scale animation to the buttons.
        shieldT.DOScale(0.1f, 0.1f);
        laserT.DOScale(0.1f, 0.1f);
    }

    // Deactivating the canvas.
    public void Deactivate()
    {
        Time.timeScale = 1;
        // re-enable the canvas in case player didnt get his control on the screen once again after he uses a super weapon.
        StartCoroutine(playerController.WaitToEnableCanvasAgain(0.5f));
        
        // simple close animation to buttons
        shieldT.DOScale(Vector3.zero, 0.2f).SetUpdate(true);
        laserT.DOScale(Vector3.zero, 0.2f).SetUpdate(true).onComplete = delegate
        {
            canvas.enabled = false;
            playerController.enabled = true;
        };
    }
}