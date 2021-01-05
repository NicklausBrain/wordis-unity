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

#if HB_UNITYADS
using UnityEngine.Advertisements;
#endif

namespace Hyperbyte.Ads
{
	/// <summary>
	/// Unity Ads configuration. All details can be filled from scriptable instance inspector via Hyperbyte -> Ad Settings menu item.
	/// </summary>
	public class UnityAdsSettings : ScriptableObject 
	{
		#pragma warning disable 0649
		// Android keys.
		[SerializeField] string gameId_android;
		[SerializeField] string bannerPlacement_android;
		[SerializeField] string interstitialPlacement_android;
		[SerializeField] string rewardedPlacement_android;
		
		// iOS Keys.
		[SerializeField] string gameId_iOS;
		[SerializeField] string bannerPlacement_iOS;
		[SerializeField] string interstitialPlacement_iOS;
		[SerializeField] string rewardedPlacement_iOS;

		// Test ads enable or not.
		#pragma warning disable 0169
		[SerializeField] bool enableTestAds = false;
		#pragma warning restore 0169

		// Banner ad position.
		[SerializeField] BannerAdPosition bannerAdPosition;

		// Banner ad bf color.
		[SerializeField] string bannerBGColor;
        #pragma warning restore 0649

		public string GetGameId() {
			#if UNITY_ANDROID
			return gameId_android;
			#elif UNITY_IOS
			return gameId_iOS;
			#else 
			return "";
			#endif
		}

		// Returns banner placement id for selected platform.
		public string GetBannerPlacement() {
			#if UNITY_ANDROID
			return bannerPlacement_android;
			#elif UNITY_IOS
			return bannerPlacement_iOS;
			#else 
			return "";
			#endif
		}

		// Returns interstitial placement id for selected platform.
		public string GetInterstitialPlacement() {
			#if UNITY_ANDROID
			return interstitialPlacement_android;
			#elif UNITY_IOS
			return interstitialPlacement_iOS;
			#else 
			return "";
			#endif
		}

		// Returns rewarded placement id for selected platform.
		public string GetRewardedPlacement() {
			#if UNITY_ANDROID
			return rewardedPlacement_android;
			#elif UNITY_IOS
			return rewardedPlacement_iOS;
			#else 
			return "";
			#endif
		}

		// Test mode enabled or not.
		public bool GetTestMode() {
			#if UNITY_EDITOR
			return true;
			#else
			return enableTestAds;
			#endif
		}

		#if HB_UNITYADS
		// Returns banner ad position.
		public BannerPosition GetBannerPosition() 
		{
			BannerPosition position = BannerPosition.BOTTOM_CENTER;
			switch(bannerAdPosition) 
			{
				case BannerAdPosition.TOP_RIGHT:
					position = BannerPosition.TOP_RIGHT;
				break;
				case BannerAdPosition.TOP_CENTER:
					position = BannerPosition.TOP_CENTER;
				break;
				case BannerAdPosition.TOP_LEFT:
					position = BannerPosition.TOP_LEFT;
				break;
				case BannerAdPosition.CENTER:
					position = BannerPosition.CENTER;
				break;
				case BannerAdPosition.BOTTOM_RIGHT:
					position = BannerPosition.BOTTOM_RIGHT;
				break;
				case BannerAdPosition.BOTTOM_CENTER:
				position = BannerPosition.BOTTOM_CENTER;
				break;
				case BannerAdPosition.BOTTOM_LEFT:
				position = BannerPosition.BOTTOM_LEFT;
				break;
			}
			return position;
		}
		#endif
	}
}
