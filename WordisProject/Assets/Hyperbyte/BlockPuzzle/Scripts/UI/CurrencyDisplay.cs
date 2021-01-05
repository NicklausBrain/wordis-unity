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
using UnityEngine.UI;

namespace Hyperbyte 
{
    /// <summary>
    /// This script displays the balance of currency and keeps updating on change of it during game.
    /// </summary>
    public class CurrencyDisplay : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] Text txtGemsBalance;
        [SerializeField] GameObject rewardAnimation;
        #pragma warning restore 0649

        int lastBalance = 0;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            CurrencyManager.OnCurrencyUpdated += OnCurrencyUpdatedEvent;   
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable() {
            RefreshCurrencyBalance(); 
        }

        /// <summary>
        /// Event callback for currecy balance change.
        /// </summary>
        private void OnCurrencyUpdatedEvent(int currencyBalance) {
            StartCoroutine(PlayCurrencyIncreaseCounter(currencyBalance));
        }

        /// <summary>
        /// Refreshes currency balance on enable.
        /// </summary>
        private void RefreshCurrencyBalance() {
            int currencyBalance = CurrencyManager.Instance.GetCurrentGemsBalance();
            txtGemsBalance.text = currencyBalance.ToString("N0");
            lastBalance = currencyBalance;
        }

        /// <summary>
        /// Currency amount update animation.
        /// </summary>
        IEnumerator PlayCurrencyIncreaseCounter(int currentBalance) 
        {
            int iterations = 10;
            int balanceDifference = (currentBalance - lastBalance);
            int balanceChangeEachIteration = (balanceDifference / iterations);

            int updatedBalance = lastBalance;

            yield return new WaitForSeconds(0.75F);
            for(int i = 0; i < iterations; i++) {
                updatedBalance += balanceChangeEachIteration;
                txtGemsBalance.text = updatedBalance.ToString("N0");
                AudioController.Instance.PlayClipLow(AudioController.Instance.addGemsSoundChord, 0.15F);
                yield return new WaitForSeconds(0.05F);
            }
            txtGemsBalance.text = currentBalance.ToString("N0");
            lastBalance = currentBalance;
        } 
    }
}
                            