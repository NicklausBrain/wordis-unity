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

namespace Hyperbyte
{
	/// <summary>
	/// Scriptable for ad settings. Can be configured from Hyperbye -> Ad Settings menu item.
	/// </summary>
	public class AppSettings : ScriptableObject
	{
		#region CommonSettings
		
		// Android store like google, amazon, samsung etc.
		public int currentAndroidStore = 0;

		// Privacy policy url.
		public string privacyPolicyURL;

		// Support url need to be enabled and need to show in settings screen or not.
		public bool enableSupportURL = true;

		// Support url is support is enabled.
		public string supportURL;

		// Apple Id to nevigate to store.
		public string appleID;
		
		#endregion


		#region ReviewSettings

		// Review popup should be enabled or not.
		public bool showReviewPopupOnLaunch = true;

		// At which launch count review request should be made. enter numbers seperated by comma.
		public string reviewPopupAppLaunchCount;

		// Review popup should be enabled or not.
		public bool showReviewPopupOnGameOver = true;

		// At which launch count review request should be made. enter numbers seperated by comma.
		public string reviewPopupGameOverCount;

		// Navigate to store for review if user selected minimum star from review popup.
		public float minRatingToNavigateToStore = 4.5F;

		// Apple native store review request should be made instead of popup.
		public bool showAppleStoreReviewPopupOniOS = true;

		// Should show review popup if already rated.
		public bool neverShowReviewPopupIfRated = true;

		// Store revire url amazon store.
		public string amazonReviewURL = "";

		// Store revire url samsung store.
		public string samsungReviewURL = "";
		
		#endregion

		// Vibration permission should be added to android manifest.
		public bool enableVibrations = true;
		
		#region InventorySettings

		// Default amount of gems at starting of game.
		public int defaultGemsAmount = 240;
		
		// Free gems reward on watching rewaerded video.
		public int watchVideoRewardAmount = 35;

		// Gems amout to rescue game.
		public int rescueGameGemsAmount = 35;
		
		#endregion

		#region DailyRewards

		// Daily reward should be enabled or not.
		public bool useDailyRewards = true;

		// Scriptable instance for daily reward settings.
		public DailyRewardSettings dailyRewardsSettings;
		#endregion
	}
}	

[System.Serializable]
public class DailyRewardSettings {
	
	// Reward on each days. Add as many days as you want with reward amount. Will keep repeating.
	public int[] allDayRewards;
}

