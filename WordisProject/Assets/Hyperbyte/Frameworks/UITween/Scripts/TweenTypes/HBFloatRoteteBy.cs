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
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Hyperbyte.UITween
{
    /// <summary>
    /// Rotate Object Tween.
    /// </summary>
    public class HBFloatRoteteBy : MonoBehaviour
    {
        Color defaultColor;

        float fromValue = 0.0F;
        float toValue = 1.0F;

        public float delay = 0F;
        public int loopCount = 0;
        public Ease easeType = Ease.EaseIn;
        public AnimationCurve animationCurve;
        public LoopType loopType;

        public Action OnCompleteDeletegate;
        public Action<int> OnLoopCompleteDelegate;

        bool isPlaying = false;
        bool isPaused = false;

        float elapsedTime = 0;
        float duration = 1F;

        int elapsedLoop = 0;

        AnimationCurve linearAnimationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        AnimationType animationType;
        Image thisImage;

        // Returns Interpolation for the given ease type.
        public float GetValue(float t)
        {
            return Mathf.LerpUnclamped(fromValue, toValue, t);
        }

        // Start Tween after given delay.
        public HBFloatRoteteBy SetDelay(float _delay)
        {
            delay = _delay;
            return this;
        }

        // Set given ease type for the tween.
        public HBFloatRoteteBy SetEase(Ease ease)
        {
            easeType = ease;
            return this;
        }

        // Set given loop type for the tween.
        public HBFloatRoteteBy SetLoop(int _loopCount, LoopType _loopType = LoopType.PingPong)
        {
            loopCount = _loopCount;
            loopType = _loopType;

            if (_loopType == LoopType.PingPong && (_loopCount > 1))
            {
                _loopCount = (_loopCount * 2);
            }
            return this;
        }

        // Set given animation curve for the tween.
        public HBFloatRoteteBy SetAnimation(AnimationCurve curve)
        {
            easeType = Ease.Custom;
            animationCurve = curve;
            return this;
        }

        // Invokes Tween complete event callback.
        public HBFloatRoteteBy OnComplete(Action onComplete)
        {
            OnCompleteDeletegate = onComplete;
            return this;
        }

        // Invokes Tween loop complete event callback.
        public HBFloatRoteteBy OnLoopComplete(Action<int> onLoopComplete)
        {
            OnLoopCompleteDelegate = onLoopComplete;
            return this;
        }

        // Destroy on tween completion.
        IEnumerator DestroyThis()
        {
            yield return new WaitForEndOfFrame();
            if (gameObject != null && this != null)
            {
                Destroy(this);
            }
        }   

        // Returns interpolation for the given time.
        public float GetLerpT(float time)
        {
            float timeValue = (time > duration) ? duration : time;
            float t = (duration == 0) ? 1 : timeValue / duration;

            switch (easeType)
            {
                case Ease.Linear:
                    t = linearAnimationCurve.Evaluate(t);
                    break;
                case Ease.EaseIn:
                    t = HBTweenUtility.EaseIn(t);
                    break;
                case Ease.EaseOut:
                    t = HBTweenUtility.EaseOut(t);
                    break;
                case Ease.Custom:
                    t = animationCurve.Evaluate(t);
                    break;
            }
            return t;
        }

        // Starts Tween.
        public void  Play()
        {
            StartCoroutine(PlayAfterSkipFrame());
        }

        // Starts Tween after skipping first frame.
        IEnumerator PlayAfterSkipFrame()
        {
            yield return new WaitForEndOfFrame();
            if (delay <= 0)
            {
                isPlaying = true;
            }
            else
            {
                Invoke("PlayAfterDelay", delay);
            }
        }

        // Starts Tween after delay.
        public void PlayAfterDelay()
        {
            isPlaying = true;
        }


        private void Update()
        {
            if (isPlaying && !isPaused)
            {
                elapsedTime += Time.deltaTime;
                UpdateAnimation(elapsedTime);

                if (elapsedTime >= duration)
                {
                    isPlaying = false;

                    elapsedLoop += 1;

                    if ((loopCount > 1 && elapsedLoop < loopCount) || loopCount < 0)
                    {
                        switch (loopType)
                        {
                            case LoopType.Loop:
                                SetTweenParams(fromValue, toValue);

                                if (OnLoopCompleteDelegate != null)
                                {
                                    OnLoopCompleteDelegate.Invoke(elapsedLoop);
                                }
                                break;
                            case LoopType.PingPong:
                                SetTweenParams(toValue, fromValue);

                                if (elapsedLoop % 2 == 0)
                                {
                                    if (OnLoopCompleteDelegate != null)
                                    {
                                        OnLoopCompleteDelegate.Invoke((elapsedLoop / 2));
                                    }
                                }
                                break;
                        }
                        PlayAfterDelay();
                        elapsedTime = 0;
                    }
                    else
                    {
                        if (OnCompleteDeletegate != null)
                        {
                            OnCompleteDeletegate.Invoke();
                        }
                        StartCoroutine(DestroyThis());
                    }
                }
            }
        }

        // Keep updating animation on each frame.
        private void UpdateAnimation(float time)
        {
            float t = GetLerpT(time);
            float val = GetValue(t);

            SetValue(val);
        }

        // Set values to object property.
        private void SetValue(float val)
        {
            thisImage.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, val);
        }

        // Set values of tween param.
        public void SetTweenParams(AnimationType _animationType, float _fromValue, float _toValue, float _duration)
        {
            thisImage = GetComponent<Image>();
            defaultColor = thisImage.color;
            animationType = _animationType;
            fromValue = _fromValue;
            toValue = _toValue;

            // fromValue = Mathf.Clamp(fromValue,0,1);
            // toValue = Mathf.Clamp(toValue,0,1);
            duration = _duration;
        }

        // Set values of tween param.
        public void SetTweenParams(float _fromValue, float _toValue)
        {
            fromValue = _fromValue;
            toValue = _toValue;
        }

        // Pauses Tween.
        public void Pause()
        {
            isPaused = true;
        }

        // Resumes tween.
        public void Resume()
        {
            isPaused = false;
        }
    }
}
