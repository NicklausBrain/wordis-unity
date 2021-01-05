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
using UnityEngine.UI;

namespace Hyperbyte
{
	/// <summary>
	/// This script component is attached to review screen popup windows and user can select starts and nevigate to store to review app.
	/// </summary>
    public class ReviewAppScreen : MonoBehaviour
    {
		[SerializeField] Button btnSubmitReview;

		[HideInInspector] public float minRatingToSubmitReview = 4.5F;
		[HideInInspector] public float currentRating = 0.0F;

		/// <summary>
    	/// Start is called on the frame when a script is enabled just before
    	/// any of the Update methods is called the first time.
    	/// </summary>
		private void Start() {
			minRatingToSubmitReview = ProfileManager.Instance.GetAppSettings().minRatingToNavigateToStore;	
		}

		/// <summary>
		/// Close button click listener.
		/// </summary>
		public void OnCloseButtonPressed() {
			if(InputManager.Instance.canInput()) {
				UIFeedback.Instance.PlayButtonPressEffect();
				UIController.Instance.reviewScreen.Deactivate();
			}
		}

		/// <summary>
		/// Submit button click listener.
		/// </summary>
		public void OnSubmitButtonPressed() {
			if(InputManager.Instance.canInput()) {
				UIFeedback.Instance.PlayButtonPressEffect();
				Invoke("NavigateToStore",0.2F);
				UIController.Instance.reviewScreen.Deactivate();
			}
		}

		private void NavigateToStore() 
		{
			if(currentRating >= ProfileManager.Instance.GetAppSettings().minRatingToNavigateToStore) 
			{
				#if UNITY_IOS
				Application.OpenURL("itms-apps://itunes.apple.com/app/id" + ProfileManager.Instance.GetAppSettings().appleID);
				#elif UNITY_ANDROID

				switch (ProfileManager.Instance.GetAppSettings().currentAndroidStore)
				{
					//Google
					case 0:
						Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
					break;

					//Amazon
					case 1:
						Application.OpenURL(ProfileManager.Instance.GetAppSettings().amazonReviewURL);
					break;

					//Samsung
					case 2:
						Application.OpenURL(ProfileManager.Instance.GetAppSettings().samsungReviewURL);
					break;
				}
				#endif
				PlayerPrefs.SetInt("AppRated",1);
			}
		} 
    }
}
