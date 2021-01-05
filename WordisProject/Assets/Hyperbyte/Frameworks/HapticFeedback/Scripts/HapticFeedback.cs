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
using System.Runtime.InteropServices;
using System;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace Hyperbyte.HapticFeedback
{	
	// Haptic feedback type.
    public enum FeedbackType
    {
        LightImpact,
        MediumImpact,
        HeavyImpact,
        Selection,
        Success,
        Warning,
        Failure,
    }

	/// <summary>
	/// This calss acts as plugin to generate Haptic feedback on iOS device. In turn it also uses Amplitude and vibrations to generate similar like 
	/// Effect on android. iOS device above iOS 7 have dedicated haptic motor which will be utilized to generate haptic feedback. Please follow offical
	/// Documentation to further enhance to effect.static
	///
	/// iOS Official Documentation : https://developer.apple.com/documentation/uikit/uifeedbackgenerator
	/// Anndroid Official Documentation : https://developer.android.com/reference/android/os/VibrationEffect
	/// 
	/// iOS Unity Plugin Guide : https://docs.unity3d.com/Manual/PluginsForIOS.html
	/// Unity Android Plugin Guide :  https://docs.unity3d.com/530/Documentation/Manual/PluginsForAndroid.html
	/// 
	/// NOTE : iOS 10 and above supports Haptic feedback while android sdk 26 and above supports amplitude with vibration.static
	/// </summary>

    public static class HapticFeedbackGenerator
    {
        public static bool isHapticSupported = false;

		/// <summary>
		/// Generates to given haptic type.
		/// </summary>
        public static void Haptic(FeedbackType type, bool defaultToRegularVibrate = false)
        {
			#if UNITY_ANDROID && !UNITY_EDITOR
			GenerateAndroidHapticFeedback(type,defaultToRegularVibrate);
			#elif UNITY_IOS && !UNITY_EDITOR
			GenerateiOSHapticFeedback (type, defaultToRegularVibrate);
			#endif
        }

		//=========================================== ANDROID SPECIFIC ===========================================//

		/// <summary>
		/// Duration and implitude setting for various effect on android.
		/// </summary>
		#if UNITY_ANDROID && !UNITY_EDITOR
		private static int _sdkVersion = -1;
		public 	static 	long	Duration_ImpactLight 			= 	20;
		public 	static 	int 	Amplitude_ImpactLight 			= 	40;
        private static 	long[] 	Pattern_ImpactLight 			= 	{ 0, Duration_ImpactLight };
        private static 	int[] 	LightImpactPatternAmplitude 	= 	{ 0, Amplitude_ImpactLight };

		public	static	long 	Duration_MediumImpact 			= 	40;
		public 	static 	int 	Amplitude_MediumImpact 			= 	120;
        private static 	long[] 	Pattern_ImpactMedium 			= 	{ 0, Duration_MediumImpact };
        private static 	int[] 	MediumImpactPatternAmplitude	=	{ 0, Amplitude_MediumImpact };

		public 	static 	long 	Duration_HeavyImpact 			= 	80;
		public 	static 	int 	Amplitude_HeavyImpact 			= 	255;
        private static 	long[] 	Pattern_ImpactHeavy 			= 	{ 0, Duration_HeavyImpact };
        private static 	int[] 	HeavyImpactPatternAmplitude 	= 	{ 0, Amplitude_HeavyImpact };
		
        private static 	long[] 	Pattern_Success 				= 	{ 0, Duration_ImpactLight, Duration_ImpactLight, Duration_HeavyImpact};
		private static 	int[] 	SuccessPatternAmplitude 		= 	{ 0, Amplitude_ImpactLight, 0, Amplitude_HeavyImpact};

		private static 	long[] 	Pattern_Warning 				= 	{ 0, Duration_HeavyImpact, Duration_ImpactLight, Duration_MediumImpact};
		private static 	int[] 	WarningPatternAmplitude 		= 	{ 0, Amplitude_HeavyImpact, 0, Amplitude_MediumImpact};
		
		private static 	long[] 	Pattern_Failure 				= 	{ 0, Duration_MediumImpact, Duration_ImpactLight, Duration_MediumImpact, Duration_ImpactLight, Duration_HeavyImpact, Duration_ImpactLight, Duration_ImpactLight};
		private static 	int[] 	FailurePatternAmplitude 		= 	{ 0, Amplitude_MediumImpact, 0, Amplitude_MediumImpact, 0, Amplitude_HeavyImpact, 0, Amplitude_ImpactLight};
		#endif

		#if UNITY_ANDROID && !UNITY_EDITOR
		
		// Instance of unity player class.
		private static AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		
		// Current activity instance.
		private static AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		// Get System service for vibration class.
		private static AndroidJavaObject AndroidVibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

		// Vibration effect class.
		private static AndroidJavaClass VibrationEffectClass;
		
		// Vibration effect object.
		private static AndroidJavaObject VibrationEffect;
		
		// Default amplitude amount.
		private static int DefaultAmplitude;
		
		private static IntPtr AndroidVibrateMethodRawClass = AndroidJNIHelper.GetMethodID(AndroidVibrator.GetRawClass(), "vibrate", "(J)V", false);
		private static jvalue[] AndroidVibrateMethodRawClassParameters = new jvalue[1];
		#endif

		/// <summary>
		/// Initialize vibration effect class.
		/// </summary>
        private static void VibrationEffectClassInitialization()
        {
			#if UNITY_ANDROID && !UNITY_EDITOR
			if (VibrationEffectClass == null)
            {
                VibrationEffectClass = new AndroidJavaClass ("android.os.VibrationEffect");
            }	
			#endif
        }

		/// <summary>
		/// Generates the given feedack type.
		/// </summary>
		public static void GenerateAndroidHapticFeedback(FeedbackType type, bool defaultToRegularVibrate = false)
        {
			#if UNITY_ANDROID && !UNITY_EDITOR
			switch (type)
			{
				// Light Impact.
				case FeedbackType.LightImpact:
					AndroidVibrate (Pattern_ImpactLight, LightImpactPatternAmplitude, -1);
					break;

				// Medium Impact.
				case FeedbackType.MediumImpact:
					AndroidVibrate (Pattern_ImpactMedium, MediumImpactPatternAmplitude, -1);
					break;

				// Heavy Impact.
				case FeedbackType.HeavyImpact:
					AndroidVibrate (Pattern_ImpactHeavy, HeavyImpactPatternAmplitude, -1);
					break;

				// Selection Impact.
				case FeedbackType.Selection:
					AndroidVibrate (Duration_ImpactLight, Amplitude_ImpactLight);
					break;

				// Success Impact.
				case FeedbackType.Success:
					AndroidVibrate(Pattern_Success, SuccessPatternAmplitude, -1);
					break;

				// Warning Impact.
				case FeedbackType.	Warning:
					AndroidVibrate(Pattern_Warning, WarningPatternAmplitude, -1);
					break;

				// Failure Impact.
				case FeedbackType.Failure:
					AndroidVibrate(Pattern_Failure, FailurePatternAmplitude, -1);
					break;
			}
			#endif
        }

		/// <summary>
		///	Will be called if android device does not support amplitude.
		/// </summary>
        public static void AndroidVibrate(long milliseconds)
        {
			#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidVibrateMethodRawClassParameters[0].j = milliseconds;
            AndroidJNI.CallVoidMethod(AndroidVibrator.GetRawObject(), AndroidVibrateMethodRawClass, AndroidVibrateMethodRawClassParameters);
			#endif
        }

		/// <summary>
		/// Android vibrate with ampitude.
		/// </summary>
        public static void AndroidVibrate(long milliseconds, int amplitude)
        {
			#if UNITY_ANDROID && !UNITY_EDITOR
			if ((_sdkVersion < 26)) 
			{ 
				AndroidVibrate (milliseconds); 
			}
			else
			{
				VibrationEffectClassInitialization ();
				VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject> ("createOneShot", new object[] { milliseconds,	amplitude });
                AndroidVibrator.Call ("vibrate", VibrationEffect);
			}
			#endif
        }

		/// <summary>
		/// Android vibrate with pattern and ampitude.
		/// </summary>
        public static void AndroidVibrate(long[] pattern, int repeat)
        {
			#if UNITY_ANDROID && !UNITY_EDITOR
			if ((_sdkVersion < 26))
			{ 
				AndroidVibrator.Call ("vibrate", pattern, repeat);
			}
			else
			{
				VibrationEffectClassInitialization ();
				VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject> ("createWaveform", new object[] { pattern,	repeat });
                AndroidVibrator.Call ("vibrate", VibrationEffect);
            }
			#endif
        }

		/// <summary>
		/// Android vibrate with pattern and ampitude.
		/// </summary>
        public static void AndroidVibrate(long[] pattern, int[] amplitudes, int repeat)
        {
			#if UNITY_ANDROID && !UNITY_EDITOR
			if ((_sdkVersion < 26))
			{ 
				AndroidVibrator.Call ("vibrate", pattern, repeat);
			}
			else
			{
				VibrationEffectClassInitialization ();
				VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject> ("createWaveform", new object[] { pattern,	amplitudes, repeat });
				AndroidVibrator.Call ("vibrate", VibrationEffect);
			}
			#endif
        }

		/// <summary>
		/// Cancel vibration effect.
		/// </summary>
        public static void AndroidCancelVibrations()
        {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidVibrator.Call("cancel");
			#endif
        }

		//========================================= END ANDROID SPECIFIC =========================================//

		//============================================= IOS SPECIFIC =============================================//

		#if UNITY_IOS && !UNITY_EDITOR
		
		[DllImport ("__Internal")]
		private static extern void InitHapticFeedback();

		[DllImport ("__Internal")]
		private static extern void ReleaseHapticFeedback();
		
		[DllImport ("__Internal")]
		private static extern void LightImpactHaptic();
		
		[DllImport ("__Internal")]
		private static extern void MediumImpactHaptic();
		
		[DllImport ("__Internal")]
		private static extern void HeavyImpactHaptic();
		
		[DllImport ("__Internal")]
		private static extern void SelectionHaptic();
		
		[DllImport ("__Internal")]
		private static extern void SuccessHaptic();
		
		[DllImport ("__Internal")]
		private static extern void WarningHaptic();
		
		[DllImport ("__Internal")]
		private static extern void FailureHaptic();
		
		#else

        private static void InitHapticFeedback() { }
        private static void ReleaseHapticFeedback() { }
        private static void LightImpactHaptic() { }
        private static void MediumImpactHaptic() { }
        private static void HeavyImpactHaptic() { }
        private static void SelectionHaptic() { }
        private static void SuccessHaptic() { }
        private static void WarningHaptic() { }
        private static void FailureHaptic() { }

		#endif

		#if UNITY_IOS && !UNITY_EDITOR
        private static bool iOSHapticsInitialized = false;
        #endif

		/// <summary>
		/// Initialized Haptic feedback generator.
		/// </summary>
        public static void InitHapticFeedbackGenerator()
        {
			#if UNITY_ANDROID && !UNITY_EDITOR
			_sdkVersion = int.Parse (SystemInfo.operatingSystem.Substring(SystemInfo.operatingSystem.IndexOf("-") + 1, 3));
			#elif UNITY_IOS && !UNITY_EDITOR
			InitHapticFeedback ();
			iOSHapticsInitialized = true;
			isHapticSupported = HapticsSupported();
			#endif
        }

		/// <summary>
		/// Releases Haptic feedback generator.
		/// </summary>
        public static void ReleaseHapticFeedbackGenerator()
        {
			#if UNITY_IOS && !UNITY_EDITOR
			ReleaseHapticFeedback ();
			#endif
        }

		/// <summary>
		/// Returns if haptic is supported or not. Not the best approach but this works.static :D
		/// </summary>
		/// <returns></returns>
        public static bool HapticsSupported()
        {
            bool hapticsSupported = false;
			#if UNITY_IOS && !UNITY_EDITOR
			DeviceGeneration generation = Device.generation;
			if (
				(generation == DeviceGeneration.iPhoneUnknown)	||	(generation == DeviceGeneration.iPhone3G)			||	(generation == DeviceGeneration.iPhone3GS)			||
				(generation == DeviceGeneration.iPodTouch1Gen) 	|| 	(generation == DeviceGeneration.iPodTouch2Gen) 		|| (generation == DeviceGeneration.iPodTouch3Gen)		||
				(generation == DeviceGeneration.iPodTouch4Gen) 	|| 	(generation == DeviceGeneration.iPhone4) 			|| (generation == DeviceGeneration.iPhone4S)			||
				(generation == DeviceGeneration.iPhone5) 		|| 	(generation == DeviceGeneration.iPhone5C) 			|| (generation == DeviceGeneration.iPhone5S)			||
				(generation == DeviceGeneration.iPhone6) 		|| 	(generation == DeviceGeneration.iPhone6Plus) 		|| (generation == DeviceGeneration.iPhone6S)			||
				(generation == DeviceGeneration.iPhone6SPlus) 	|| 	(generation == DeviceGeneration.iPhoneSE1Gen) 		|| (generation == DeviceGeneration.iPad1Gen)			||
            	(generation == DeviceGeneration.iPad2Gen) 		|| 	(generation == DeviceGeneration.iPad3Gen) 			|| (generation == DeviceGeneration.iPad4Gen)			||
            	(generation == DeviceGeneration.iPad5Gen) 		|| 	(generation == DeviceGeneration.iPadAir1) 			|| (generation == DeviceGeneration.iPadAir2)			||
            	(generation == DeviceGeneration.iPadMini1Gen) 	|| 	(generation == DeviceGeneration.iPadMini2Gen) 		|| (generation == DeviceGeneration.iPadMini3Gen)		||
            	(generation == DeviceGeneration.iPadMini4Gen) 	|| 	(generation == DeviceGeneration.iPadPro10Inch1Gen)	|| (generation == DeviceGeneration.iPadPro10Inch2Gen)	||
            	(generation == DeviceGeneration.iPadPro1Gen) 	|| (generation == DeviceGeneration.iPadPro2Gen)			||	(generation == DeviceGeneration.iPadUnknown)  		|| 
				(generation == DeviceGeneration.iPodTouch1Gen)	||	(generation == DeviceGeneration.iPodTouch2Gen) 		|| 	(generation == DeviceGeneration.iPodTouch3Gen) 		|| 
				(generation == DeviceGeneration.iPodTouch4Gen)	||	(generation == DeviceGeneration.iPodTouch5Gen) 		|| 	(generation == DeviceGeneration.iPodTouch6Gen) 		|| 
				(generation == DeviceGeneration.iPhone6SPlus))	//|| (generation == DeviceGeneration.iPadPro11Inch)  ||(generation == DeviceGeneration.iPadPro3Gen) 
			{
			    hapticsSupported = false;
			}
			else
			{
			    hapticsSupported = true;
			}
			#endif
            return hapticsSupported;
        }

		/// <summary>
		/// Generates Haptic feedback impact of given type.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="defaultToRegularVibrate"></param>
        public static void GenerateiOSHapticFeedback(FeedbackType type, bool defaultToRegularVibrate = false)
        {
			#if UNITY_IOS && !UNITY_EDITOR

			if (!iOSHapticsInitialized)
			{
				InitHapticFeedbackGenerator ();
			}

			if (isHapticSupported)
			{
				switch (type)
				{
					// Light Impact.
					case FeedbackType.LightImpact:
						LightImpactHaptic ();
						break;

					// Medium Impact.
					case FeedbackType.MediumImpact:
						MediumImpactHaptic ();
						break;

					// Heavy Impact.
					case FeedbackType.HeavyImpact:
						HeavyImpactHaptic ();
						break;

					// Selection Impact.
					case FeedbackType.Selection:
						SelectionHaptic ();
						break;

					// Success Impact.
					case FeedbackType.Success:
						SuccessHaptic ();
						break;

					// Warning Impact.
					case FeedbackType.Warning:
						WarningHaptic ();
						break;

					// Failure Impact.
					case FeedbackType.Failure:
						FailureHaptic ();
						break;
				}
			}
			else if (defaultToRegularVibrate)
			{
				Handheld.Vibrate();
			}
			#endif
        }


		/// <summary>
		/// Returns iOS SDK Version.
		/// </summary>
		/// <returns></returns>
        public static string iOSSDKVersion()
        {
			#if UNITY_IOS && !UNITY_EDITOR
			return Device.systemVersion;
			#else
            return null;
			#endif
        }
		//=========================================== END IOS SPECIFIC ===========================================//
    }
}