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
using UnityEngine;
using Hyperbyte.Localization;
using Hyperbyte.UITween;

namespace Hyperbyte
{
    /// <summary>
    /// UIController controlls the entire UI Navigation of the game.
    /// </summary>
    public class UIController : Singleton<UIController>
    {
        List<string> screenStack = new List<string>();

        [SerializeField] Canvas UICanvas;

        [Header("UI Screens")]
        public HomeScreen homeScreen;
        public GamePlayUI gameScreen;
        public Hyperbyte.Tutorial.GamePlayUI gameScreen_Tutorial;

        [Header("Public Members.")]
        public GameObject shopScreen;
        public GameObject settingScreen;
        public GameObject consentScreen;
        public GameObject reviewScreen;
        public GameObject selectModeScreen;
        public GameObject pauseGameScreen;
        public GameObject rescueGameScreen;
        public GameObject selectThemeScreen;
        public GameObject purchaseSuccessScreen;
        public GameObject commonMessageScreen;
        public GameObject dailyRewardScreen;
        public GameObject gameOverScreen;
        public GameObject lanagueSelectionScreen;
        public GameObject currencyBalanceButton;

        public GameObject tipView;

        [Header("Other Public Members")]
        public RectTransform ShopButtonGemsIcon;
        public Transform RuntimeEffectSpawnParent;

        // Ordered popup stack is used when another popup tries to open when already a popup is opened. Ordered stack will control it and add upcoming popups
        // to queue so it will load automatically when alreay existing popup gets closed.
        List<string> orderedPopupStack = new List<string>();

        [System.NonSerialized] public GameMode cachedSelectedMode = GameMode.Classic;

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
            /// Enables home screen on game start.

            homeScreen.gameObject.Activate();
            
           
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            // Registeres session update callback.
            SessionManager.OnSessionUpdatedEvent += OnSessionUpdated;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            // Unregisteres session update callback.
            SessionManager.OnSessionUpdatedEvent -= OnSessionUpdated;
        }

        /// <summary>
        /// Session Updated callback.
        /// </summary>
        private void OnSessionUpdated(SessionInfo info) {
            CheckForReviewAppPopupOnLauch(info.currentSessionCount);
        }

        /// <summary>
        /// Try to show review screen if app setting has review popup on current session id.
        /// </summary>
        void CheckForReviewAppPopupOnLauch(int currentSessionCount)
        {
            bool canShowReviewPopup = true;

            if ((!ProfileManager.Instance.GetAppSettings().showReviewPopupOnLaunch))
            {
                canShowReviewPopup = false;
                return;
            }

            if (PlayerPrefs.HasKey("AppRated") && ProfileManager.Instance.GetAppSettings().neverShowReviewPopupIfRated)
            {
                canShowReviewPopup = false;
                return;
            }

            if (canShowReviewPopup && (ProfileManager.Instance.appLaunchReviewSessions.Contains(currentSessionCount))) {
                ShowReviewPopup();
            }
        }


