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

namespace  Hyperbyte.UITween
{
    /// <summary>
    /// Abstract Tween behaviour will be dervied by tween classes.
    /// </summary>
    public abstract class HBTweenBehaviour : MonoBehaviour
    {
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
        Transform thisTransform;
    
        // Returns Interpolation for the given ease type.
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

        // Start Playing after given delay.
        public void PlayAfterDelay() {
            isPlaying = true;
        }

        // Destroy on tween completion.
        IEnumerator DestroyThis() {
            yield return new WaitForEndOfFrame();
            if(gameObject != null && this != null) {
                Destroy(this);
            }
        }

        // Update is called every frame, if the MonoBehaviour is enabled.
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
                            SetTweenParams(GetStartPoint(), GetEndPoint());

                            if(OnLoopCompleteDelegate != null) {
                              OnLoopCompleteDelegate.Invoke(elapsedLoop);
                            }
                            break;
                            case LoopType.PingPong:
                            SetTweenParams(GetEndPoint(), GetStartPoint());

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
            object	val	= GetValue(t);

            SetValue(val);
        }
        
        // Abstact methods.
        public abstract object GetValue(float time);
        public abstract void SetValues(object fromValue, object toValue);
        public abstract object GetStartPoint();
        public abstract object GetEndPoint();

        // Set values to object property.
        private void SetValue(object val)
        {
            switch(animationType) {
                case AnimationType.AnchorX:
                    ((RectTransform) thisTransform).anchoredPosition = new Vector2((float)val, ((RectTransform)thisTransform).anchoredPosition.y);
                break;
                case AnimationType.AnchorY:
                    ((RectTransform)thisTransform).anchoredPosition = new Vector2(((RectTransform)thisTransform).anchoredPosition.x, (float)val);
                break;
                 case AnimationType.AnchorZ:
                    ((RectTransform)thisTransform).anchoredPosition3D = new Vector3(((RectTransform)thisTransform).anchoredPosition3D.x, ((RectTransform)thisTransform).anchoredPosition3D.y, (float)val);
                break;
                case AnimationType.PositionX:
                    thisTransform.position = new Vector3((float)val,thisTransform.position.y, thisTransform.position.z);
                break;
                case AnimationType.LocalPositionX:
                    thisTransform.localPosition = new Vector3((float)val,thisTransform.localPosition.y, thisTransform.localPosition.z);
                break;

                case AnimationType.PositionY:
                    thisTransform.position = new Vector3(thisTransform.position.x, (float)val, thisTransform.position.z);
                break;
                case AnimationType.LocalPositionY:
                    thisTransform.localPosition = new Vector3(thisTransform.localPosition.x, (float)val, thisTransform.localPosition.z);
                break;

                case AnimationType.PositionZ:
                    thisTransform.position = new Vector3(thisTransform.position.x,thisTransform.position.y, (float)val);
                break;
                case AnimationType.LocalPositionZ:
                    thisTransform.localPosition = new Vector3(thisTransform.localPosition.x, thisTransform.localPosition.y, (float)val);
                break;

                case AnimationType.LocalRotationToX:
                    thisTransform.localEulerAngles = new Vector3((float)val, thisTransform.localEulerAngles.y, thisTransform.localEulerAngles.z);
                    break;

                case AnimationType.LocalRotationToY:
                    thisTransform.localEulerAngles = new Vector3(thisTransform.localEulerAngles.x, (float)val, thisTransform.localEulerAngles.z);
                    break;

                case AnimationType.LocalRotationToZ:
                    thisTransform.localEulerAngles = new Vector3(thisTransform.localEulerAngles.x, thisTransform.localEulerAngles.y, (float)val);
                    break;

                case AnimationType.LocalScaleX:
                    thisTransform.localScale = new Vector3( (float)val, thisTransform.localScale.y, thisTransform.localScale.z);
                break;

                case AnimationType.LocalScaleY:
                    thisTransform.localScale = new Vector3( thisTransform.localScale.x,  (float)val, thisTransform.localScale.z);
                break;

                case AnimationType.LocalScaleZ:
                    thisTransform.localScale = new Vector3(thisTransform.localScale.x, thisTransform.localScale.y,  (float)val);
                break;

                case AnimationType.AnchoredPosition3D:
                    ((RectTransform)thisTransform).anchoredPosition3D = (Vector3)val;
                break;
                case AnimationType.Position:
                    thisTransform.position = (Vector3)val;
                break;
                case AnimationType.LocalPosition:
                    thisTransform.localPosition = (Vector3)val;
                break;

                case AnimationType.LocalRotationTo:
                    thisTransform.localEulerAngles = (Vector3)val;
                break;

                case AnimationType.LocalScale:
                    thisTransform.localScale = (Vector3)val;
                break;

                case AnimationType.AnchoredPosition:
                    ((RectTransform)thisTransform).anchoredPosition = (Vector2)val;
                break;

                case AnimationType.RotateByZ:
                    transform.localEulerAngles = new Vector3(thisTransform.localEulerAngles.x, thisTransform.localEulerAngles.y, (float)val);
                break;
            }
        }

        // Set values of tween param.
        public void SetTweenParams(AnimationType _animationType, object _fromValue, object _toValue, float _duration ) 
        {
            thisTransform = transform;
            animationType = _animationType;
            SetValues(_fromValue, _toValue);
            duration = _duration;
        }

        // Set values of tween param.
        public void SetTweenParams(object _fromValue, object _toValue) 
        {
            SetValues(_fromValue, _toValue);
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