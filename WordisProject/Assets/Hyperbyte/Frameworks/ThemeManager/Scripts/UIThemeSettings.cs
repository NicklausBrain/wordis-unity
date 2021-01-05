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
	/// Theme settings scriptable instance for all the ingame themes.
	/// This settings can be configured from Hyperbyte -> Theme Settings menu item.
	/// </summary>
    public class UIThemeSettings : ScriptableObject
    {
		// Theme selection should be used or not.
        public bool useUIThemes = true;

		// Id of default theme.
        public int defaultTheme = 0;

		/// <summary>
		/// List all of thr UI Themes.
		/// </summary>
        public ThemeConfig[] allThemeConfigs;
    }

    [System.Serializable]
    public class ThemeConfig
    {
		// Theme enabled or not.
        public bool isEnabled = true;

		// Name of the theme.
        public string themeName;

		// Demo sprite to display how this theme will look during game.
        public Sprite demoSprite;

		// UI theme scriptable that contains all color and sprite tags.
        public UITheme uiTheme;

        // 0 - LOCKED, 1 - UNLOCKED
        public int defaultStatus;

        // Cost to unlock theme [GEMS]
        public int unlockCost;
    }
}