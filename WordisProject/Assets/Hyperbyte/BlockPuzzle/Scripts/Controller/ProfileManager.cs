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

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Hyperbyte.Ads;
using Hyperbyte.HapticFeedback;

namespace Hyperbyte
{
	/// <summary>
	/// This script compont manages and saves the basic status if user control including sound, music, haptic feedback, notification, ad status etc.
	/// </summary>
    public class ProfileManager : Singleton<ProfileManager>
    {
        [SerializeField] AppSettings appSettings;

        // Sound status change event action.
        public static event Action<bool> OnSoundStatusChangedEvent;

        // Music status change event action.
        public static event Action<bool> OnMusicStatusChangedEvent;

        // Vibration/Haptic feedback status change event action.
        public static event Action<bool> OnVibrationStatusChangedEvent;

        // Notification status change event action.
        public static event Action<bool> OnNotificationStatusChangedEvent;

		// Returns current status of sound.
        private bool isSoundEnabled = true;
        public bool IsSoundEnabled
        {
            get
            {
                return isSoundEnabled;
            }
        }

		// Returns current status of music.
        private bool isMusicEnabled = true;
        public bool IsMusicEnabled
        {
            get
            {
                return isMusicEnabled;
            }
        }

		// Returns current status of Vibrations/Haptic Feedback.
        private bool isVibrationEnabled = true;
        public bool IsVibrationEnabled
        {
            get
            {
                return isVibrationEnabled;
            }
        }

		// Returns current status of Notifications.
        private bool isNotificationEnabled = true;
        public bool IsNotificationEnabled
        {
            get
            {
                return isNotificationEnabled;
            }
        }

		// Whether user will be served ads or not.
        [System.NonSerialized] bool isUserAdFree = false;

		// List of all sessions when review nad should be shown to user.
        [HideInInspector] public List<int> appLaunchReviewSessions = new List<int>();
        
        [HideInInspector] public List<int> gameOverReviewSessions = new List<int>();

		// Profile manager has initialized or not.
        bool hasInitialised = false;

        /// <summary>
    	/// Start is called on the frame when a script is enabled just before
    	/// any of the Update methods is called the first time.
    	/// </summary>
        private void Start()
        {
            Initialise();
        }

		/// <summary>
		/// Initializes the profilemanager.
		/// </summary>
        void Initialise()
        {
            if (appSettings == null)
            {
                appSettings = (AppSettings)Resources.Load("AppSettings");
                if (appSettings.showReviewPopupOnLaunch && appSettings.reviewPopupAppLaunchCount != string.Empty) {
                    appLaunchReviewSessions = appSettings.reviewPopupAppLaunchCount.Split(',').Select(n => Convert.ToInt32(n)).ToList();
                }

                if (appSettings.showReviewPopupOnGameOver && appSettings.reviewPopupGameOverCount != string.Empty) {
                    gameOverReviewSessions = appSettings.reviewPopupGameOverCount.Split(',').Select(n => Convert.ToInt32(n)).ToList();
                }
            }
            hasInitialised = true;
            initProfileStatus();
        }

		/// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
		    /// Initiate haptic feedback generator.
            HapticFeedbackGenerator.InitHapticFeedbackGenerator();
        }

