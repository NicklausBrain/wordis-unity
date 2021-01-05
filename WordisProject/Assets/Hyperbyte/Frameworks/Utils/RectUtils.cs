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
    /// Extention class Rect.
    /// </summary>
    public static class RectUtils
    {
        // Replace X value of Rect with given value.
        public static Rect WithNewX(this Rect rect, float x)
        {
            rect.x = x;
            return rect;
        }

        // Replace Y value of Rect with given value.
        public static Rect WithNewY(this Rect rect, float y)
        {
            rect.y = y;
            return rect;
        }

        // Replace Width value of Rect with given value.
        public static Rect WithNewWidth(this Rect rect, float width)
        {
            rect.width = width;
            return rect;
        }

        // Replace Height of Rect with given value.
        public static Rect WithNewHeight(this Rect rect, float height)
        {
            rect.height = height;
            return rect;
        }
    }
}
