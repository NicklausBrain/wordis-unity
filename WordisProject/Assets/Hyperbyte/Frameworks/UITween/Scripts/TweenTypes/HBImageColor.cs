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
    public class HBImageColor : MonoBehaviour
    {
        Color fromColor;
        Color toColor;

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
        public Color GetValue(float t)
        {
            return UnityEngine.Color.Lerp(fromColor, toColor, t);
        }

        // Start Tween after given delay.
        public HBImageColor SetDelay(float _delay) {
            delay = _delay;
            return this;
        } 

        // Set given ease type for the tween.
        public HBImageColor SetEase(Ease ease) {
            easeType = ease;
            return this;
        }

        // Set given loop type for the tween.
        public HBImageColor SetLoop(int _loopCount, LoopType _loopType = LoopType.PingPong) {
            loopCount = _loopCount;
            loopType = _loopType;

            if(_loopType == LoopType.PingPong && (_loopCount > 1)) {
                _loopCount = (_loopCount * 2);
            }
            return this;
        }

        // Set given animation curve for the tween.
        public HBImageColor SetAnimation(AnimationCurve curve) {
            easeType = Ease.Custom;
            animationCurve = curve;
            return this;
        }

        // Invokes Tween complete event callback.
        public HBImageColor OnComplete(Action onComplete){
            OnCompleteDeletegate = onComplete;
            return this;
        }

        // Invokes Tween loop complete event callback.
        public HBImageColor OnLoopComplete(Action<int> onLoopComplete){
            OnLoopCompleteDelegate = onLoopComplete;
            return this;
        }

        // Destroy on tween completion.
        IEnumerator DestroyThis() {
            yield return new WaitForEndOfFrame();
            if(gameObject != null && this != null) {
                Destroy(this);
            }
        }

        // Returns interpolation for the given time.
        public float GetLerpT(float time)
        {
            float timeValue = (time > duration) ? duration : time;
            float t = (duration == 0) ? 1 : timeValue / duration;

            switch(easeType) {
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
        public void Play() {
            StartCoroutine(PlayAfterSkipFrame());
        }

        // Starts Tween after skipping first frame.
        IEnumerator PlayAfterSkipFrame() {
            yield return new WaitForEndOfFrame();
            if(delay <= 0) {
                isPlaying = true;
            } else {
                Invoke("PlayAfterDelay",delay);
            }
        }

        // Starts Tween given delay.
        public void PlayAfterDelay() {
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
                    
                    if((loopCount > 1 && elapsedLoop < loopCount) || loopCount < 0)  
                    {
                        switch(loopType) {
                            case LoopType.Loop:
                            SetTweenParams(fromColor, toColor);

                            if(OnLoopCompleteDelegate != null) {
                              OnLoopCompleteDelegate.Invoke(elapsedLoop);
                            }
                            break;
                            case LoopType.PingPong:
                            SetTweenParams(toColor, fromColor);

                            if(elapsedLoop % 2 == 0) {
                                if(OnLoopCompleteDelegate != null) {
                                    OnLoopCompleteDelegate.Invoke((elapsedLoop /2));
                                }
                            }
                            break;
                        }
                        PlayAfterDelay();
                        elapsedTime = 0;
                    } else {
                        if(OnCompleteDeletegate != null) {
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
            float	t	= GetLerpT(time);
            Color	val	= GetValue(t);

            SetValue(val);
        }

        // Set values to object property.
        private void SetValue(Color val)
        {
            thisImage.color = val;
        }

        // Set values of tween param.
        public void SetTweenParams(AnimationType _animationType, Color _fromValue, Color _toValue, float _duration ) 
        {
            thisImage = GetComponent<Image>();
            animationType = _animationType;
            fromColor = _fromValue;
            toColor = _toValue;
            duration = _duration;
        }   

        // Set values of tween param.
        public void SetTweenParams(Color _fromValue, Color _toValue) 
        {
            fromColor = _fromValue;
            toColor = _toValue;
        }

        // Pauses Tween.
        public void Pause() {
            isPaused = true;
        }

         // Resumes tween.
        public void Resume() {
            isPaused = false;
        }
    }
}
