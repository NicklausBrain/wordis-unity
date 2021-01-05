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

namespace Hyperbyte
{
	public class AudioController : Singleton<AudioController> 
	{
		[Header("Audio Soureces")]
		public AudioSource audioSource;
		public AudioSource lowSoundSource;
		
		float lowAudioDefaultVolume = 0.1F;

		[Header("Audio Clips")]
		public AudioClip btnPressSound;
		public AudioClip addGemsSound;
		public AudioClip addScoreSoundChord;
		public AudioClip addGemsSoundChord;

		#region GamePlay Sounds
		public AudioClip blockPickSound;
		public AudioClip blockPlaceSound;
		public AudioClip blockResetSound;

		public AudioClip lineBreakSound1;
		public AudioClip lineBreakSound2;
		public AudioClip lineBreakSound3;
		public AudioClip lineBreakSound4;
		#endregion

		public void PlayClip(AudioClip clip) {
			if(ProfileManager.Instance.IsSoundEnabled) { 
				audioSource.PlayOneShot(clip);
			}
		}

		public void PlayClipLow(AudioClip clip) {
			if(ProfileManager.Instance.IsSoundEnabled) { 
				lowSoundSource.volume = lowAudioDefaultVolume;
				lowSoundSource.PlayOneShot(clip);
			}
		}

		public void PlayClipLow(AudioClip clip, float volume) {
			if(ProfileManager.Instance.IsSoundEnabled) { 
				lowSoundSource.volume = volume;
				lowSoundSource.PlayOneShot(clip);
			}
		}

		public void PlayButtonClickSound() {
			if(ProfileManager.Instance.IsSoundEnabled) {
				audioSource.PlayOneShot(btnPressSound);
			}
		}

		public void PlayBlockShapePickSound() {
			if(ProfileManager.Instance.IsSoundEnabled) {
				audioSource.PlayOneShot(blockPickSound);
			}
		}

		public void PlayBlockShapePlaceSound() {
			if(ProfileManager.Instance.IsSoundEnabled) {
				audioSource.PlayOneShot(blockPlaceSound);
			}
		}

		public void PlayBlockShapeResetSound() {
			if(ProfileManager.Instance.IsSoundEnabled) {
				audioSource.PlayOneShot(blockResetSound);
			}
		}


		public void PlayLineBreakSound(int lines) {

			if(ProfileManager.Instance.IsSoundEnabled) {
				switch(lines) {
					case 1:
					audioSource.PlayOneShot(lineBreakSound1);
					break;

					case 2:
					audioSource.PlayOneShot(lineBreakSound2);
					break;

					case 3:
					audioSource.PlayOneShot(lineBreakSound3);
					break;

					case 4:
					audioSource.PlayOneShot(lineBreakSound4);
					break;

					default:
					audioSource.PlayOneShot(lineBreakSound4);
					break;
				}
			}
		}
	}
}

