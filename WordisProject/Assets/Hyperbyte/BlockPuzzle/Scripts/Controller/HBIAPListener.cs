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
using Hyperbyte.Localization;
using UnityEngine;

#if HB_UNITYIAP
using UnityEngine.Purchasing;
#endif

namespace Hyperbyte
{
    public class HBIAPListener : MonoBehaviour
    {
        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable() {
            IAPManager.OnPurchaseSuccessfulEvent += OnPurchaseSuccessful;
            IAPManager.OnPurchaseFailedEvent += OnPurchaseFailed;
            IAPManager.OnRestoreCompletedEvent += OnRestoreCompleted;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable() {
            IAPManager.OnPurchaseSuccessfulEvent -= OnPurchaseSuccessful;
            IAPManager.OnPurchaseFailedEvent -= OnPurchaseFailed;
            IAPManager.OnRestoreCompletedEvent -= OnRestoreCompleted;
        }

        /// <summary>
        /// Purchase Rewards will be processed from here. You can adjust your code based on your requirements.
        /// </summary>
        /// <param name="productInfo"></param>
        void OnPurchaseSuccessful(ProductInfo productInfo) 
        {
            RewardType rewardType = ((RewardType)productInfo.rewardType); 

			switch(rewardType) 
            {
				case RewardType.REMOVE_ADS:
					ProfileManager.Instance.SetAppAsAdFree();
                    UIController.Instance.ShowMessage(LocalizationManager.Instance.GetTextWithTag("txtSuccess"), LocalizationManager.Instance.GetTextWithTag("txtInappSuccessMsg"));
				break;
				case RewardType.GEMS:
					int rewardAmount = productInfo.rewardAmount;
					CurrencyManager.Instance.AddGems(rewardAmount);
                    UIController.Instance.purchaseSuccessScreen.Activate();
				break;

                case RewardType.OTHER :
                break;
			}

            #if HB_UNITYIAP
            Product product = IAPManager.Instance.GetProductFromSku(productInfo.productName);
            #endif
        }

        void OnPurchaseFailed(string reason) {
            new CommonDialogueInfo().SetTitle(LocalizationManager.Instance.GetTextWithTag("txtOops")).
			SetMessage(LocalizationManager.Instance.GetTextWithTag("txtPurchaseFail")).
			SetMessageType(CommonDialogueMessageType.Info).
			SetOnConfirmButtonClickListener(()=> {
				UIController.Instance.commonMessageScreen.Deactivate();
                UIController.Instance.shopScreen.Activate();
			}).Show();
        }

        void OnRestoreCompleted(bool result) {
            if(result) {
                UIController.Instance.ShowMessage(("txtSuccess"), LocalizationManager.Instance.GetTextWithTag("txtInAppRestored"));
            } else {
                UIController.Instance.ShowMessage(LocalizationManager.Instance.GetTextWithTag("txtAlert"), LocalizationManager.Instance.GetTextWithTag("txtNoRestore"));
            }
        }
    }   
}
