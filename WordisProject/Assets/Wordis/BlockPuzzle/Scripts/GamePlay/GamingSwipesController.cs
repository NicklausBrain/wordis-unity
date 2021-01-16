using Assets.Wordis.BlockPuzzle.GameCore;
using UnityEngine;

namespace Assets.Wordis.BlockPuzzle.Scripts.GamePlay
{
    public class GamingSwipesController : MonoBehaviour
    {
        private void Update()
        {
            if (SwipeManager.IsSwipingLeft())
            {
                GamePlayUI.Instance.HandleGameEvent(GameEvent.Left);
                Debug.Log("LEFT SWIPE");
            }

            if (SwipeManager.IsSwipingRight())
            {
                GamePlayUI.Instance.HandleGameEvent(GameEvent.Right);
                Debug.Log("RIGHT SWIPE");
            }

            if (SwipeManager.IsSwipingDown())
            {
                GamePlayUI.Instance.HandleGameEvent(GameEvent.Down);
                Debug.Log("DOWN SWIPE");
            }
        }
    }
}
