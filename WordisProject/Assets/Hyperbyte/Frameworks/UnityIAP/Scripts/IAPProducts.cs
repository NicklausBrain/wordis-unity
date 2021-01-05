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

/// <summary>
/// This scriptable class instance contains list of all in game iap skus. This can be
/// configured from Hyperbyte -> Unity IAP Settings menu item.
/// </summary>
public class IAPProducts : ScriptableObject
{
    public ProductInfo[] productInfos;
}

[System.Serializable]
public class ProductInfo
{
	// ID of product.
    public string productName;

	// If store sku is different then product name then enable override.
    public bool overrideStoreIds = false;

	// iOS Sku.
    public string iOSSku;

	// Google Sku.
    public string googleSku;

	// Amazon Sku.
    public string amazonSku;

	// Samsung Sku.
    public string samsungSku;

	// Product type : CONSUMABLE, NON CONSUMABLE, SUBSCRIPTION
    public int productType;

	// Type of reward.
    public int rewardType;
    
	// Amount of reward if reward type is GEMS.
	public int rewardAmount;
}

// Type of reward associated with product.
public enum RewardType
{
    REMOVE_ADS,
    GEMS,
    OTHER
}
