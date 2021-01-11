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

using System.Collections.Generic;
using Assets.Wordis.BlockPuzzle.Scripts.Controller;
using Assets.Wordis.Frameworks.InputManager.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Wordis.BlockPuzzle.Scripts.GamePlay
{
    /// <summary>
    /// This script controls the block shapes that being place/played on board grid.
    /// It controls spawning of block shapes and organizing it.
    /// </summary>
    public class GamingButtonsController : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] Button btnArrowLeft;
        [SerializeField] Button btnArrowDown;
        [SerializeField] Button btnArrowRight;
#pragma warning disable 0649

        /// <summary>
        /// listener
        /// </summary>
        public void OnRightButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                //UIFeedback.Instance.PlayButtonPressEffect();
                //UIController.Instance.LoadGamePlay(GameMode.Classic);
            }
        }

        public void OnDownButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                //UIFeedback.Instance.PlayButtonPressEffect();
                //UIController.Instance.LoadGamePlay(GameMode.Classic);
            }
        }

        /// <summary>
        /// listener
        /// </summary>
        public void OnLeftButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                //UIFeedback.Instance.PlayButtonPressEffect();
                //UIController.Instance.LoadGamePlay(GameMode.Classic);
            }
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            // Registers game status callbacks.
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            // Unregisters game status callbacks.
        }
    }
}