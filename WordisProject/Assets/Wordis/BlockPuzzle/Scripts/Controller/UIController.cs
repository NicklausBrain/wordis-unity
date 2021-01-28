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

using System.Collections;
using System.Collections.Generic;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.Scripts.EffectUtils;
using Assets.Wordis.BlockPuzzle.Scripts.GamePlay;
using Assets.Wordis.BlockPuzzle.Scripts.Home;
using Assets.Wordis.BlockPuzzle.Scripts.UI;
using Assets.Wordis.BlockPuzzle.Scripts.UI.Extensions;
using Assets.Wordis.Frameworks.InputManager.Scripts;
using Assets.Wordis.Frameworks.Localization.Scripts;
using Assets.Wordis.Frameworks.UITween.Scripts.Utils;
using Assets.Wordis.Frameworks.Utils;
using UnityEngine;

namespace Assets.Wordis.BlockPuzzle.Scripts.Controller
{
    /// <summary>
    /// UIController controls the entire UI Navigation of the game.
    /// </summary>
    public class UIController : Singleton<UIController>
    {
        readonly List<string> _screenStack = new List<string>();

        [SerializeField] Canvas UICanvas;

        [Header("UI Screens")]
        public HomeScreen homeScreen;
        public GamePlayUI gameScreen;

        [Header("Public Members.")]
        public GameObject shopScreen;
        public GameObject settingScreen;
        public GameObject consentScreen;
        public GameObject reviewScreen;
        public GameObject selectLevelScreen;
        public GameObject pauseGameScreen;
        public GameObject rescueGameScreen;
        public GameObject selectThemeScreen;
        public GameObject purchaseSuccessScreen;
        public GameObject commonMessageScreen;
        public GameObject dailyRewardScreen;
        public GameObject gameOverScreen;
        public GameObject languageSelectionScreen;
        public GameObject statisticsScreen;
        public GameObject currencyBalanceButton;

        public GameObject topTipView;
        public GameObject downTipView;

        [Header("Other Public Members")]
        public RectTransform ShopButtonGemsIcon;
        public Transform RuntimeEffectSpawnParent;

        // Ordered popup stack is used when another popup tries to open when already a popup is opened. Ordered stack will control it and add upcoming popups
        // to queue so it will load automatically when already existing popup gets closed.
        readonly List<string> _orderedPopupStack = new List<string>();

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            // Enables home screen on game start.

            homeScreen.gameObject.Activate();
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            // Registers session update callback.
            SessionManager.OnSessionUpdatedEvent += OnSessionUpdated;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            // Un-registers session update callback.
            SessionManager.OnSessionUpdatedEvent -= OnSessionUpdated;
        }

        /// <summary>
        /// Session Updated callback.
        /// </summary>
        private void OnSessionUpdated(SessionInfo info)
        {
            CheckForReviewAppPopupOnLaunch(info.currentSessionCount);
        }

        /// <summary>
        /// Try to show review screen if app setting has review popup on current session id.
        /// </summary>
        void CheckForReviewAppPopupOnLaunch(int currentSessionCount)
        {
            bool canShowReviewPopup = true;

            if (!ProfileManager.Instance.GetAppSettings().showReviewPopupOnLaunch)
            {
                canShowReviewPopup = false;
                return;
            }

            if (PlayerPrefs.HasKey("AppRated") && ProfileManager.Instance.GetAppSettings().neverShowReviewPopupIfRated)
            {
                canShowReviewPopup = false;
                return;
            }

            if (canShowReviewPopup && ProfileManager.Instance.appLaunchReviewSessions.Contains(currentSessionCount))
            {
                ShowReviewPopup();
            }
        }

        /// <summary>
        /// Try to show review screen if app setting has review popup on current gameover id.
        /// </summary>
        public void CheckForReviewAppPopupOnGameOver(int currentGameOver)
        {
            bool canShowReviewPopup = true;

            if (!ProfileManager.Instance.GetAppSettings().showReviewPopupOnGameOver)
            {
                canShowReviewPopup = false;
                return;
            }

            if (PlayerPrefs.HasKey("AppRated") && ProfileManager.Instance.GetAppSettings().neverShowReviewPopupIfRated)
            {
                canShowReviewPopup = false;
                return;
            }

            if (canShowReviewPopup && ProfileManager.Instance.gameOverReviewSessions.Contains(currentGameOver))
            {
                ShowReviewPopup();
            }
        }

