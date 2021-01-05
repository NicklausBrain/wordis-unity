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

namespace Hyperbyte.Ads
{
	/// <summary>
	/// Ironsource Ads configuration. All details can be filled from scriptable instance inspector via Hyperbyte -> Ad Settings menu item.
	/// </summary>
	public class IronSourceAdsSettings : ScriptableObject 
	{	
		#pragma warning disable 0649
		// Android App Id.
		[SerializeField] string appId_android;
		
		// Apple App Id.
		[SerializeField] string appId_iOS;

		// Banner ad position.
		[SerializeField] BannerAdPosition bannerAdPosition;

		// Banner ad bg color.
		[SerializeField] string bannerBGColor;
        #pragma warning restore 0649

		public string GetAppId() {
			#if UNITY_ANDROID
			return appId_android;
			#elif UNITY_IOS
			return appId_iOS;
			#else 
			return "";
			#endif
		}

		#if HB_IRONSOURCE
		// Returns banner ad position.
		public IronSourceBannerPosition GetBannerPosition() 
		{
			IronSourceBannerPosition position = IronSourceBannerPosition.BOTTOM;
			switch(bannerAdPosition) 
			{
				case BannerAdPosition.TOP_RIGHT:
				case BannerAdPosition.TOP_CENTER:
				case BannerAdPosition.TOP_LEFT:
					position = IronSourceBannerPosition.TOP;
				break;
					
				case BannerAdPosition.CENTER:
				case BannerAdPosition.BOTTOM_RIGHT:
				case BannerAdPosition.BOTTOM_CENTER:
				case BannerAdPosition.BOTTOM_LEFT:
					position = IronSourceBannerPosition.BOTTOM;
				break;
			}
			return position;
		}
		#endif

		// Banner ad bg color.
		public string GetBannerBgColor() {
			return bannerBGColor;
		}
	}
}
