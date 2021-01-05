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
using Hyperbyte.UITween;
using Hyperbyte.HapticFeedback;

namespace Hyperbyte
{
    /// <summary>
    /// This script generates gems adding/deducting effect while any change any gems balance.
    /// </summary>
    public class RewardAddAnimation : MonoBehaviour
    {   
        #pragma warning disable 0649
        [SerializeField] List<RectTransform> allElements;
        #pragma warning restore 0649
        Vector3 elementMovePosition = Vector3.zero;

        /// Starts Animation.
        public void PlayGemsBalanceUpdateAnimation(Vector3 toPosition, float delay) {
            elementMovePosition = toPosition;
            StartCoroutine(PlayAddRewardAnimationCoroutine(delay));
        }

        /// Plays animations and iterated all gems images.
        IEnumerator PlayAddRewardAnimationCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            int iterations = 10;
            for (int i = 0; i < iterations; i++)
            {
                allElements[i].Position(elementMovePosition, 0.5F).OnComplete(() =>
                {
                    if (ProfileManager.Instance.IsVibrationEnabled)
                    {
                        HapticFeedbackGenerator.Haptic(HapticFeedback.FeedbackType.LightImpact);
                    }
                });
                yield return new WaitForSeconds(0.05F);
                if (ProfileManager.Instance.IsVibrationEnabled)
                {
                    HapticFeedbackGenerator.Haptic(HapticFeedback.FeedbackType.LightImpact);
                }
            }
            Invoke("DestroyAnim", 0.5F);
        }

        void DestroyAnim()
        {
            Destroy(gameObject);
        }
    }
}
