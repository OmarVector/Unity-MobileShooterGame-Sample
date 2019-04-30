using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenuManager : MonoBehaviour
{
   [SerializeField] private RectTransform LoadingScreenLeftDoor;

   [SerializeField] private RectTransform LoadingScreenRightDoor;

   [SerializeField] private RectTransform LoadingScreenLock;

   [SerializeField] private RectTransform StartButton;
   // HardCoded Loading Level 01
   public void LoadLevel()
   {
      // Start button scaled to Zero then disabled.
      StartButton.DOScale(0, 0.3f).onComplete = delegate
      {
         StartButton.GetComponent<Button>().interactable = false;
         //Animating both Doors
         LoadingScreenLeftDoor.DOAnchorPosX(-260, 0.5f).SetEase(Ease.OutQuad);
         LoadingScreenRightDoor.DOAnchorPosX(260,0.5f).SetEase(Ease.OutQuad).onComplete = delegate
         {
            //When animation of both door end, The Lock Animation start
            LoadingScreenLock.DOAnchorPosY(0, 0.5f).onComplete = delegate
            {
               // Then Loading will start.
               LoadingScreenLock.transform.SetSiblingIndex(3);
               SceneManager.LoadSceneAsync(1);
            };
         };
      };
     
   }
}
