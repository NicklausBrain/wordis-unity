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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hyperbyte.Localization
{
    public static class LocalizationExtentions
    {
        // Sets localized text with given tag to the Text.
        public static void SetTextWithTag(this Text txt, string tag)
        {
            txt.text = LocalizationManager.Instance.GetTextWithTag(tag);
        }

        public static void SetFormattedTextWithTag(this Text txt, string tag, string value1 = "", string value2 = "")
        {
            string localizedText = LocalizationManager.Instance.GetTextWithTag(tag);
            if (value1 != "" && value2 != "")
            {
                txt.text = string.Format(localizedText, value1, value2);
            }
            else
            {
                if (value1 != "")
                {
                    txt.text = string.Format(localizedText, value1);
                }
                else if (value2 != "")
                {
                    txt.text = string.Format(localizedText, value2);
                }
                else
                {
                    txt.text = localizedText;
                }
            }
        }

        public static void SetFormattedTextWithTag(this Text txt, string tag, object value1)
        {
            string localizedText = LocalizationManager.Instance.GetTextWithTag(tag);
            txt.text = string.Format(localizedText, value1.ToString());
        }
    }
}
