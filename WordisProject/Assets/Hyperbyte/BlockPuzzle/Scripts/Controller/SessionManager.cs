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
using System;

public class SessionManager : MonoBehaviour 
{
	public static event Action<SessionInfo> OnSessionUpdatedEvent;
	bool isFreshLauched = true;

	TimeSpan backgroundThreshHoldTimeSpan = new TimeSpan(0,0,120);
	SessionInfo currentSessionInfo = null;

	/// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
	void Start() 
	{	
		if(!PlayerPrefs.HasKey("firstOpenDate")) {
			PlayerPrefs.SetString("firstOpenDate", DateTime.Now.ToBinary().ToString());
		}

		Invoke("CheckForSessionStatus",0.5F);
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if(pauseStatus) {
			isFreshLauched = false;
			PlayerPrefs.SetString("lastPauseTime",DateTime.Now.ToBinary().ToString());
			PlayerPrefs.SetString("lastAccessedDate",DateTime.Now.ToBinary().ToString());
		} else 
		{
			if(!isFreshLauched) {
				
				bool doCheckForSessionChange = true;

				if(PlayerPrefs.HasKey("lastPauseTime")) {

					DateTime lastOpenedTime = DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("lastPauseTime")));
					TimeSpan pauseDuration = (DateTime.Now - lastOpenedTime);
					
					if(pauseDuration.TotalSeconds < backgroundThreshHoldTimeSpan.TotalSeconds) {
						doCheckForSessionChange = false;
					}
				}

				if(doCheckForSessionChange) {
					Invoke("CheckForSessionStatus",0.5F);
				}
			}
		}
	}

	void CheckForSessionStatus() 
	{
		//Calculate Session Count.
		int currentSessionCount = 0;
		
		if(PlayerPrefs.HasKey("currentSessionCount")) {
			currentSessionCount = PlayerPrefs.GetInt("currentSessionCount",0);
		}
		currentSessionCount ++;
		PlayerPrefs.SetInt("currentSessionCount", currentSessionCount);
		PlayerPrefs.DeleteKey("lastPauseTime");

		//SessionOfTheDay
		int currentSessionOfDay = PlayerPrefs.GetInt("currentSessionOfDay_"+DateTime.Now.Year + "_" + DateTime.Now.DayOfYear, 0);
		currentSessionOfDay += 1;
		PlayerPrefs.SetInt("currentSessionOfDay_"+DateTime.Now.Year + "_" + DateTime.Now.DayOfYear, currentSessionOfDay);

		//Days Since Last Accessed
		DateTime lastAccessedDate = DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("lastAccessedDate",DateTime.Now.ToBinary().ToString())));
		TimeSpan durationSinceLastAccessed = (DateTime.Now - lastAccessedDate);

		//Days Since Installed
		DateTime firstOpenDate = DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("firstOpenDate",DateTime.Now.ToBinary().ToString())));
		TimeSpan durationSinceFirstTimeAccessed = (DateTime.Now - firstOpenDate);

		currentSessionInfo = new SessionInfo(currentSessionCount, currentSessionOfDay, durationSinceLastAccessed, durationSinceFirstTimeAccessed);

		if(OnSessionUpdatedEvent != null) {
			OnSessionUpdatedEvent.Invoke(currentSessionInfo);
		}
	}

	void OnApplicationQuit() {
		PlayerPrefs.SetString("lastAccessedDate",DateTime.Now.ToBinary().ToString());
	}

	public SessionInfo getCurrentSessionInfo() {
		return currentSessionInfo;
	}
}

public class SessionInfo {
	public int currentSessionCount = 0;
	public int currentSessionOfDay = 0;
	public TimeSpan durationSinceLastAccessed;
	public TimeSpan durationSinceFirstTimeAccessed;

	public SessionInfo(int _currentSessionCount, int _currentSessionOfDay, TimeSpan _durationSinceLastAccessed, TimeSpan _durationSinceFirstTimeAccessed) {
		this.currentSessionCount = _currentSessionCount;
		this.currentSessionOfDay = _currentSessionOfDay;
		this.durationSinceLastAccessed = _durationSinceLastAccessed;
		this.durationSinceFirstTimeAccessed = _durationSinceFirstTimeAccessed;
	}
}
