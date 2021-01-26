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
using Assets.Wordis.BlockPuzzle.Scripts.GamePlay;
using Assets.Wordis.BlockPuzzle.Scripts.UI.Extensions;
using Assets.Wordis.Frameworks.InputManager.Scripts;
using UnityEngine;
using UnityEngine.UI;
using GameAnalyticsSDK;

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
            GamePlaySettings gamePlaySettings = (GamePlaySettings)Resources.Load(nameof(GamePlaySettings));

            if (!gamePlaySettings.classicModeSettings.modeEnabled)
            {
                _btnPlay.gameObject.SetActive(false);
            }

            if (!gamePlaySettings.timeModeSettings.modeEnabled)
            {
                btnTimeMode.gameObject.SetActive(false);
            }

            if (!gamePlaySettings.advancedModeSettings.modeEnabled)
            {
                btnAdvanceMode.gameObject.SetActive(false);
            }

            gamePlaySettings = null;
            GameAnalytics.Initialize();
        }

        /// <summary>
        /// Action on pressing play button on home screen. This is not used and Select Mode screen is also not in use.
        /// </summary>
        public void OnPlayButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();

                //Opens mode selection screen.
                UIController.Instance.selectLevelScreen.Activate();
            }
        }

        /// <summary>
        /// Classic mode button listener.
        /// </summary>
        public void OnClassicModeButtonPressed()
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
        public void OnTimeModeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.LoadGamePlay();
            }
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

                //Opens mode selection screen.
                //UIController.Instance.selectModeScreen.Activate();
            }
        }

        /// <summary>
        /// Advance mode button listener.
        /// </summary>
        public void OnAdvanceModeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.LoadGamePlay();
            }
        }

        /// <summary>
        /// Advance mode button listener.
        /// </summary>
        public void OnCampaignModeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.selectLevelScreen.Activate();
            }
        }
    }
}