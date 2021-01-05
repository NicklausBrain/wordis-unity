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

#if HB_ADMOB
using GoogleMobileAds.Api;
#endif

namespace Hyperbyte.Ads
{
	/// <summary>
	/// Google Mobile Ads configuration. All details can be filled from scriptable instance inspector via Hyperbyte -> Ad Settings menu item.
	/// </summary>
	public class GoogleMobileAdsSettings : ScriptableObject 
	{
		#pragma warning disable 0649
		// Android keys.
		[SerializeField] string appId_android;
		[SerializeField] string bannerId_android;
		[SerializeField] string interstitialId_android;
		[SerializeField] string rewardedId_android;
		
		// iOS Keys.
		[SerializeField] string appId_iOS;
		[SerializeField] string bannerId_iOS;
		[SerializeField] string interstitialId_iOS;
		[SerializeField] string rewardedId_iOS;

		// Tag or child treatment reqiured or not.
		[SerializeField] bool tagForChildTreatment = false;

		// Banner ad position.
		[SerializeField] BannerAdPosition bannerAdPosition;

		// Banner ad bg color.
		[SerializeField] string bannerBGColor;
        #pragma warning restore 0649

		// Returns app id for selected platform.
		public string GetAppId() {
			#if UNITY_ANDROID
			return appId_android;
			#elif UNITY_IOS
			return appId_iOS;
			#else 
			return "";
			#endif
		}

		// Returns banner unit id for selected platform.
		public string GetBannetAdUnitId() {
			#if UNITY_ANDROID
			return bannerId_android;
			#elif UNITY_IOS
			return bannerId_iOS;
			#else 
			return "";
			#endif
		}

		// Returns interstitial unit id for selected platform.
		public string GetInterstitialAdUnityId() {
			#if UNITY_ANDROID
			return interstitialId_android;
			#elif UNITY_IOS
			return interstitialId_iOS;
			#else 
			return "";
			#endif
		}
		
		// Returns rewarded unit id for selected platform.
		public string GetRewardedAdUnitId() {
			#if UNITY_ANDROID
			return rewardedId_android;
			#elif UNITY_IOS
			return rewardedId_iOS;
			#else 
			return "";
			#endif
		}

		#if HB_ADMOB
		// Returns banner ad position.
		public AdPosition GetBannerPosition() 
		{
			AdPosition position = AdPosition.Bottom;
			switch(bannerAdPosition) 
			{
				case BannerAdPosition.TOP_RIGHT:
					position = AdPosition.TopRight;
				break;
				case BannerAdPosition.TOP_CENTER:
					position = AdPosition.Top;
				break;
				case BannerAdPosition.TOP_LEFT:
					position = AdPosition.TopLeft;
				break;
				case BannerAdPosition.CENTER:
					position = AdPosition.Center;
				break;
				case BannerAdPosition.BOTTOM_RIGHT:
					position = AdPosition.BottomRight;
				break;
				case BannerAdPosition.BOTTOM_CENTER:
				position = AdPosition.Bottom;
				break;
				case BannerAdPosition.BOTTOM_LEFT:
				position = AdPosition.BottomLeft;
				break;
			}
			return position;
		}
		#endif

		// Tag for child treatmennt.
		public bool GetTagForChildTreatment() {
			return tagForChildTreatment;
		}

		// Banner ad bg color.
		public string GetBannerBgColor() {
			return bannerBGColor;
		}
	}
}
