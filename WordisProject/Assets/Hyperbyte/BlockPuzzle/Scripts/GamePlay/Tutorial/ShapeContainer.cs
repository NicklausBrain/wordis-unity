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

namespace Hyperbyte.Tutorial
{
    /// <summary>
    /// This script component is attached to all block shape containers.
    /// </summary>
	public class ShapeContainer : MonoBehaviour {

        // Rect transfrom of the container inside which block shape spawns.
        [System.NonSerialized] public RectTransform blockParent;
        
        // Assigned block shape.
        [System.NonSerialized] public BlockShape blockShape;

        /// <summary>
        /// Awakes the script instance and initializes block parent to cache it.
		/// Awake is called when the script instance is being loaded.
		/// </summary>
        private void Awake() {
            blockParent = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Resets and destroy block shape on game over or game leave.
        /// </summary>
        public void Reset() {
            if(blockShape != null) {
                Destroy(blockShape.gameObject);
                blockShape = null;
            }
        }
    }
}