        /// <summary>
        /// Handles the device back button, this will be used for android only. 
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (InputManager.Instance.canInput())
                {
                    if (_screenStack.Count > 0)
                    {
                        ProcessBackButton(Peek());
                    }
                }
            }
        }

        /// <summary>
        /// Adds the latest activated GameObject to stack.
        /// </summary>
        public void Push(string screenName)
        {
            if (!_screenStack.Contains(screenName))
            {
                _screenStack.Add(screenName);
            }
        }

        /// <summary>
        /// Returns the name of last activated GameObject from the stack.
        /// </summary>
        public string Peek()
        {
            if (_screenStack.Count > 0)
            {
                return _screenStack[_screenStack.Count - 1];
            }

            return string.Empty;
        }

        /// <summary>
        /// Removes the last GameObject name from the stack.
        /// </summary>
        public void Pop(string screenName)
        {
            if (_screenStack.Contains(screenName))
            {
                _screenStack.Remove(screenName);

                if (_orderedPopupStack.Contains(screenName))
                {
                    _orderedPopupStack.Remove(screenName);

                    if (_orderedPopupStack.Count > 0)
                    {
                        ShowDialogFromStack();
                    }
                }
            }
        }

        /// <summary>
        /// On pressing back button of device, the last added popup/screen will get deactivated based on state of the game. 
        /// </summary>
        private void ProcessBackButton(string currentScreen)
        {
            switch (currentScreen)
            {
                case nameof(HomeScreen):
                    QuitGamePopup();
                    break;
                case nameof(SelectLevel):
                    selectLevelScreen.Deactivate();
                    break;
                case nameof(Statistics):
                    statisticsScreen.Deactivate();
                    break;
                case "GamePlay":
                    break;
                case "Shop":
                    shopScreen.Deactivate();
                    break;
                case "Settings":
                    settingScreen.Deactivate();
                    break;
                case "CommonMessageScreen":
                    commonMessageScreen.Deactivate();
                    break;
                case "PurchaseSuccessScreen":
                    purchaseSuccessScreen.Deactivate();
                    break;
                case "ReviewAppScreen":
                    reviewScreen.Deactivate();
                    break;
                case "SelectLanguage":
                    languageSelectionScreen.Deactivate();
                    break;
                case "PauseGame":
                    pauseGameScreen.Deactivate();
                    break;
            }
        }

        /// <summary>
        /// Opens a quit game popup.
        /// </summary>
        private void QuitGamePopup()
        {
            new CommonDialogueInfo().SetTitle(LocalizationManager.Instance.GetTextWithTag("txtQuitTitle"))
                .SetMessage(LocalizationManager.Instance.GetTextWithTag("txtQuitConfirm"))
                .SetPositiveButtomText(LocalizationManager.Instance.GetTextWithTag("txtNo"))
                .SetNegativeButtonText(LocalizationManager.Instance.GetTextWithTag("txtYes"))
                .SetMessageType(CommonDialogueMessageType.Confirmation)
                .SetOnPositiveButtonClickListener(() => { Instance.commonMessageScreen.Deactivate(); })
                .SetOnNegativeButtonClickListener(() =>
                {
                    QuitGame();
                    Instance.commonMessageScreen.Deactivate();
                }).Show();
        }

        // Quits the game.
        public void QuitGame()
        {
            Invoke(nameof(QuitGameAfterDelay), 0.5F);
        }

        /// <summary>
        /// Quits game after little delay.  Waiting for poup animation to get completed.
        /// </summary>
        void QuitGameAfterDelay()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
                //On Android, on quitting app, App actually won't quit but will be sent to background. So it can be load fast while reopening. 
                AndroidJavaObject activity =
 new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack" , true);
#elif UNITY_IOS
                Application.Quit();
