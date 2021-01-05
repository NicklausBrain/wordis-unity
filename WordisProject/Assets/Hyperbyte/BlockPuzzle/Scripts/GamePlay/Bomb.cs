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
using Hyperbyte.UITween;

namespace Hyperbyte
{
    /// <summary>
    /// The script component attached to Bomb object. Will be used during blast mode only.
    /// </summary>
	public class Bomb : MonoBehaviour
    {
        #pragma warning disable 0649
        // Text of remaining coundown on bomb.
        [SerializeField] Text txtCounter;
        
        //Particle Ring
        [SerializeField] GameObject ringParticle;
        #pragma warning restore 0649

        // Remaining coundown amount on bomb.
        [System.NonSerialized] public int remainingCounter = 9;

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable() {
            /// Registers game status callbacks.
            GamePlayUI.OnShapePlacedEvent += GamePlayUI_OnShapePlacedEvent;  
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable() {
            /// Uregisters game status callbacks.
            GamePlayUI.OnShapePlacedEvent -= GamePlayUI_OnShapePlacedEvent;
        }

        /// <summary>
        /// Sets the given counter on bomb.
        /// </summary>
        public void SetCounter(int remainCounter) {
            remainingCounter = remainCounter;
            txtCounter.text = remainCounter.ToString();
        }

        /// <summary>
        /// Counter will keep reduding upon each block shape placed and will lead to game over or recue state on reaching to zero.
        /// </summary>
        private void GamePlayUI_OnShapePlacedEvent() {
            remainingCounter -= 1;
            txtCounter.transform.LocalScale(Vector3.zero, 0.15F).OnComplete(() =>
            {
                txtCounter.text = remainingCounter.ToString();
                ringParticle.SetActive(true);
                txtCounter.transform.LocalScale(Vector3.one, 0.15F).OnComplete(() => {
                    if (remainingCounter <= 0) {
                        GamePlayUI.Instance.TryRescueGame(GameOverReason.BOMB_BLAST);
                    }
                });
            });
        }
    }
}

