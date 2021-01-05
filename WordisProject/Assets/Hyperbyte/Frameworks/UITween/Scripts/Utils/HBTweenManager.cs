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

namespace Hyperbyte.UITween
{    
    /// <summary>
    /// UI Tween Managers.
    /// </summary>
    public class HBTweenManager : MonoBehaviour
    {
        // Anchor X.
        public static HBFloatTweener AnchorX(RectTransform rectT,  float fromValue, float toValue, float duration) {
            HBFloatTweener tweener = rectT.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.AnchorX, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Anchor Y.
        public static HBFloatTweener AnchorY(RectTransform rectT,  float fromValue, float toValue, float duration) {
            HBFloatTweener tweener = rectT.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.AnchorY, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Anchor Z.
        public static HBFloatTweener AnchorZ(RectTransform rectT,  float fromValue, float toValue, float duration) {
            HBFloatTweener tweener = rectT.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.AnchorZ, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Vector2 Tween.
        public static HBVector2Tweener AnchoredPosition(RectTransform rectT, Vector2 fromValue, Vector2 toValue, float duration)
        {
            HBVector2Tweener tweener = rectT.gameObject.AddComponent<HBVector2Tweener>();
            tweener.SetTweenParams(AnimationType.AnchoredPosition, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Vector3 Tween.
        public static HBVector3Tweener AnchoredPosition3D(RectTransform rectT, Vector3 fromValue, Vector3 toValue, float duration)
        {
            HBVector3Tweener tweener = rectT.gameObject.AddComponent<HBVector3Tweener>();
            tweener.SetTweenParams(AnimationType.AnchoredPosition3D, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // PositionX Tween.
        public static HBFloatTweener PositionX(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.PositionX, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local PositionX Tween.
        public static HBFloatTweener LocalPositionX(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.LocalPositionX, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // PositionY Tween.
        public static HBFloatTweener PositionY(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.PositionY, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local PositionY Tween.
        public static HBFloatTweener LocalPositionY(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.LocalPositionY, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // PositionZ Tween.
        public static HBFloatTweener PositionZ(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.PositionZ, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local PositionZ Tween.
        public static HBFloatTweener LocalPositionZ(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.LocalPositionZ, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Position Tween.
        public static HBVector3Tweener Position(Transform transform, Vector3 fromValue, Vector3 toValue, float duration)
        {
            HBVector3Tweener tweener = transform.gameObject.AddComponent<HBVector3Tweener>();
            tweener.SetTweenParams(AnimationType.Position, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local Position Tween.
        public static HBVector3Tweener LocalPosition(Transform transform, Vector3 fromValue, Vector3 toValue, float duration)
        {
            HBVector3Tweener tweener = transform.gameObject.AddComponent<HBVector3Tweener>();
            tweener.SetTweenParams(AnimationType.LocalPosition, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local RotationX Tween.
        public static HBFloatTweener LocalRotationToX(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.LocalRotationToX, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local RotationY Tween.
        public static HBFloatTweener LocalRotationToY(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.LocalRotationToY, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local RotationZ Tween.
        public static HBFloatTweener LocalRotationToZ(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.LocalRotationToZ, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local RotationTo Tween.
        public static HBVector3Tweener LocalRotationTo(Transform transform, Vector3 fromValue, Vector3 toValue, float duration)
        {
            HBVector3Tweener tweener = transform.gameObject.AddComponent<HBVector3Tweener>();
            tweener.SetTweenParams(AnimationType.LocalRotationTo, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local ScaleX Tween.
        public static HBFloatTweener LocalScaleX(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.LocalScaleX, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local ScaleY Tween.
        public static HBFloatTweener LocalScaleY(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.LocalScaleY, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local ScaleZ Tween.
        public static HBFloatTweener LocalScaleZ(Transform transform, float fromValue, float toValue, float duration)
        {
            HBFloatTweener tweener = transform.gameObject.AddComponent<HBFloatTweener>();
            tweener.SetTweenParams(AnimationType.LocalScaleZ, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Local Scale Tween.
        public static HBVector3Tweener LocalScale(Transform transform, Vector3 fromValue, Vector3 toValue, float duration)
        {
            HBVector3Tweener tweener = transform.gameObject.AddComponent<HBVector3Tweener>();
            tweener.SetTweenParams(AnimationType.LocalScale, fromValue, toValue, duration);
            tweener.Play();
            return tweener;
        }

        // Sprite Alpha.
        public static HBSpriteAlpha SetAlpha(SpriteRenderer spriteR, float endValue, float time) { 
            HBSpriteAlpha tweener = spriteR.gameObject.AddComponent<HBSpriteAlpha>();
            tweener.SetTweenParams(AnimationType.Color, spriteR.color.a, endValue, time);
            tweener.Play();
            return tweener;
        }

        // Image Alpha
        public static HBImageAlpha SetAlpha(Image image, float endValue, float time) {
            HBImageAlpha tweener = image.gameObject.AddComponent<HBImageAlpha>();
            tweener.SetTweenParams(AnimationType.Color, image.color.a, endValue, time);
            tweener.Play();
            return tweener;
        }

        // Text Color
        public static HBTextAlpha SetAlpha(Text image, float endValue, float time) {
            HBTextAlpha tweener = image.gameObject.AddComponent<HBTextAlpha>();
            tweener.SetTweenParams(AnimationType.Color, image.color.a, endValue, time);
            tweener.Play();
            return tweener;
        }

        // CanvasGroup Alpha
        public static HBCanvasGroupAlpha SetAlpha(CanvasGroup canvasGroup, float endValue, float time) {
            HBCanvasGroupAlpha tweener = canvasGroup.gameObject.AddComponent<HBCanvasGroupAlpha>();
            tweener.SetTweenParams(AnimationType.Color, canvasGroup.alpha, endValue, time);
            tweener.Play();
            return tweener;
        }


        // Sprite Color
        public static HBSpriteColor SetColor(SpriteRenderer spriteR, Color endValue, float time) {
            HBSpriteColor tweener = spriteR.gameObject.AddComponent<HBSpriteColor>();
            tweener.SetTweenParams(AnimationType.Color, spriteR.color, endValue, time);
            tweener.Play();
            return tweener;
        }

        // Image Color
        public static HBImageColor SetColor(Image image, Color endValue, float time) {
            HBImageColor tweener = image.gameObject.AddComponent<HBImageColor>();
            tweener.SetTweenParams(AnimationType.Color, image.color, endValue, time);
            tweener.Play();
            return tweener;
        }

        // Text Color
        public static HBTextColor SetColor(Text text, Color endValue, float time) {
            HBTextColor tweener = text.gameObject.AddComponent<HBTextColor>();
            tweener.SetTweenParams(AnimationType.Color, text.color, endValue, time);
            tweener.Play();
            return tweener;
        }

        // Image Fill Amount
        public static HBImageFillAmount FillAmount(Image image, float endValue, float time) {
            HBImageFillAmount tweener = image.gameObject.AddComponent<HBImageFillAmount>();
            tweener.SetTweenParams(AnimationType.Color, image.fillAmount, endValue, time);
            tweener.Play();
            return tweener;
        }
    }
}