		/// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
		    /// Releases haptic feedback generator.
            HapticFeedbackGenerator.ReleaseHapticFeedbackGenerator();
        }

        /// <summary>
        /// Inits the audio status.
        /// </summary>
        public void initProfileStatus()
        {
			// Fetches the status of all user setting and invokes event callbacks for each settings.
			
            isMusicEnabled = (PlayerPrefs.GetInt("isMusicEnabled", 0) == 0) ? true : false;
            isVibrationEnabled = (PlayerPrefs.GetInt("isVibrationEnabled", 0) == 0) ? true : false;
            isNotificationEnabled = (PlayerPrefs.GetInt("isNotificationEnabled", 0) == 0) ? true : false;
            isSoundEnabled = (PlayerPrefs.GetInt("isSoundEnabled", 0) == 0) ? true : false;

            if ((!isSoundEnabled) && (OnSoundStatusChangedEvent != null))
            {
                OnSoundStatusChangedEvent.Invoke(isSoundEnabled);
            }
            if ((!isMusicEnabled) && (OnMusicStatusChangedEvent != null))
            {
                OnMusicStatusChangedEvent.Invoke(isMusicEnabled);
            }
            if ((!isVibrationEnabled) && (OnVibrationStatusChangedEvent != null))
            {
                OnVibrationStatusChangedEvent.Invoke(isVibrationEnabled);
            }
            if (!appSettings.enableVibrations) { isVibrationEnabled = false; }

            if ((!isNotificationEnabled) && (OnNotificationStatusChangedEvent != null))
            {
                OnNotificationStatusChangedEvent.Invoke(isNotificationEnabled);
            }

            isUserAdFree = ((PlayerPrefs.GetInt("isUserAdFree", 0) == 1) ? true : false);
        }

        /// <summary>
        /// Toggles the sound status.
        /// </summary>
        public void ToggleSoundStatus()
        {
            isSoundEnabled = (isSoundEnabled) ? false : true;
            PlayerPrefs.SetInt("isSoundEnabled", (isSoundEnabled) ? 0 : 1);

            if (OnSoundStatusChangedEvent != null)
            {
                OnSoundStatusChangedEvent.Invoke(isSoundEnabled);
            }
        }

        /// <summary>
        /// Toggles the music status.
        /// </summary>
        public void ToggleMusicStatus()
        {
            isMusicEnabled = (isMusicEnabled) ? false : true;
            PlayerPrefs.SetInt("isMusicEnabled", (isMusicEnabled) ? 0 : 1);

            if (OnMusicStatusChangedEvent != null)
            {
                OnMusicStatusChangedEvent.Invoke(isMusicEnabled);
            }
        }

        /// <summary>
        /// Toggles the vibration status.
        /// </summary>
        public void TogglVibrationStatus()
        {
            isVibrationEnabled = (isVibrationEnabled) ? false : true;
            PlayerPrefs.SetInt("isVibrationEnabled", (isVibrationEnabled) ? 0 : 1);

            if (OnVibrationStatusChangedEvent != null)
            {
                OnVibrationStatusChangedEvent.Invoke(isVibrationEnabled);
            }
        }

        /// <summary>
        /// Toggles the notification status.
        /// </summary>
        public void ToggleNotificationStatus()
        {
            isNotificationEnabled = (isNotificationEnabled) ? false : true;
            PlayerPrefs.SetInt("isNotificationEnabled", (isNotificationEnabled) ? 0 : 1);

            if (OnNotificationStatusChangedEvent != null)
            {
                OnNotificationStatusChangedEvent.Invoke(isNotificationEnabled);
            }
        }

        // Returns the app setting scriptable instance.
        public AppSettings GetAppSettings()
        {
            if (!hasInitialised)
            {
                Initialise();
            }
            return appSettings;
        }

        /// <summary>
        /// Sets app as ad free. Will be called when user purchase inapp to remove ads.
        /// </summary>
        public void SetAppAsAdFree()
        {
            PlayerPrefs.SetInt("isUserAdFree", 1);
            isUserAdFree = true;
            AdManager.Instance.HideBanner();
        }

        public bool IsAppAdFree()
        {
            return isUserAdFree;
        }

        /// <summary>
        /// Returns best score for the given mode.
        /// </summary>
        public int GetBestScore(GameMode gameMode)
        {
            return PlayerPrefs.GetInt("bestScore_" + gameMode, 0);
        }

        /// <summary>
        /// Saves best for the give mode.
        /// </summary>
        public void SetBestScore(int score, GameMode gameMode)
        {
            PlayerPrefs.SetInt("bestScore_" + gameMode, score);
        }
    }
}
