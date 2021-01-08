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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Wordis.BlockPuzzle.Scripts.GamePlay.Tutorial
{
    /// <summary>
    /// This script typically checks whether the given block shape can be place on board or not. This script generates a clone of block space 
    /// and tries to place at all the block positions of the block grid to determine it can be placed or. Upon returning the result clone of block 
    /// shape will be destroyed.
    /// </summary>
    public class BlockShapePlacementChecker : MonoBehaviour
    {
        List<Transform> _activeBlocks = new List<Transform>();
        Transform _shapeClone = null;

        /// <summary>
        /// Checks whether the given block shape can be place on board.
        /// </summary>
        public bool CheckShapeCanBePlaced(BlockShape blockShape)
        {
            _activeBlocks = new List<Transform>();
            _shapeClone =
                ((GameObject)Instantiate(blockShape.gameObject, Vector3.zero, Quaternion.identity, transform)).transform;
            _shapeClone.localScale = Vector3.one;
            _shapeClone.localEulerAngles = blockShape.transform.localEulerAngles;

            foreach (Transform t in _shapeClone)
            {
                if (t.gameObject.activeSelf)
                {
                    _activeBlocks.Add(t);
                    t.GetComponent<Image>().enabled = false;
                }
            }

            // Tries to place shape will rotations if rotation is allowed. Will Iterate through all the blocks inside the grid.
            if (GamePlayUI.Instance.currentModeSettings.allowRotation)
            {
                for (int i = 0; i < 4; i++)
                {
                    _shapeClone.transform.localEulerAngles = new Vector3(0, 0, 90 * i);
                    foreach (List<Block> blockRow in GamePlay.Instance.allRows)
                    {
                        foreach (Block b in blockRow)
                        {
                            if (b.isAvailable)
                            {
                                bool result = CheckCanPlaceShapeAtPosition(b.transform.position);
                                if (result)
                                {
                                    Destroy(_shapeClone.gameObject);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (List<Block> blockRow in GamePlay.Instance.allRows)
                {
                    foreach (Block b in blockRow)
                    {
                        if (b.isAvailable)
                        {
                            bool result = CheckCanPlaceShapeAtPosition(b.transform.position);
                            if (result)
                            {
                                Destroy(_shapeClone.gameObject);
                                return true;
                            }
                        }
                    }
                }
            }

            if (_shapeClone != null)
            {
                Destroy(_shapeClone.gameObject);
            }

            return false;
        }

        /// <summary>
        /// Checks whether shape can be placed at the current position. 
        /// </summary>
        bool CheckCanPlaceShapeAtPosition(Vector3 blockPosition)
        {
            Transform firstPoint = _activeBlocks[0];
            Vector3 positionOffset = _shapeClone.position - firstPoint.position;
            _shapeClone.position = blockPosition + positionOffset;

            List<Block> hittingBlocks = new List<Block>();

            foreach (Transform t in _activeBlocks)
            {
                Block hittingBlock = GetHittingBlock(t);
                if (hittingBlock == null || hittingBlocks.Contains(hittingBlock))
                {
                    return false;
                }

                hittingBlocks.Add(hittingBlock);

                if (hittingBlocks.Count == _activeBlocks.Count)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///  Returns block that is intersecting with current block shape. Returns null if not any.
        /// </summary>
        private Block GetHittingBlock(Transform draggingBlock)
        {
            RaycastHit2D hit = Physics2D.Raycast(draggingBlock.position, Vector2.zero, 1);
            if (hit.collider != null && hit.collider.GetComponent<Block>() != null)
            {
                return hit.collider.GetComponent<Block>();
            }

            return null;
        }
    }
}