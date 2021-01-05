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
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Hyperbyte.UITween;

namespace Hyperbyte
{
    /// <summary>
    /// This script compont is attached to all the block shape on the game. This script will handles the actual game play and user interaction with the game.
    /// Each block shapes represents a grid format where unrequired blocks will be disabled to form a required shape.
    /// </summary>
    public class BlockShape : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
    {
        #pragma warning disable 0649
        //Row size of block shape grid.
        [SerializeField] int rowSize;

        //Column size of block shape grid.
        [SerializeField] int columnSize;
        #pragma warning restore 0649

        // List of all blocks that are being highlighted. Will keep updating runtime.
        List<Block> highlightingBlocks = new List<Block>();

        // Will set to true after slight time of user touches the block shape.
        bool shouldDrag = false;

        // Cached instance of this block shape.
        Transform thisTransform;

        // Sprite tag represents image sprite on the block shape. You can configure sprite tag from inspector. You can also look UITheme for sprite tag and its associated sprite settings.
        public string spriteTag = "";

        // Type of block shape is adavce or standard. Only used for saving progress and add to required pool in forming level from previous progress.
        public bool isAdvanceShape = false;
        Sprite thisBlockSprite = null;

        // Time untill user can rotate shape on taking pointer up to rotate shape. will work onyl if rotation of shape is allowed.
        float pointerDownTime;

        // List of all active blocks inside block shape.
        List<Transform> activeBlocks = new List<Transform>();

        float dragOffset = 1.0F;

        /// <summary>
        /// Awakes the instance and inintializes and prepare block shape.
        /// </summary>
        private void Awake()
        {
            thisTransform = transform;
            dragOffset = GamePlayUI.Instance.currentModeSettings.shapeDragPositionOffset;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            PrepareBlockShape();
        }

        /// <summary>
        /// Prepared block shape based on the settings user has selecyted in game settings scriptable. Typically handles size of blocks and space inbetween blocks inside block shape.
        /// </summary>
        public void PrepareBlockShape()
        {
            bool doUpdateSprite = false;
            thisBlockSprite = ThemeManager.Instance.GetBlockSpriteWithTag(spriteTag);

            if (thisBlockSprite != null)
            {
                doUpdateSprite = true;
            }

            // Fetched the size of block that should be used.
            float blockSize = GamePlayUI.Instance.currentModeSettings.blockSize;

            // Fetched the space between blocks that should be used.
            float blockSpace = GamePlayUI.Instance.currentModeSettings.blockSpace;

            // Starting points represents point from where block shape grid should start inside block shape.
            float startPointX = GetStartPointX(blockSize, columnSize);
            float startPointY = GetStartPointY(blockSize, rowSize);

            // Will keep updating with iterations.
            float currentPositionX = startPointX;
            float currentPositionY = startPointY;

            int index = 0;

            float blockRotation = (360.0F - transform.localEulerAngles.z);

            for (int row = 0; row < rowSize; row++)
            {
                for (int column = 0; column < columnSize; column++)
                {
                    // Sets the position and  size on block inside block shape.
                    RectTransform blockElement = GetBlockInsideGrid(index);
                    blockElement.localPosition = new Vector3(currentPositionX, currentPositionY, 0);
                    blockElement.localEulerAngles = new Vector3(0,0,blockRotation);
                    
                    currentPositionX += (blockSize + blockSpace);
                    blockElement.sizeDelta = Vector3.one * blockSize;

                    if (doUpdateSprite)
                    {
                        blockElement.GetComponent<Image>().sprite = thisBlockSprite;
                    }
                    index++;
                }
                currentPositionX = startPointX;
                currentPositionY -= (blockSize + blockSpace);
            }

            // Will add all the actibve blocks to list that will be used during gameplay.
            foreach (Transform t in thisTransform)
            {
                if (t.gameObject.activeSelf)
                {
                    activeBlocks.Add(t);
                }
            }
        }

        /// <summary>
        /// Sets tag of sprite to block shape from the settings.
        /// </summary>
        public void SetSpriteTag(string tag)
        {
            spriteTag = tag;
        }

        #region Input Handling
        /// <summary>
        /// Pointer down on block shape.
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                // Checks whether this block shape is touched on pointer down.
                Transform clickedObject = eventData.pointerCurrentRaycast.gameObject.transform;

                if (clickedObject == thisTransform)
                {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);

