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

using Assets.Hyperbyte.Frameworks.HapticFeedback.Scripts;
using Assets.Hyperbyte.Frameworks.Utils;

namespace Assets.Hyperbyte.BlockPuzzle.Scripts.Controller
{
    public class UIFeedback : Singleton<UIFeedback>
    {
        /// Play Haptic/Vibration Light.
        public void PlayHapticLight()
        {
            if (ProfileManager.Instance.IsVibrationEnabled)
            {
                HapticFeedbackGenerator.Haptic(FeedbackType.LightImpact);
            }
        }

        /// Play Haptic/Vibration Medium.
        public void PlayHapticMedium()
        {
            if (ProfileManager.Instance.IsVibrationEnabled)
            {
                HapticFeedbackGenerator.Haptic(FeedbackType.MediumImpact);
            }
        }

        /// Play Haptic/Vibration Heavy.
        public void PlayHapticHeavy()
        {
            if (ProfileManager.Instance.IsVibrationEnabled)
            {
                HapticFeedbackGenerator.Haptic(FeedbackType.HeavyImpact);
            }
        }


        /// Plays Button Click Sound and Haptic Feedback.
        public void PlayButtonPressEffect()
        {
            AudioController.Instance.PlayButtonClickSound();
            PlayHapticLight();
        }

        /// Plays Block Shape Pick Effect.
        public void PlayBlockShapePickEffect()
        {
            AudioController.Instance.PlayBlockShapePickSound();
            PlayHapticLight();
        }

        /// Plays Block Shape Pick Effect.
        public void PlayBlockShapePlaceEffect()
        {
            AudioController.Instance.PlayBlockShapePlaceSound();
            PlayHapticLight();
        }

        /// Plays Block Shape Pick Effect.
        public void PlayBlockShapeResetEffect()
        {
            AudioController.Instance.PlayBlockShapeResetSound();
            PlayHapticLight();
        }
    }
}