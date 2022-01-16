// ©2019 - 2020 HYPERBYTE STUDIOS LLP
// All rights reserved
// Redistribution of this software is strictly not allowed.
// Copy of this software can be obtained from unity asset store only.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Assets.Wordis.BlockPuzzle.GameCore.Levels;
using Assets.Wordis.BlockPuzzle.Scripts.Controller;
using Assets.Wordis.BlockPuzzle.Scripts.UI.Extensions;
using Assets.Wordis.Frameworks.InputManager.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

namespace Assets.Wordis.BlockPuzzle.Scripts.Home
{
    public class HomeScreen : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] Button _btnPlay; // todo: rename to play button
        [SerializeField] Button btnTimeMode;
        [SerializeField] Button btnAdvanceMode;
#pragma warning restore 0649

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// Classic mode button listener.
        /// </summary>
        public void OnClassicModeButtonPressed() // todo: rename to survival
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.LoadGamePlay();
            }
        }

        /// <summary>
        /// Time mode button listener.
        /// </summary>
        public void OnTimeModeButtonPressed() // todo: drop
        {
        }

        /// <summary>
        /// Campaign button listener.
        /// </summary>
        public void OnTutorialModePressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.LoadGamePlay(new WordisTutorialLevel());
            }
        }

        /// <summary>
        /// Advance mode button listener.
        /// </summary>
        public void OnAdvanceModeButtonPressed() // todo: drop
        {
        }

        /// <summary>
        /// Advance mode button listener.
        /// </summary>
        public void OnCampaignModeButtonPressed() // todo: rename to challenges
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.selectLevelScreen.Activate();
            }
        }
    }
}