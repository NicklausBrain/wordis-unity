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
using UnityEngine.UI;

namespace Hyperbyte
{
	/// <summary>
	/// This script component will animate the new added score and will deappear after 1 second.
	/// </summary>
    public class ScoreAnimator : MonoBehaviour
    {	
        #pragma warning disable 0649
		// Animating text.
        [SerializeField] Text txtAnimatingScore;

		// Animator controller for the text.
        [SerializeField] Animator scoereAnim;
        #pragma warning restore 0649

		// Plays animation effect with given score amount.
        public void Animate(int score)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            mousePos.y += 1;
            transform.position = mousePos;

            txtAnimatingScore.text = score.ToString();
            scoereAnim.SetTrigger("Animate");
        }
    }
}