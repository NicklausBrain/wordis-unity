using Assets.Wordis.BlockPuzzle.GameCore;
using Assets.Wordis.BlockPuzzle.Scripts.Controller;
using UnityEngine;

namespace Assets.Wordis.BlockPuzzle.Scripts.GamePlay
{
    public class GamingSwipesController : MonoBehaviour
    {
        private void Update()
        {
            if (SwipeManager.IsSwipingLeft())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                GamePlayUI.Instance.HandleGameEvent(GameEvent.Left);
            }

            if (SwipeManager.IsSwipingRight())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                GamePlayUI.Instance.HandleGameEvent(GameEvent.Right);
            }

            if (SwipeManager.IsSwipingDown())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                GamePlayUI.Instance.HandleGameEvent(GameEvent.Down);
            }
        }
    }
}
