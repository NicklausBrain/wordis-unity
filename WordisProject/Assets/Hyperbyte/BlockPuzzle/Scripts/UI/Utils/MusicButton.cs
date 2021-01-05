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
	public class MusicButton : MonoBehaviour 
	{
		// The button to toggle music, assigned from inspector.
		public Button btnMusic;
		// The image of the button.
		public Image btnMusicImage;
		// The On sprite for music.
		public Sprite musicOnSprite;
		// The off sprite for music.
		public Sprite musicOffSprite;

		/// <summary>
    	/// Start is called on the frame when a script is enabled just before
    	/// any of the Update methods is called the first time.
    	/// </summary>
		void Start()
		{
			btnMusic.onClick.AddListener(() => {
				if (InputManager.Instance.canInput ()) {
					UIFeedback.Instance.PlayButtonPressEffect();
					ProfileManager.Instance.ToggleMusicStatus();
				}
			});
		}

		/// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
		void OnEnable()
		{
			ProfileManager.OnMusicStatusChangedEvent += OnMusicStatusChanged;
			initMusicStatus ();
		}

		/// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
		void OnDisable()
		{
			ProfileManager.OnMusicStatusChangedEvent -= OnMusicStatusChanged;
		}

		/// <summary>
		/// Inits the music status.
		/// </summary>
		void initMusicStatus()
		{
			if(ProfileManager.Instance.IsMusicEnabled)
			{
				btnMusicImage.sprite = musicOnSprite;
			}
			else
			{
				btnMusicImage.sprite = musicOffSprite;
			}
		}

		/// <summary>
		/// Raises the music status changed event.
		/// </summary>
		void OnMusicStatusChanged (bool isMusicEnabled)
		{
			if(isMusicEnabled)
			{
				btnMusicImage.sprite = musicOnSprite;
			}
			else
			{
				btnMusicImage.sprite = musicOffSprite;
			}
		}	
	}
}