                    if (!GamePlayUI.Instance.currentModeSettings.allowRotation)
                    {
                        UIFeedback.Instance.PlayBlockShapePickEffect();
                        thisTransform.LocalScale(Vector3.one, 0.05F);
                        thisTransform.Position(new Vector3(pos.x, (pos.y + dragOffset), 0), 0.05F);
                    }
                    else
                    {
                        thisTransform.localScale = Vector3.one;
                        pointerDownTime = Time.time;
                    }
                    // Shape can be dragged now.
                    shouldDrag = true;
                }
            }
        }


        /// <summary>
        /// Begins dragging of block shape.
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (shouldDrag)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
                pos.z = transform.localPosition.z;
                thisTransform.localScale = Vector3.one;
                thisTransform.position = new Vector3(pos.x, (pos.y + dragOffset), 0);
            }
        }

        /// <summary>
        /// Action to taken on pointer up.
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            shouldDrag = false;
            bool canPlaceShape = TryPlacingShape();

            if (canPlaceShape)
            {
                UIFeedback.Instance.PlayBlockShapePlaceEffect();
                GamePlayUI.Instance.OnShapePlaced();
                gameObject.transform.SetParent(null);
                Destroy(gameObject);
            }
            else
            {
                if (!GamePlayUI.Instance.currentModeSettings.allowRotation)
                {
                    // Will reset shape and move to its original position and scale on leaving pointer from block shape.
                    UIFeedback.Instance.PlayBlockShapeResetEffect();
                    ResetShape();
                }
                else
                {
                    // Will check for rotation of shape on releasing finger if rotation of shape is allowed from game settings.
                    CheckForShapeRotation();
                }
            }
        }

        /// <summary>
        /// Will check if shape should be rotated or not on pointer up. Rotating of shape is allowed only for 0.3 seconds after pointer down and if not dragged.
        /// </summary>
        void CheckForShapeRotation()
        {
            float pointerUpTime = Time.time;
            bool isRotationDetected = ((pointerUpTime - pointerDownTime) < 0.3F);

            if (isRotationDetected)
            {
                ResetShapeWithAddRotation();
            }
            else
            {
                ResetShape();
            }
        }

        /// <summary>
        /// Handles block shape dragging event.
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (shouldDrag)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
                pos = new Vector3(pos.x, (pos.y + dragOffset), 0F);
                thisTransform.position = pos;
                CheckCanPlaceShape();
            }
        }
        #endregion
        
        
        /// <summary>
        // Returns the horizontal starting point from where grid should start.
        /// </summary>
        public float GetStartPointX(float blockSize, int rowSize)
        {
            float totalWidth = (blockSize * rowSize) + ((rowSize - 1) * GamePlayUI.Instance.currentModeSettings.blockSpace);
            return -((totalWidth / 2) - (blockSize / 2));
        }

        /// <summary>
        // Returns the vertical starting point from where grid should start.
         /// </summary>
        public float GetStartPointY(float blockSize, int columnSize)
        {
            float totalHeight = (blockSize * columnSize) + ((columnSize - 1) * GamePlayUI.Instance.currentModeSettings.blockSpace);
            return ((totalHeight / 2) - (blockSize / 2));
        }

        /// <summary>
        /// Returns recttransfrom component of the block at the given index. 
        /// </summary>
        public RectTransform GetBlockInsideGrid(int index)
        {
            return thisTransform.GetChild(index).GetComponent<RectTransform>();
        }

        /// <summary>
        /// Checks whether shape can be placed at the current position while dragging it. 
        /// </summary>
        bool CheckCanPlaceShape()
        {
            List<Block> hittingBlocks = new List<Block>();
            List<int> hittingRows = new List<int>();
            List<int> hittingColumns = new List<int>();

            foreach (Transform t in activeBlocks)
            {
                Block hittingBlock = GetHittingBlock(t);
                if (hittingBlock == null || hittingBlocks.Contains(hittingBlock))
                {
                    StopHighlight();
                    GamePlay.Instance.StopHighlight();
                    return false;
                }
                hittingBlocks.Add(hittingBlock);

                // Row Id of block which is interacting with block shape will be added to list. Used to highlight lines that can be completed by placing block shape at current position.
                if (!hittingRows.Contains(hittingBlock.RowId))
                {
                    if (GamePlay.Instance.CanHighlightRow(hittingBlock.RowId))
                    {
                        hittingRows.Add(hittingBlock.RowId);
                    }
                }

                // Column Id of block which is interacting with block shape will be added to list. Used to highlight lines that can be completed by placing block shape at current position.
                if (!hittingColumns.Contains(hittingBlock.ColumnId))
                {
                    if (GamePlay.Instance.CanHighlightColumn(hittingBlock.ColumnId))
                    {
                        hittingColumns.Add(hittingBlock.ColumnId);
                    }
                }

                // Will be called when user ends touch/mouse from the block shape and block shape will try to place on grid. Will go back to original if block shape cann not be placed.
                if (hittingBlocks.Count == activeBlocks.Count)
                {
                    foreach (Block block in hittingBlocks)
                    {
                        block.Highlight(thisBlockSprite);
                    }
                    GamePlay.Instance.HighlightAllRows(hittingRows, thisBlockSprite);
                    GamePlay.Instance.HighlightAllColmns(hittingColumns, thisBlockSprite);

                    StopHighlight(hittingBlocks);

                    // Will stop highlight all rows and columns except for given. 
                    GamePlay.Instance.StopHighlight(hittingRows, hittingColumns);

                    highlightingBlocks.AddRange(hittingBlocks);
                    GamePlay.Instance.highlightingRows.AddRange(hittingRows);
                    GamePlay.Instance.highlightingColumns.AddRange(hittingColumns);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Will be called when user ends touch/mouse from the block shape and block shape will try to place on grid. Will go back to original if block shape cann not be placed.
        /// </summary>
        bool TryPlacingShape()
        {
            List<Block> hittingBlocks = new List<Block>();
            List<int> completedRows = new List<int>();
            List<int> completedColumns = new List<int>();

            foreach (Transform t in activeBlocks)
            {
                Block hittingBlock = GetHittingBlock(t);
                if (hittingBlock == null || hittingBlocks.Contains(hittingBlock))
                {
                    StopHighlight();
                    GamePlay.Instance.StopHighlight();
                    return false;
                }
                hittingBlocks.Add(hittingBlock);

                // Row id of block will be added to list if entire row is goint to finish on placing current shape.
                if (!completedRows.Contains(hittingBlock.RowId))
                {
                    if (GamePlay.Instance.IsRowCompleted(hittingBlock.RowId))
                    {
                        completedRows.Add(hittingBlock.RowId);
                    }
                }

                // Column id of block will be added to list if entire column is goint to finish on placing current shape.
                if (!completedColumns.Contains(hittingBlock.ColumnId))
                {
                    if (GamePlay.Instance.IsColumnCompleted(hittingBlock.ColumnId))
                    {
                        completedColumns.Add(hittingBlock.ColumnId);
                    }
                }

                // Amount of blocks on grid that are interacting with block shape should be excact to amount of active blocks in the the blockshape. All the hitting blocks should be unique.
                if (hittingBlocks.Count == activeBlocks.Count)
                {
                    foreach (Block block in hittingBlocks)
                    {
                        block.PlaceBlock(thisBlockSprite, spriteTag);
                    }

                    // Will clear all rows that completed by placing current shape.
                    if (completedRows.Count > 0)
                    {
                        GamePlay.Instance.ClearRows(completedRows);
                    }

                    // Will clear all columns that completed by placing current shape.
                    if (completedColumns.Count > 0)
                    {
                        GamePlay.Instance.ClearColumns(completedColumns);
                    }

                    int linesCleared = (completedRows.Count + completedColumns.Count);
                    // Adds score based on the number of rows, columnd and blocks cleares. final calculation will be done in score manager.
                    GamePlayUI.Instance.scoreManager.AddScore(linesCleared, activeBlocks.Count);

                    if(linesCleared > 0) {
                        AudioController.Instance.PlayLineBreakSound(completedRows.Count + completedColumns.Count);
                    }

                    #region TimeMode Specific
                    if (GamePlayUI.Instance.currentGameMode == GameMode.Timed)
                    {
                        // Will add line completion bonus time to timer.
                        GamePlayUI.Instance.timeModeProgresssBar.AddTime((GamePlayUI.Instance.timeModeAddSecondsOnLineBreak * (completedRows.Count + completedColumns.Count)));
                    }
                    #endregion
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///  Returns block that is interecting with current block shape. Returns null if not any.
        /// </summary>
        Block GetHittingBlock(Transform draggingBlock)
        {
            RaycastHit2D hit = Physics2D.Raycast(draggingBlock.position, Vector2.zero, 1);
            if (hit.collider != null && hit.collider.GetComponent<Block>() != null)
            {
                return hit.collider.GetComponent<Block>();
            }
            return null;
        }

        /// <summary>
        /// Stops highlighting all blocks from highlightingBlocks list except for given blocks.
        /// </summary>
        void StopHighlight(List<Block> excludingList)
        {
            foreach (Block b in highlightingBlocks)
            {
                if (!excludingList.Contains(b))
                {
                    b.Reset();
                }
            }
            highlightingBlocks.Clear();
        }

        /// <summary>
        /// Stops highlighting all blocks.
        /// </summary>
        void StopHighlight()
        {
            foreach (Block b in highlightingBlocks)
            {
                b.Reset();
            }
            highlightingBlocks.Clear();
        }

        /// <summary>
        /// Reset shape and will move it to its original position. Typically called when it fails to place on grid.
        /// </summary>
        void ResetShape()
        {
            thisTransform.LocalPosition(Vector3.zero, 0.25F);
            thisTransform.LocalScale((Vector3.one * GamePlayUI.Instance.currentModeSettings.shapeInactiveSize), 0.25F);
        }

        /// <summary>
        /// Adds rotation to block shape at its original position.
        /// </summary>
        void ResetShapeWithAddRotation()
        {
            float newRotation = (transform.localEulerAngles.z - 90);
            InputManager.Instance.DisableTouchForDelay(0.2F);
            transform.LocalRotationToZ(newRotation, 0.2F).OnComplete(() => {
                ResetShape();
            });
        }
    }
}
