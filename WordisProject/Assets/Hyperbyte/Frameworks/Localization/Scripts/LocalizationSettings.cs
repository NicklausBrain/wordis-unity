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

namespace Hyperbyte.Localization
{
	/// <summary>
	/// Scriptable instance of localization settings containing all info regarding in game localization. 
	/// This settings can be configured from Hyperbyte -> Localization Settings menu item.
	/// </summary>
    public class LocalizationSettings : ScriptableObject
    {
        public bool useLocalization = true;
        public LocalizedLanguage[] localizedLanguages;
        public int defaultLangauge;
        public bool localizeToSystemDetectedLanguage;
    }

	/// <summary>
	/// Localization info and settings for langauge.
	/// </summary>
    [System.Serializable]
    public class LocalizedLanguage
    {
		// Name of langauges.
        public string languageName;

		// Language enabled to use or not.
        public bool isLanguageEnabled = true;

		// Disaply name of langauges. 
        public string langaugeDisplayName;

		// Langauge code.
        public string languageCode;

		// Flag of langauge.
        public Sprite languageFlag;

		// Localized string file for the configured langauge.
        public TextAsset localizedTextFile;
    }
}