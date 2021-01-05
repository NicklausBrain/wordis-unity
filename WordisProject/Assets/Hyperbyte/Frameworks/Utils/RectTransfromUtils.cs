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

namespace Hyperbyte.Utils
{    
    /// <summary>
    /// Extention class Rect Transform.
    /// </summary>
    public static class RectTransfromUtils 
    {
        // Set width of rect transform as given.
        public static void SetNewWidth(this RectTransform rect, float width) {
            rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
        }

        // Set height of rect transform as given.
        public static void SetNewHeight(this RectTransform rect, float height) {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
        }
    }   
}
