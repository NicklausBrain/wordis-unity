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
    /// Creates a instance of monobehaviour of the given script component.static You can implement this singleton behaviour in generic 
	/// form which in turn will create instance of the class in static form which can be accesses easily.
    /// </summary>
	public class Singleton<T> : MonoBehaviour where T : Component {

		private static T instance;
		public static T Instance {
			get{
				if (instance == null) {
					instance = FindObjectOfType<T> ();

					if (instance == null) {
						GameObject g = new GameObject ("Controller");
						instance = g.AddComponent<T> ();
						//g.hideFlags = HideFlags.HideInHierarchy;

					}
				}
				return instance;
			}
		}

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		void Awake()
		{
			//DontDestroyOnLoad (gameObject);
			if (instance == null ) {
				instance = this as T;
			} else {
				if (instance != this) {
					Destroy (gameObject);
				}
			}
		}
	}
}
