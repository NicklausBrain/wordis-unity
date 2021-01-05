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
using System.Linq;
using Hyperbyte.Utils;

namespace Hyperbyte
{
	/// <summary>
	/// Theme button is attached to all theme selection buttons.
	/// </summary>
	public class ThemeButton : MonoBehaviour  
	{
        ThemeConfig currentSelectedTheme;

		#pragma warning disable 0649
		[SerializeField] Text txtThemeName;
        [SerializeField] Image imgBg;
		[SerializeField] Image imgBlockSample;
        [SerializeField] Button btnSelect;
		[SerializeField] Button btnUnlock;
		[SerializeField] Button btnActive;
		[SerializeField] GameObject imgBorder;
		[SerializeField] RectTransform gemsIcon;
		[SerializeField] Text txtUnlockPrice;
        #pragma warning restore 0649

        bool isActiveTheme = false;
		bool isUnlockedTheme = false;

		/// <summary>
		/// Visually prepares how this theme will look like.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="isActive"></param>
		public void SetTheme(ThemeConfig settings, bool unlockStatus, bool isActive = false) 
		{
            currentSelectedTheme = settings;
            imgBg.color = settings.uiTheme.colorTags.FirstOrDefault(o => o.tagName == "GPBG").tagColor;
            txtThemeName.text = settings.themeName;
            imgBlockSample.sprite = settings.demoSprite;
            txtThemeName.color = settings.uiTheme.colorTags.FirstOrDefault(o => o.tagName == "PopUpText").tagColor;
            
			btnSelect.GetComponent<Image>().color = settings.uiTheme.colorTags.FirstOrDefault(o => o.tagName == "PopUpButton").tagColor;
			btnUnlock.GetComponent<Image>().color = settings.uiTheme.colorTags.FirstOrDefault(o => o.tagName == "UnlockThemeButton").tagColor;
			btnActive.GetComponent<Image>().color = settings.uiTheme.colorTags.FirstOrDefault(o => o.tagName == "PopUpTitle").tagColor;

			txtUnlockPrice.text = settings.unlockCost.ToString();
			isUnlockedTheme = unlockStatus;
            isActiveTheme = isActive;
			imgBorder.SetActive(isActiveTheme);

			if(!unlockStatus) {
				imgBlockSample.color = imgBlockSample.color.WithNewA(0.1F);
				btnUnlock.gameObject.SetActive(true);
			} else {
				if(isActive) {
					btnActive.gameObject.SetActive(true);
				} else {
					btnSelect.gameObject.SetActive(true);
				}
			}
		}

		/// <summary>
		/// Theme selection button click listener.
		/// </summary>
		public void OnThemeButtonPressed() {
			if(InputManager.Instance.canInput()) {
				InputManager.Instance.DisableTouchForDelay();
				UIFeedback.Instance.PlayButtonPressEffect();

				if(PlayerPrefs.HasKey("ThemeUnlockStatus_"+currentSelectedTheme.themeName)) {
					ThemeManager.Instance.SetTheme(currentSelectedTheme);
				} else {
					UnlockTheme();
				}
			}
		}

		private void UnlockTheme() 
		{
			if(CurrencyManager.Instance.DeductGems(currentSelectedTheme.unlockCost)) {
				UIController.Instance.PlayDeductGemsAnimation(gemsIcon.position, 0.1F);
				PlayerPrefs.SetInt("ThemeUnlockStatus_"+currentSelectedTheme.themeName, 1);
				isUnlockedTheme = true;
				Invoke("UpdateUIAfterUnlock",1.2F);
			} else {
				UIController.Instance.shopScreen.Activate();
			}
		}

		private void UpdateUIAfterUnlock() {
			btnUnlock.gameObject.SetActive(false);
			btnSelect.gameObject.SetActive(true);
            imgBlockSample.color = imgBlockSample.color.WithNewA(1F);
        }

		/// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
		private void OnEnable() 
		{
			ThemeManager.OnThemeChangedEvent += OnThemeChanged;
		}

		/// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
		private void OnDisable() {
            ThemeManager.OnThemeChangedEvent -= OnThemeChanged;
        }

		/// <summary>
		/// Theme change event callback
		/// </summary>
		/// <param name="themeName"></param>
		void OnThemeChanged(string themeName) {
			if(isUnlockedTheme) {
				if(currentSelectedTheme.themeName.Equals(themeName)) {
					if(!isActiveTheme) {
						imgBorder.SetActive(true);
						isActiveTheme = true;
						btnActive.gameObject.SetActive(true);
						btnSelect.gameObject.SetActive(false);
					}
				} else {
					if(isActiveTheme) {
						imgBorder.SetActive(false);
						btnActive.gameObject.SetActive(false);
						btnSelect.gameObject.SetActive(true);
					}
					isActiveTheme = false;	
				}
			}
		}
	}
}
