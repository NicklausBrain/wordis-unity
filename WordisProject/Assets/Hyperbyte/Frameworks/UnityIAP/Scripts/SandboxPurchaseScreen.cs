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

namespace Hyperbyte
{
    /// <summary>
    /// Sandbox purchase will be used while Unity IAP SDK setup is not completed.
    /// </summary>
    public class SandboxPurchaseScreen : MonoBehaviour
    {
        public Text txtProductDescription;
        ProductInfo currentProduct = null;

        /// <summary>
        /// Initializes the purchase request.
        /// </summary>
        public void InitialiseSandboxPurchase(ProductInfo productInfo)
        {
            txtProductDescription.text = "Purchasing " + productInfo.productName;
            currentProduct = productInfo;
        }

        /// <summary>
        /// Close button listener.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Returns purchase success responce.
        /// </summary>
        public void SendSuccessResponce()
        {
            IAPManager.Instance.OnSandboxPurchaseSuccess(currentProduct);
            Destroy(gameObject);
        }

        /// <summary>
        /// Returns purchase fail responce.
        /// </summary>
        public void SendFailureResponce()
        {
            IAPManager.Instance.OnSandboxPurchaseFailure();
            Destroy(gameObject);
        }
    }
}