#endif
        }

        /// <summary>
        /// Show common pop-up. 
        /// </summary>
        public void ShowMessage(string title, string message)
        {
            new CommonDialogueInfo().SetTitle(title).SetMessage(message).SetMessageType(CommonDialogueMessageType.Info)
                .SetOnConfirmButtonClickListener(() => { Instance.commonMessageScreen.Deactivate(); })
                .Show();
        }

        /// <summary>
        /// Shows Consent Dialogue.
        /// </summary>
        public void ShowConsentDialogue()
        {
            _orderedPopupStack.Add(consentScreen.name);
            ShowDialogFromStack();
        }

        /// <summary>
        /// Opens Daily Reward screen if day has changed.
        /// </summary>
        public void ShowDailyRewardsPopup()
        {
            _orderedPopupStack.Add(dailyRewardScreen.name);
            ShowDialogFromStack();
        }

        /// <summary>
        /// Open review popup if all conditions to show review screen satisfies.
        /// </summary>
        private void ShowReviewPopup()
        {
            bool canShowReviewPopup = false;

#if UNITY_IOS
            canShowReviewPopup = UnityEngine.iOS.Device.RequestStoreReview();
           
            if (canShowReviewPopup)
            {
                PlayerPrefs.SetInt("AppRated", 1);
            }
#endif

            if (!canShowReviewPopup)
            {
                _orderedPopupStack.Add(reviewScreen.name);
                ShowDialogFromStack();
            }
        }

        /// <summary>
        /// Controls the ordered stack.
        /// </summary>
        private void ShowDialogFromStack()
        {
            if (_orderedPopupStack.Count > 0)
            {
                string screenName = _orderedPopupStack[0];

                switch (screenName)
                {
                    case "ConsentSetting":
                        if (!consentScreen.activeSelf)
                        {
                            consentScreen.Activate();
                        }

                        break;

                    case "DailyRewardScreen":
                        if (!dailyRewardScreen.activeSelf)
                        {
                            dailyRewardScreen.Activate();
                        }

                        break;

                    case "ReviewAppScreen":
                        if (!reviewScreen.activeSelf)
                        {
                            reviewScreen.Activate();
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Disables home and select mode screen and opens gameplay.
        /// </summary>
        public void LoadGamePlay(IWordisGameLevel gameLevel = null)
        {
            homeScreen.gameObject.Deactivate();

            gameScreen.gameObject.Activate();
            gameScreen
                .GetComponent<GamePlayUI>()
                .SetLevel(gameLevel)
                .RestartGame(restore: true);
        }

        /// <summary>
        /// Open Home screen when user presses home button from gameover screen.
        /// </summary>
        public void OpenHomeScreenFromGameOver()
        {
            StartCoroutine(OpenHomeScreenFromGameOverCoroutine());
        }

        private IEnumerator OpenHomeScreenFromGameOverCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            gameScreen.gameObject.Deactivate();
            gameOverScreen.Deactivate();
            homeScreen.gameObject.Activate();
        }

        /// <summary>
        /// Open Home screen when user presses home button from pause screen during gameplay.
        /// </summary>
        public void OpenHomeScreenFromPauseGame()
        {
            StartCoroutine(OpenHomeScreenFromPauseGameCoroutine());
        }

        private IEnumerator OpenHomeScreenFromPauseGameCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            gameScreen.gameObject.Deactivate();
            pauseGameScreen.Deactivate();
            homeScreen.gameObject.Activate();
        }

        /// <summary>
        /// Enables currency balance button. Currency balance button will be shown during shop screen, reward adding or reducing current only.
        /// </summary>
        public void EnableCurrencyBalanceButton()
        {
            currencyBalanceButton.GetComponent<CanvasGroup>().SetAlpha(1, 0.3F);
        }

        /// <summary>
        /// Disable currency balance button.
        /// </summary>
        public void DisableCurrencyBalanceButton()
        {
            if (!(selectThemeScreen.activeSelf ||
                  rescueGameScreen.activeSelf ||
                  gameOverScreen.activeSelf ||
                  shopScreen.activeSelf ||
                  purchaseSuccessScreen.activeSelf))
            {
                if (currencyBalanceButton != null &&
                    currencyBalanceButton.activeSelf)
                {
                    currencyBalanceButton.GetComponent<CanvasGroup>().SetAlpha(0, 0.3F);
                }
            }
        }

        public void PlayAddGemsAnimationAtPosition(Vector3 position, float delay)
        {
            GameObject rewardAnim = (GameObject)Instantiate(Resources.Load("RewardAnimation"));
            rewardAnim.transform.SetParent(RuntimeEffectSpawnParent);
            rewardAnim.GetComponent<RectTransform>().position = position;
            rewardAnim.transform.localScale = Vector3.one;
            rewardAnim.GetComponent<RewardAddAnimation>()
                .PlayGemsBalanceUpdateAnimation(ShopButtonGemsIcon.position, delay);
        }

        public void PlayDeductGemsAnimation(Vector3 position, float delay)
        {
            GameObject rewardAnim = (GameObject)Instantiate(Resources.Load("RewardAnimation"));
            rewardAnim.transform.SetParent(RuntimeEffectSpawnParent);
            rewardAnim.GetComponent<RectTransform>().position = ShopButtonGemsIcon.position;
            rewardAnim.transform.localScale = Vector3.one;
            rewardAnim.GetComponent<RewardAddAnimation>().PlayGemsBalanceUpdateAnimation(position, delay);
        }

        public void ShowTopTipAtPosition(
            Vector2 tipPosition,
            Vector2 anchor,
            string tipText,
            float duration)
        {
            topTipView.GetComponent<TipView>().ShowTipAtPosition(
                tipPosition: tipPosition,
                anchor: anchor,
                tipText: tipText,
                duration: duration);

            topTipView.Activate(false);
        }

        public void ShowDownTipAtPosition(
            Vector2 tipPosition,
            Vector2 anchor,
            string tipText,
            float duration)
        {
            downTipView.GetComponent<TipView>().ShowTipAtPosition(
                tipPosition: tipPosition,
                anchor: anchor,
                tipText: tipText,
                duration: duration);

            downTipView.Activate(false);
        }

        public void HideTips()
        {
            topTipView.GetComponent<TipView>().HideTip();
            downTipView.GetComponent<TipView>().HideTip();
        }
    }
}