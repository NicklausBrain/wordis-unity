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
	public class NotificationButton : MonoBehaviour 
	{
		// The button to turn on/off notification.
		public Button btnNotification;	
		// Image of the button on which notification sprite will get assigned. Default on
		public Image btnNotificationImage; 
		// Notification on sprite.
		public Sprite notificationOnSprite;
		// Notification off sprite.
		public Sprite notificationOffSprite;

		/// <summary>
    	/// Start is called on the frame when a script is enabled just before
    	/// any of the Update methods is called the first time.
    	/// </summary>
		void Start()
		{
			btnNotification.onClick.AddListener(() => 
			{
				if (InputManager.Instance.canInput ()) {
					UIFeedback.Instance.PlayButtonPressEffect();
					ProfileManager.Instance.ToggleNotificationStatus();
				}
			});
		}

		/// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
		void OnEnable()
		{
			ProfileManager.OnNotificationStatusChangedEvent += OnNotificationStatusChanged;
			initNotificationStatus ();
		}

		/// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
		void OnDisable()
		{
			ProfileManager.OnNotificationStatusChangedEvent -= OnNotificationStatusChanged;
		}

		/// <summary>
		/// Inits the notification status.
		/// </summary>
		void initNotificationStatus()
		{
			if(ProfileManager.Instance.IsNotificationEnabled)
			{
				btnNotificationImage.sprite = notificationOnSprite;
			}
			else
			{
				btnNotificationImage.sprite = notificationOffSprite;
			}
		}

		/// <summary>
		/// Raises the notification status changed event.
		/// </summary>
		void OnNotificationStatusChanged (bool isNotificationEnabled)
		{
			if(isNotificationEnabled)
			{
				btnNotificationImage.sprite = notificationOnSprite;
			}
			else
			{
				btnNotificationImage.sprite = notificationOffSprite;
			}
		}	
	}
}