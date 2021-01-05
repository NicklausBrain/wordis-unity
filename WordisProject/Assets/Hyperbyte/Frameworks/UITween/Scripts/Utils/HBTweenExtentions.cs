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

namespace  Hyperbyte.UITween
{
    /// <summary>
    /// Extentions for the tweens classes.
    /// </summary>
    public static class HBTweenExtentions
    {
        //Rect Transform
        public static HBFloatTweener AnchorX(this RectTransform rectT, float endValue, float time) {
            return HBTweenManager.AnchorX(rectT, rectT.anchoredPosition.x, endValue, time);
        }

        public static HBFloatTweener AnchorY(this RectTransform rectT, float endValue, float time)
        {
            return HBTweenManager.AnchorY(rectT, rectT.anchoredPosition.y, endValue, time);
        }

        public static HBFloatTweener AnchorZ(this RectTransform rectT, float endValue, float time)
        {
            return HBTweenManager.AnchorZ(rectT, rectT.anchoredPosition3D.z, endValue, time);
        }

        public static HBVector2Tweener AnchoredPosition(this RectTransform rectT, Vector2 endValue, float time)
        {
            return HBTweenManager.AnchoredPosition(rectT, rectT.anchoredPosition, endValue, time);
        }

        public static HBVector3Tweener AnchoredPosition3D(this RectTransform rectT, Vector3 endValue, float time)
        {
            return HBTweenManager.AnchoredPosition3D(rectT, rectT.anchoredPosition3D, endValue, time);
        }

        //PositionX
        public static HBFloatTweener PositionX(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.PositionX(transform, transform.position.x, endValue, time);
        }

        //LocalPositionX
        public static HBFloatTweener LocalPositionX(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.LocalPositionX(transform, transform.localPosition.x, endValue, time);
        }

        //PositionY
        public static HBFloatTweener PositionY(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.PositionY(transform, transform.position.y, endValue, time);
        }

        //LocalPositionY
        public static HBFloatTweener LocalPositionY(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.LocalPositionY(transform, transform.localPosition.y, endValue, time);
        }

        //PositionZ
        public static HBFloatTweener PositionZ(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.PositionZ(transform, transform.position.z, endValue, time);
        }

        //LocalPositionZ
        public static HBFloatTweener LocalPositionZ(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.LocalPositionZ(transform, transform.localPosition.z, endValue, time);
        }

        //Position
        public static HBVector3Tweener Position(this Transform transform, Vector3 endValue, float time)
        {
            return HBTweenManager.Position(transform, transform.position, endValue, time);
        }

        //LocalPosition
        public static HBVector3Tweener LocalPosition(this Transform transform, Vector3 endValue, float time)
        {
            return HBTweenManager.LocalPosition(transform, transform.localPosition, endValue, time);
        }


        //LocalRotationX
        public static HBFloatTweener LocalRotationToX(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.LocalRotationToX(transform, transform.localEulerAngles.x, endValue, time);
        }

        //LocalRotationY
        public static HBFloatTweener LocalRotationToY(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.LocalRotationToY(transform, transform.localEulerAngles.y, endValue, time);
        }

        //LocalRotationZ
        public static HBFloatTweener LocalRotationToZ(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.LocalRotationToZ(transform, transform.localEulerAngles.z, endValue, time);
        }


        //LocalRotation
        public static HBVector3Tweener LocalRotationTo(this Transform transform, Vector3 endValue, float time)
        {
            return HBTweenManager.LocalRotationTo(transform, transform.localEulerAngles, endValue, time);
        }


        //LocalScaleX
        public static HBFloatTweener LocalScaleX(this RectTransform rectT, float endValue, float time)
        {
            return HBTweenManager.LocalScaleX(rectT, rectT.localScale.x, endValue, time);
        }


        //LocalScaleX
        public static HBFloatTweener LocalScaleX(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.LocalScaleX(transform, transform.localScale.x, endValue, time);
        }

        //LocalScaleY
        public static HBFloatTweener LocalScaleY(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.LocalScaleY(transform, transform.localScale.y, endValue, time);
        }

         //LocalScaleZ
        public static HBFloatTweener LocalScaleZ(this Transform transform, float endValue, float time)
        {
            return HBTweenManager.LocalScaleZ(transform, transform.localScale.z, endValue, time);
        }
         //LocalScaleZ
        public static HBVector3Tweener LocalScale(this Transform transform, Vector3 endValue, float time)
        {
            return HBTweenManager.LocalScale(transform, transform.localScale, endValue, time);
        }


        //Sprite Alpha
        public static HBSpriteAlpha SetAlpha(this SpriteRenderer spriteR, float endValue, float time) {
            return HBTweenManager.SetAlpha(spriteR, endValue, time);
        }

        //Image Alpha
        public static HBImageAlpha SetAlpha(this Image image, float endValue, float time) {
            return HBTweenManager.SetAlpha(image, endValue, time);
        }

         //Text Alpha
        public static HBTextAlpha SetAlpha(this Text txt, float endValue, float time) {
            return HBTweenManager.SetAlpha(txt, endValue, time);
        }

        //CanvasGroup Alpha
        public static HBCanvasGroupAlpha SetAlpha(this CanvasGroup canvasGroup, float endValue, float time) {
            return HBTweenManager.SetAlpha(canvasGroup, endValue, time);
        }

        //Sprite Color
        public static HBSpriteColor SetColor(this SpriteRenderer spriteR, Color endValue, float time) {
           return HBTweenManager.SetColor(spriteR, endValue, time);
        }

        //Image Color
        public static HBImageColor SetColor(this Image image, Color endValue, float time) {
            return HBTweenManager.SetColor(image, endValue, time);
        }

        //Image FillAmount
        public static HBImageFillAmount FillAmount(this Image image, float endValue, float time) {
            return HBTweenManager.FillAmount(image, endValue, time);
        }

        //Image Color
        public static HBTextColor SetColor(this Text txt, Color endValue, float time) {
            return HBTweenManager.SetColor(txt, endValue, time);
        }

        // Pauses the tween.

        public static void Pause(this Transform transform) {
            transform.SendMessage("Pause");
        }

        public static void Pause(this Image image) {
           image.SendMessage("Pause");
        }

        public static void Pause(this SpriteRenderer spriteR) {
           spriteR.SendMessage("Pause");
        }

        public static void Pause(this CanvasGroup canvasGroup) {
           canvasGroup.SendMessage("Pause");
        }

        // Resumes the tween.

        public static void Resume(this Transform transform) {
            transform.SendMessage("Resume");
        }

        public static void Resume(this Image image) {
           image.SendMessage("Resume");
        }

        public static void Resume(this SpriteRenderer spriteR) {
           spriteR.SendMessage("Resume");
        }

        public static void Resume(this CanvasGroup canvasGroup) {
           canvasGroup.SendMessage("Resume");
        }
        
        
    }
}
