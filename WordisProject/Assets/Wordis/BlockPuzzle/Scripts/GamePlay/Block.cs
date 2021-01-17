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

using Assets.Wordis.BlockPuzzle.Scripts.Controller;
using Assets.Wordis.Frameworks.ThemeManager.Scripts;
using Assets.Wordis.Frameworks.UITween.Scripts.Utils;
using Assets.Wordis.Frameworks.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Wordis.BlockPuzzle.Scripts.GamePlay
{
    /// <summary>
    /// This class component is attached to all blocks in the grid.
    /// This is presentation level component.
    /// </summary>
    public class Block : MonoBehaviour
    {
        #region Wordis

        public static string DefaultCharTag => "b1"; // nice blue;

        public static string WaterTag => "b4"; // azure (?);

        #endregion


        // Returns rowId 
        public int RowId { get; private set; }

        //Returns columnId
        public int ColumnId { get; private set; }

        // Block is filled  with current playing block shape.
        [System.NonSerialized] public bool isFilled = false;

        // Block is available to place block shape or not.
        [System.NonSerialized] public bool isAvailable = true;

        // Default sprite tag on the block. Will update runtime.
        public string defaultSpriteTag;

        // Sprite that is assigned on the block. Will update runtime.
        public string assignedSpriteTag;

        //Default sprite that is assigned to block.
        Sprite defaultSprite;

        // Box collide attached to this block.
        BoxCollider2D thisCollider;

#pragma warning disable 0649
        // Image component on the block. Assigned from Inspector.
        [SerializeField] Image blockImage;
#pragma warning restore 0649

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Initializes the collider component on Awake.
            thisCollider = GetComponent<BoxCollider2D>();
        }

        /// <summary>
        /// Assignes logical position on block on the grid.
        /// TODO: seems like should be make in constructor and should not be change ever
        /// </summary>
        public void SetBlockLocation(int rowIndex, int columnIndex)
        {
            RowId = rowIndex;
            ColumnId = columnIndex;
        }

        /// <summary>
        /// Highlights block with given sprite.
        /// </summary>
        public void Highlight(Sprite sprite)
        {
            blockImage.sprite = sprite;
            blockImage.enabled = true;
            isFilled = true;
        }

        /// <summary>
        /// Resets block to its default state.
        /// </summary>
        public void Reset()
        {
            if (!isAvailable)
            {
                blockImage.sprite = defaultSprite;
                isFilled = true;
            }
            else
            {
                blockImage.enabled = false;
                isFilled = false;
            }
        }

        /// <summary>
        /// Places block from the block shape. Typically will be called during gameplay.
        /// </summary>
        public void PlaceBlock(Sprite sprite, string spriteTag)
        {
            thisCollider.enabled = false;
            blockImage.enabled = true;
            blockImage.sprite = sprite;
            blockImage.color = blockImage.color.WithNewA(1);
            defaultSprite = sprite;
            isFilled = true;
            isAvailable = false;
            assignedSpriteTag = spriteTag;
        }

        /// <summary>
        /// Places block from the block shape.
        /// Typically will be called when game starting with progress from previous session.
        /// </summary>
        public void PlaceBlock(string spriteTag)
        {
            Sprite sprite = ThemeManager.Instance.GetBlockSpriteWithTag(spriteTag);
            thisCollider.enabled = false;
            blockImage.enabled = true;
            blockImage.sprite = sprite;
            blockImage.color = blockImage.color.WithNewA(1);
            defaultSprite = sprite;
            isFilled = true;
            isAvailable = false;
            assignedSpriteTag = spriteTag;
        }

        /// <summary>
        /// Clears block. Will be called when line containing this block will get completed. This is typical animation effect of how completed block shoudl disappear.
        /// </summary>
        public void Clear()
        {
            transform.GetComponent<Image>().color = new Color(1, 1, 1, 0);

            // BlockImage will scale down to 0 in 0.35 seconds. and will reset to scale 1 on animation completion.
            UIFeedback.Instance.PlayHapticLight();
            blockImage.transform.LocalScale(Vector3.zero, 0.35F).OnComplete(() =>
            {
                blockImage.transform.localScale = Vector3.one;
                blockImage.sprite = null;
            });

            blockImage.transform.LocalRotationToZ(90, 0.2F).OnComplete(() =>
            {
                blockImage.transform.localEulerAngles = Vector3.zero;
            });

            transform.GetComponent<Image>().SetAlpha(1, 0.35F).SetDelay(0.3F);

            // Opacity of block image will set to 0 in 0.3 seconds. and will reset to 1 on animation completion.
            blockImage.SetAlpha(0.5F, 0.3F).OnComplete(() => { blockImage.enabled = false; });

            isFilled = false;
            isAvailable = true;
            thisCollider.enabled = true;
            assignedSpriteTag = defaultSpriteTag;
            GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
        }

        public void Fade()
        {
            // todo: to be done
        }
    }
}