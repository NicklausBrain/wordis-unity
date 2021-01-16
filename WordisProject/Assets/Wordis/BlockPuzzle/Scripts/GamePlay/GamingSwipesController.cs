using Assets.Wordis.BlockPuzzle.GameCore;
using UnityEngine;

namespace Assets.Wordis.BlockPuzzle.Scripts.GamePlay
{
    public class GamingSwipesController : MonoBehaviour
    {
        private Vector2 startTouchPosition;
        private Vector2 endTouchPosition;

        private void Update()
        {
            if (Input.touchCount > 0 &&
                Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(0).position;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouchPosition = Input.GetTouch(0).position;

                if (endTouchPosition.x < startTouchPosition.x)
                {
                    GamePlayUI.Instance.HandleGameEvent(GameEvent.Left);
                    Debug.Log("LEFT SWIPE");
                }

                if (endTouchPosition.x > startTouchPosition.x)
                {
                    GamePlayUI.Instance.HandleGameEvent(GameEvent.Right);
                    Debug.Log("RIGHT SWIPE");
                }

                if (endTouchPosition.y < startTouchPosition.y)
                {
                    GamePlayUI.Instance.HandleGameEvent(GameEvent.Down);
                    Debug.Log("DOWN SWIPE");
                }
            }
        }
    }
}
