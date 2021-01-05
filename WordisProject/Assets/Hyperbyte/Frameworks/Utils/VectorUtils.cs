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
	/// Extention class for Vector operations.
	/// </summary>
    public static class VectorUtils
    {
        // Replace X value of Vector3 with given value.
        public static Vector3 WithNewX(this Vector3 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        // Replace Y value of Vector3 with given value.
        public static Vector3 WithNewY(this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        // Replace Z value of Vector3 with given value.
        public static Vector3 WithNewZ(this Vector3 vector, float z)
        {
            vector.z = z;
            return vector;
        }

        // Replace X value of Vector2 with given value.
        public static Vector2 WithNewX(this Vector2 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        // Replace Y value of Vector2 with given value.
        public static Vector2 WithNewY(this Vector2 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static Vector3 GetGlobalToLocalScaleFactor(Transform t)
        {
            Vector3 factor = Vector3.one;

            while (true)
            {
                Transform tParent = t.parent;

                if (tParent != null)
                {
                    factor.x *= tParent.localScale.x;
                    factor.y *= tParent.localScale.y;
                    factor.z *= tParent.localScale.z;

                    t = tParent;
                }
                else
                {
                    return factor;
                }
            }
        }
    }
}
