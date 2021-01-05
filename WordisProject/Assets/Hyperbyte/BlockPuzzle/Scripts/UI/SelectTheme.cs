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
	public class SelectTheme : MonoBehaviour
	{   
        #pragma warning disable 0649
		[SerializeField] GameObject themeButtonTemplate;

		[SerializeField] GameObject themeListContent;
        #pragma warning restore 0649

        /// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		private void Awake() {
			PrepareThemeSelectionScreen();
		}

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
		private void OnEnable() 
		{
            UIController.Instance.EnableCurrencyBalanceButton();
		}

		/// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
		private void OnDisable() {
            UIController.Instance.Invoke("DisableCurrencyBalanceButton",0.1F);
        }

		void PrepareThemeSelectionScreen()
		{
			UITheme currentUITheme = ThemeManager.Instance.GetCurrentTheme();

            if (currentUITheme != null)
            {
                UIThemeSettings uiThemeSettings = (UIThemeSettings)Resources.Load("UIThemeSettings");
                foreach (ThemeConfig setting in uiThemeSettings.allThemeConfigs) {
                    if (setting.isEnabled && setting.themeName != "" && setting.uiTheme != null) {
                        GetThemeButton(setting, setting.defaultStatus, setting.themeName.Equals(ThemeManager.Instance.GetCurrentThemeName()));
                    }
                }
            }
        }

        GameObject GetThemeButton(ThemeConfig setting, int defaultUnlockStatus, bool isActive = false)
        {
            GameObject themeButton = (GameObject)Instantiate(themeButtonTemplate);
            themeButton.transform.SetParent(themeListContent.transform);
            themeButton.name = setting.themeName;
            themeButton.transform.localScale = Vector3.one;
            themeButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            themeButton.transform.SetAsLastSibling();
            themeButton.SetActive(true);
            
            bool unlockStatus = false;
            if(setting.defaultStatus == 1) {
                PlayerPrefs.SetInt("ThemeUnlockStatus_"+setting.themeName, 1);
            }

            if((PlayerPrefs.GetInt("ThemeUnlockStatus_"+setting.themeName, 0) == 1)) {
                unlockStatus = true;
            }
            themeButton.GetComponent<ThemeButton>().SetTheme(setting, unlockStatus, isActive);
            return themeButton;
        }

        public void OnCloseButtonPressed() {
			if (InputManager.Instance.canInput()) {
				UIFeedback.Instance.PlayButtonPressEffect();
				gameObject.Deactivate();
			}
		}
	}
}
