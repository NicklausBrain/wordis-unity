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

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hyperbyte
{
    /// <summary>
    /// Theme manager contrll game themes, returns requires images, colors attached to tags and controlls the event callbacks.
    /// </summary>
    public class ThemeManager : Singleton<ThemeManager>
    {
        string currentThemeName = "";
        [SerializeField] UIThemeSettings uiThemeSettings;

        [SerializeField] UITheme currentUITheme;
        [System.NonSerialized] public bool hasInitialised = false;

        public static event Action<string> OnThemeInitializedEvent;
        public static event Action<string> OnThemeChangedEvent;

        // List<ThemeConfig> allActiveThemes = new List<ThemeConfig>();

        [HideInInspector] public bool UIThemeEnabled = false;
        /// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		private void Awake()
        {
            if (uiThemeSettings == null) {
                uiThemeSettings = (UIThemeSettings)Resources.Load("UIThemeSettings");
            }

            if (uiThemeSettings.useUIThemes) {
                UIThemeEnabled = true;
                Initialise();
            }  
            else {
                currentUITheme = (UITheme) (Resources.Load("UIThemes/DefaultTheme"));
            }
        }


        /// <summary>
        /// Initializes theme manager.
        /// </summary>
        void Initialise()
        {
            if (!hasInitialised)
            {
                int defaultTheme = uiThemeSettings.defaultTheme;

                if(!PlayerPrefs.HasKey("currentThemeName")) {
                    int themeIndex = 0;
                    foreach (ThemeConfig theme in uiThemeSettings.allThemeConfigs) {
                        if (theme.isEnabled && theme.themeName != "" && theme.uiTheme != null && theme.defaultStatus == 1) {

                            if(themeIndex == defaultTheme) {
                                PlayerPrefs.SetString("currentThemeName", theme.themeName);
                                currentThemeName = theme.themeName;
                                break;
                            }
                            themeIndex++;
                        }
                    }
                } else {
                    currentThemeName = PlayerPrefs.GetString("currentThemeName");
                }

                currentUITheme = uiThemeSettings.allThemeConfigs.ToList().Find(o => o.themeName == currentThemeName).uiTheme;
                uiThemeSettings = null;
                hasInitialised = true;

                if (OnThemeInitializedEvent != null) {
                    OnThemeInitializedEvent.Invoke(currentThemeName);
                }
            }
        }

        /// <summary>
        /// Applies given theme to app.
        /// </summary>
        public void SetTheme(ThemeConfig themeSetting)
        {
            currentUITheme = themeSetting.uiTheme;
            currentThemeName = themeSetting.themeName;
            PlayerPrefs.SetString("currentThemeName", currentThemeName);

            if (OnThemeChangedEvent != null)  {
                OnThemeChangedEvent.Invoke(currentThemeName);
            }
        }

        /// <summary>
        /// Returns instance of current active theme.
        /// </summary>
        public UITheme GetCurrentTheme()
        {
            return currentUITheme;
        }

        /// <summary>
        /// Returns current active theme id.
        /// </summary>
        public string GetCurrentThemeName() {
            return currentThemeName;
        }

        /// <summary>
        /// Returns color for the given tag from selected theme scriptable.
        /// </summary>
        public Color GetThemeColorWithTag(string colorTag)
        {
            return currentUITheme.colorTags.FirstOrDefault(o => o.tagName == colorTag).tagColor;
        }

        /// <summary>
        /// Returns Sprite for the given tag from selected theme scriptable.
        /// </summary>
        public Sprite GetThemeSpriteWithTag(string spriteTag) {
            return currentUITheme.spriteTags.FirstOrDefault(o => o.tagName == spriteTag).tagSprite;
        }

        public Sprite GetBlockSpriteWithTag(string spriteTag) {
            return currentUITheme.spriteTags.FirstOrDefault(o => o.tagName == spriteTag).tagSprite;
        }
    }
}
