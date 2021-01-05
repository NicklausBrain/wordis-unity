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

namespace Hyperbyte
{
    public static class UIExtentions
    {
        // Clear all child gameobjects of the given gameobject.
        public static void ClearAllChild(this GameObject obj)
        {
            if (obj.transform.childCount > 0)
            {
                foreach (Transform child in obj.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

		// Clear all child gameobjects of the given transform.
        public static void ClearAllChild(this Transform obj)
        {
            if (obj.childCount > 0)
            {
                foreach (Transform child in obj)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

		// Clear all child gameobjects of the given rect transform.
        public static void ClearAllChild(this RectTransform obj)
        {
            if (obj.childCount > 0)
            {
                foreach (Transform child in obj)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        // Activates the given gameobject with animation. used for only popups of the game.
        public static void Activate(this GameObject target, bool addToStack = true)
        {
            target.gameObject.SetActive(true);
            target.transform.SetAsLastSibling();

            if(addToStack) {
                UIController.Instance.Push(target.name);
            }
        }

        // Deactivates the game object with animation.
        public static void Deactivate(this GameObject target)
        {
            Animator animator = target.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Close");
                UIController.Instance.StartCoroutine(DisableWindow(target));
            }
            else
            {
                target.SetActive(false);
                UIController.Instance.Pop(target.name);
            }
        }

		/// <summary>
		/// Disable given gameobject and removes it from stack added to any.
		/// </summary>
        static IEnumerator DisableWindow(GameObject target)
        {
            yield return new WaitForSeconds(0.3F);
            target.SetActive(false);
            UIController.Instance.Pop(target.name);
        }
    }
}
