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

using System;
using UnityEngine;
using UnityEngine.UI;
using Hyperbyte.Localization;
using Hyperbyte.UITween;

namespace Hyperbyte.Tutorial
{
    public class GamePlayUI : Singleton<GamePlayUI>
    {
        [Header("Public Class Members")]
        [Tooltip("GamePlay Script Reference")]
        public GamePlay gamePlay;

        [System.NonSerialized] public GameModeSettings currentModeSettings;

        // Stores current playing mode.
        [System.NonSerialized] public GameMode currentGameMode;

        // GamePlay Setting Scriptable Instance. Initializes on awake.
        [System.NonSerialized] GamePlaySettings gamePlaySettings;

        #region  Game Status event callbacks.
        //Event action for shape place callback.
        public static event Action OnShapePlacedEvent;
        #endregion

        public GameObject boardHighlightImage;
        public GameObject shapeDragHandImage;

        public Text txtTutorialText1;
        public Text txtTutorialText2;

        public GameObject tutorialCompletePopup;

        int tutorialStep = 1;
        /// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
        private void Awake() 
        {
            // Initializes the GamePlay Settings Scriptable.
            if (gamePlaySettings == null)  {
                gamePlaySettings = (GamePlaySettings)Resources.Load("GamePlaySettings");
            }
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            StartTutorial(GameMode.Tutorial);
        }
       
        /// <summary>
        /// Starts game with selected game mode.
        /// </summary>
        public void StartTutorial(GameMode gameMode) 
        {
            currentGameMode = gameMode;
            currentModeSettings = gamePlaySettings.tutorialModeSettings;

            //This is little static code for the tutorial.
            currentModeSettings.boardSize = BoardSize.BoardSize_5X5;

            // Enables gameplay screen if not active.
            if (!gamePlay.gameObject.activeSelf) {
                gamePlay.gameObject.SetActive(true);
            }
            
            // Generated gameplay grid.
            gamePlay.boardGenerator.GenerateBoard();
            gamePlay.boardGenerator.UpdateBoardForTutorial(1);

            // Board Generator will create and initialize board with progress data if available.
            gamePlay.blockShapeController.PrepareShapeContainer(tutorialStep);

            txtTutorialText1.text = LocalizationManager.Instance.GetTextWithTag("txtTutorial1");
            txtTutorialText1.SetAlpha(1, 0.3F);

            // Adjust size of highlighting block image.
            boardHighlightImage.transform.localScale = Vector3.one * (currentModeSettings.blockSize / 90F);
        }

        public void TutorialStepCompleted() 
        {
            tutorialStep += 1;
            gamePlay.boardGenerator.UpdateBoardForTutorial(tutorialStep);   
            gamePlay.blockShapeController.PrepareShapeContainer(tutorialStep);

            if(tutorialStep == 2) {
                txtTutorialText1.SetAlpha(0,0);
                txtTutorialText2.SetAlpha(0,0);
                txtTutorialText2.text = "";

                txtTutorialText1.text = LocalizationManager.Instance.GetTextWithTag("txtTutorial3");
                txtTutorialText1.SetAlpha(1, 0.3F);

            }

            if(tutorialStep == 3) 
            {
                GamePlay.Instance.boardGenerator.gameObject.SetActive(false);
                GamePlay.Instance.blockShapeController.gameObject.SetActive(false);
                
                boardHighlightImage.SetActive(false);
                tutorialCompletePopup.Activate(false);

                PlayerPrefs.SetInt("tutorialShown",1);
            }
        }

        public void ContinueGamePlay() {
            if (InputManager.Instance.canInput()) {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.LoadGamePlay(UIController.Instance.cachedSelectedMode);
            }
        }

        /// <summary>
        /// Returns size of the current grid.
        /// </summary>
        /// <returns></returns>
        public BoardSize GetBoardSize() {
            return currentModeSettings.boardSize;
        }

        // Invokes callback for OnShapePlaced Event.
        public void OnShapePlaced() {


            if(OnShapePlacedEvent != null) {
                OnShapePlacedEvent.Invoke();
            }

            if(tutorialStep == 1) 
            {
                if(txtTutorialText2.text == "") {
                    txtTutorialText2.text = LocalizationManager.Instance.GetTextWithTag("txtTutorial2");
                    txtTutorialText2.SetAlpha(1, 0.3F);
                }
            }
        }
    }
}