        /// <summary>
        /// Try to show review screen if app setting has review popup on current gameover id.
        /// </summary>
        public void CheckForReviewAppPopupOnGameOver(int currentGameOver)
        {
            bool canShowReviewPopup = true;

            if ((!ProfileManager.Instance.GetAppSettings().showReviewPopupOnGameOver))
            {
                canShowReviewPopup = false;
                return;
            }

            if (PlayerPrefs.HasKey("AppRated") && ProfileManager.Instance.GetAppSettings().neverShowReviewPopupIfRated)
            {
                canShowReviewPopup = false;
                return;
            }

            if (canShowReviewPopup && (ProfileManager.Instance.gameOverReviewSessions.Contains(currentGameOver))) {
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
                    if (screenStack.Count > 0)
                    {
                        ProcessBackButton(Peek());
                    }
                }
            }
        }

        /// <summary>
        /// Adds the latest activated gameobject to stack.
        /// </summary>
        public void Push(string screenName)
        {
            if (!screenStack.Contains(screenName))
            {
                screenStack.Add(screenName);
            }
        }

        /// <summary>
        /// Returns the name of last activated gameobject from the stack.
        /// </summary>
        public string Peek()
        {
            if (screenStack.Count > 0)
            {
                return screenStack[screenStack.Count - 1];
            }
            return "";
        }

        /// <summary>
        /// Removes the last gameobject name from the stack.
        /// </summary>
        public void Pop(string screenName)
        {
            if (screenStack.Contains(screenName))
            {
                screenStack.Remove(screenName);

                if (orderedPopupStack.Contains(screenName))
                {
                    orderedPopupStack.Remove(screenName);

                    if (orderedPopupStack.Count > 0) {
                        ShowDailogFromStack();
                    }
                }
            }
        }

        /// <summary>
        /// On pressing back button of device, the last added popup/screen will get deactivated based on state of the game. 
        /// </summary>
        void ProcessBackButton(string currentScreen)
        {
            switch (currentScreen)
            {
                case "HomeScreen":
                    QuitGamePopup();
                    break;
                case "SelectMode":
                    selectModeScreen.Deactivate();
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
                case "SelectLangauge":
                    lanagueSelectionScreen.Deactivate();
                break;

                case "PauseGame":
                    pauseGameScreen.Deactivate();
                break;

                case "RescueGame":
                GamePlayUI.Instance.OnRescueCancelled();
                rescueGameScreen.Deactivate();
                break;
            }
        }

        /// <summary>
        /// Opens a quit game popup.
        /// </summary>
        void QuitGamePopup()
        {
            new CommonDialogueInfo().SetTitle(LocalizationManager.Instance.GetTextWithTag("txtQuitTitle")).
            SetMessage(LocalizationManager.Instance.GetTextWithTag("txtQuitConfirm")).
            SetPositiveButtomText(LocalizationManager.Instance.GetTextWithTag("txtNo")).
            SetNegativeButtonText(LocalizationManager.Instance.GetTextWithTag("txtYes")).
            SetMessageType(CommonDialogueMessageType.Confirmation).
            SetOnPositiveButtonClickListener(() => {
                UIController.Instance.commonMessageScreen.Deactivate();

            }).SetOnNegativeButtonClickListener(() =>
            {
                QuitGame();
                UIController.Instance.commonMessageScreen.Deactivate();
            }).Show();
        }

        // Quits the game.
        public void QuitGame()
        {
            Invoke("QuitGameAfterDelay", 0.5F);
        }

        /// <summary>
        /// Quits game after little delay.  Waiting for poup animation to get completed.
        /// </summary>
        void QuitGameAfterDelay()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_ANDROID
                //On Android, on quitting app, App actully won't quit but will be sent to background. So it can be load fast while reopening. 
				AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
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
            new CommonDialogueInfo().SetTitle(title).
            SetMessage(message).
            SetMessageType(CommonDialogueMessageType.Info).
            SetOnConfirmButtonClickListener(() => {
                UIController.Instance.commonMessageScreen.Deactivate();
            }).Show();
        }

        /// <summary>
        /// Unload unused asset. please call this on safe place as it might give a slight lag. 
        /// </summary>
        public void ClearCache()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        /// <summary>
        /// Shows Consent Dialogue.
        /// </summary>
        public void ShowConsentDialogue()
        {
            orderedPopupStack.Add(consentScreen.name);
            ShowDailogFromStack();
        }

        /// <summary>
        /// Opens Daily Reward screen if day has changed.
        /// </summary>
        public void ShowDailyRewardsPopup()
        {
            orderedPopupStack.Add(dailyRewardScreen.name);
            ShowDailogFromStack();
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

            if (!canShowReviewPopup) {
                orderedPopupStack.Add(reviewScreen.name);
                ShowDailogFromStack();
            }
        }

        /// <summary>
        /// Controlls the ordered stack.
        /// </summary>
        void ShowDailogFromStack()
        {
            if (orderedPopupStack.Count > 0)
            {
                string screenName = orderedPopupStack[0];

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
        public void LoadGamePlay(GameMode gameMode)
        {
            bool showTutorial = false;

            if (!PlayerPrefs.HasKey("tutorialShown"))
            {
                GamePlaySettings gamePlaySettings = (GamePlaySettings)Resources.Load("GamePlaySettings");
                showTutorial = gamePlaySettings.tutorialModeSettings.modeEnabled;

                if (!showTutorial) {
                    PlayerPrefs.SetInt("tutorialShown", 1);
                }
            }

            homeScreen.gameObject.Deactivate();
            if(showTutorial) {
                gameScreen_Tutorial.gameObject.Activate();
                cachedSelectedMode = gameMode;
            } else {
                gameScreen.gameObject.Activate();
                gameScreen.GetComponent<GamePlayUI>().StartGamePlay(gameMode);
            }            
        }

        public bool IsGamePlay()
        {
            if (Peek().Equals(gameScreen.name))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Open Home screen when user presses home button from gameover screen.
        /// </summary>
        public void OpenHomeScreenFromGameOver()
        {
            StartCoroutine(OpenHomeScreenFromGameOverCoroutine());
        }

        IEnumerator OpenHomeScreenFromGameOverCoroutine()
        {
            GamePlayUI.Instance.ResetGame();
            yield return new WaitForSeconds(0.1f);
            gameScreen.gameObject.Deactivate();
            gameOverScreen.Deactivate();
            homeScreen.gameObject.Activate();
        }

        /// <summary>
        /// Open Home screen when user presses home button from pause screen during gameplay.
        /// </summary>
        public void OpenHomeScreenFromPauseGame() {
            StartCoroutine(OpenHomeScreenFromPauseGameCoroutine());
        }

        IEnumerator OpenHomeScreenFromPauseGameCoroutine()
        {
            GamePlayUI.Instance.ResetGame();
            yield return new WaitForSeconds(0.1f);
            gameScreen.gameObject.Deactivate();
            pauseGameScreen.Deactivate();
            homeScreen.gameObject.Activate();
        }

         /// <summary>
        /// Enables currency balance button. Currency balance button will be shown during shop screen, reward adding or reducing current only.
        /// </summary>
        public void EnableCurrencyBalanceButton() {
            currencyBalanceButton.GetComponent<CanvasGroup>().SetAlpha(1, 0.3F);
        }

        /// <summary>
        /// Disable currency balance button.
        /// </summary>
        public void DisableCurrencyBalanceButton()
        {
            if(! (selectThemeScreen.activeSelf || rescueGameScreen.activeSelf || gameOverScreen.activeSelf || shopScreen.activeSelf || purchaseSuccessScreen.activeSelf))
            {
                if (currencyBalanceButton != null && currencyBalanceButton.activeSelf) {
                    currencyBalanceButton.GetComponent<CanvasGroup>().SetAlpha(0, 0.3F);
                }
            }
        }

        public void PlayAddGemsAnimationAtPosition(Vector3 position, float delay) 
        {
            GameObject rewardAnim = (GameObject)Instantiate(Resources.Load("RewardAnimation")) as GameObject;
            rewardAnim.transform.SetParent(RuntimeEffectSpawnParent);
            rewardAnim.GetComponent<RectTransform>().position = position;
            rewardAnim.transform.localScale = Vector3.one;
            rewardAnim.GetComponent<RewardAddAnimation>().PlayGemsBalanceUpdateAnimation(ShopButtonGemsIcon.position, delay);
        }

        public void PlayDeductGemsAnimation(Vector3 position, float delay) {
            GameObject rewardAnim = (GameObject)Instantiate(Resources.Load("RewardAnimation")) as GameObject;
            rewardAnim.transform.SetParent(RuntimeEffectSpawnParent);
            rewardAnim.GetComponent<RectTransform>().position = ShopButtonGemsIcon.position;
            rewardAnim.transform.localScale = Vector3.one;
            rewardAnim.GetComponent<RewardAddAnimation>().PlayGemsBalanceUpdateAnimation(position, delay);
        }

        
        public void ShowTipWithTextIdAtPosition(Vector2 tipPosition, Vector2 anchor, string tipText) {
			ShowTipAtPosition(tipPosition, anchor, LocalizationManager.Instance.GetTextWithTag(tipText));
		}

		public void ShowTipWithTextIdAtPosition(Vector2 tipPosition, Vector2 anchor, string tipText, float duration) {
            ShowTipAtPosition(tipPosition, anchor, LocalizationManager.Instance.GetTextWithTag(tipText), duration);
		}

        public void ShowTipAtPosition(Vector2 tipPosition, Vector2 anchor, string tipText) {
			tipView.GetComponent<TipView>().ShowTipAtPosition(tipPosition, anchor, tipText);
            tipView.Activate(false);
		}

		public void ShowTipAtPosition(Vector2 tipPosition, Vector2 anchor, string tipText, float duration) {
            tipView.GetComponent<TipView>().ShowTipAtPosition(tipPosition, anchor, tipText, duration);
            tipView.Activate(false);
		}

        public void ShowTimeModeTip() {
             if(!PlayerPrefs.HasKey("timeTip")) 
            {
                UIController.Instance.ShowTipWithTextIdAtPosition(new Vector2(0,-350F), new Vector2(0.5F, 1), "txtTimeTip1", 6F);
                Invoke("ShowTimeModeTip2",5F);
            }
        }

        void ShowTimeModeTip2() {
            UIController.Instance.ShowTipWithTextIdAtPosition(new Vector2(0,-350F), new Vector2(0.5F, 1), "txtTimeTip2", 7F);
            PlayerPrefs.SetInt("timeTip",1);
        }

        public void ShowBombPlaceTip() {
            UIController.Instance.ShowTipWithTextIdAtPosition(new Vector2(0,520F), new Vector2(0.5F, 0), "txtBombTip", 4.5F);
        }
    }
}