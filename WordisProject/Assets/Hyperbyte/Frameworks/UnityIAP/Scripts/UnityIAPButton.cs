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

#if HB_UNITYIAP
using UnityEngine.Purchasing;
#endif

namespace Hyperbyte
{
    /// <summary>
    /// Any UI Button with attached this script will work as IAP Button and request a IAP Purchase of selected SKU from the dropdown selection. 
    /// Also this script acts as restore iap button.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UnityIAPButton : MonoBehaviour
    {
        // Button type - Purchase or Restore.
        public int buttonType = 1;

        // Which product sku to be assigned to this button.
        public int inAppProductIndex = 0;

        // Info of the producrt.
        ProductInfo thisProduct;

        // IAP title text.
        public Text txtTitle;

        // IAP price text.
        public Text txtPrice;

        // IAP description text.
        public Text txtDescription;

        // Button has initialized or not.
        bool hasInitialized = false;

        Button thisButton;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            thisButton = GetComponent<Button>();
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            Invoke("InitIAPProduct", 0.1F);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            thisButton.onClick.RemoveListener(OnPurchaseButtonPressed);
        }

        /// <summary>
        ///  Fetrched IAP product and assign to button which listener.
        /// </summary>
        void InitIAPProduct() 
        {
            thisProduct = IAPManager.Instance.GetProductInfoById(inAppProductIndex);
            thisButton.onClick.AddListener(OnPurchaseButtonPressed);

            if(!hasInitialized) 
            {
                if(IAPManager.Instance.hasUnityIAPSdkInitialised) 
                {
                    #if HB_UNITYIAP
                    Product product = IAPManager.Instance.GetProductFromSku(thisProduct.productName);

                    if(product != null) {

                        if(txtPrice != null) {
                            txtPrice.text = product.metadata.localizedPriceString;
                        }

                        if(txtTitle != null) {
                            txtTitle.text = product.metadata.localizedTitle;
                        }

                        if(txtDescription != null) {
                            txtDescription.text = product.metadata.localizedDescription;
                        }
                        hasInitialized = true;
                    }
                    #endif
                }
            }
        }

        // Purchase button click listner.
        void OnPurchaseButtonPressed()
        {
            if (buttonType == 0)
            {
                IAPManager.Instance.RestoreAllProducts();
            }
            else
            {
                IAPManager.Instance.PurchaseProduct(thisProduct);
            }
        }
    }
}
