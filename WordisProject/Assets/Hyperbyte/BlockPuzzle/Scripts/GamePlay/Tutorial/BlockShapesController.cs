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
using Hyperbyte.Utils;
using Hyperbyte.UITween;

namespace Hyperbyte.Tutorial
{
    /// <summary>
    /// This script controlls the block shapes that being place/played on board grid. It controlls spawning of block shapes and organizing it.
    /// </summary>
    public class BlockShapesController : MonoBehaviour
    {
        #pragma warning disable 0649
        // All The Block shape containers are added via inspector. Typically used 3 in block puzzle games.
        [SerializeField] List<ShapeContainer> allShapeContainers;
        #pragma warning restore 0649

        public List<BlockShape> tutorialBlockShapes_1 = new List<BlockShape>();
        public List<BlockShape> tutorialBlockShapes_2 = new List<BlockShape>();

        // Instance of block shape placement checker script component to check if given block shape can be placed on board grid or not.
        [System.NonSerialized] public BlockShapePlacementChecker blockShapePlacementChecker;

        // Pool of all block shape prepared with probability. Will stay unchanged during gameplay.
        List<GameObject> blockShapesPool = new List<GameObject>();

        // Upcoming block shapes pool copies elements from blockShapPool and keeps updating with game progress.
        List<GameObject> upcomingBlockShapes = new List<GameObject>();

        // Size of block shape when its inactive and inside block shape container.
        Vector3 shapeInactiveScale = Vector3.one;

        int totalShapesPlaced = 0;

        int currentTutorialStep = 1;

        /// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
        private void Awake() {
            blockShapePlacementChecker = GetComponent<BlockShapePlacementChecker>();
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            ///  Registers game status callbacks.
            GamePlayUI.OnShapePlacedEvent += GamePlayUI_OnShapePlacedEvent;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            /// Unregisters game status callbacks.
            GamePlayUI.OnShapePlacedEvent -= GamePlayUI_OnShapePlacedEvent;
        }

        /// <summary>
        /// Prepares all block shapes based on gameplay settings.
        /// </summary>
        public void PrepareShapeContainer(int tutorialStep)
        {
            currentTutorialStep = tutorialStep;
            shapeInactiveScale = Vector3.one * GamePlayUI.Instance.currentModeSettings.shapeInactiveSize;
            PrepareShapePool();
            PrepareUpcomingShapes();
            FillAllShapeContainers();
        }

        /// <summary>
        /// Prepares a block shape pool with given probability amount as logical blocks shape references based on gameplay settings.
        /// </summary>
        void PrepareShapePool()
        {
            if(currentTutorialStep == 1) 
            {
                blockShapesPool = new List<GameObject>();
                // Gets all the standard shapes added in gameplay settings.
                foreach(BlockShape blockShape in tutorialBlockShapes_1) {
                    blockShapesPool.Add(blockShape.gameObject);
                }
                Invoke("ShowDragTip",1F);
            }

            if(currentTutorialStep == 2) 
            {
                blockShapesPool.Clear();
                upcomingBlockShapes.Clear();
                ClearAllShapeContainers();

                // Gets all the standard shapes added in gameplay settings.
                foreach(BlockShape blockShape in tutorialBlockShapes_1) {
                    blockShapesPool.Add(blockShape.gameObject);
                }
            }    
        }

        void ShowDragTip() {
            GamePlayUI.Instance.shapeDragHandImage.SetActive(true);
        }

        /// <summary>
        /// Adds a block shape on all the block containers.
        /// </summary>
        void FillAllShapeContainers()
        {
            foreach (ShapeContainer shapeContainer in allShapeContainers) {
                FillShapeInContainer(shapeContainer);
            }
        }

        /// <summary>
        /// Adds a block shape in the given shape container with animation effect.
        /// </summary>
        void FillShapeInContainer(ShapeContainer shapeContainer)
        {
            if (shapeContainer.blockShape == null)
            {
                BlockShape blockShape = GetBlockShape();
                blockShape.transform.SetParent(shapeContainer.blockParent);
                shapeContainer.blockShape = blockShape;
                blockShape.transform.localScale = shapeInactiveScale;
                blockShape.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero.WithNewX(1500);
                blockShape.GetComponent<RectTransform>().AnchorX(0, 0.5F).SetDelay(0.3F).SetEase(Ease.EaseOut);
            }
        }

        /// <summary>
        /// Adds a block shape to last shape container. Typically will be called when gameplay setting have always keep all shapes filled.
        /// Upon placing a shape, all shapes will reorder and last shape container needs to add new block shape.
        /// </summary>
        void FillLastShapeContainer()
        {
            ShapeContainer shapeContainer = allShapeContainers[allShapeContainers.Count - 1];

            // Only add shape container is empty.
            if (shapeContainer.blockShape == null)
            {
                BlockShape blockShape = GetBlockShape();
                blockShape.transform.SetParent(shapeContainer.blockParent);
                shapeContainer.blockShape = blockShape;
                blockShape.transform.localScale = shapeInactiveScale;
                blockShape.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero.WithNewX(300);
                blockShape.GetComponent<RectTransform>().AnchorX(0, 0.3F).SetDelay(0.1F).SetEase(Ease.EaseOut);
            }
        }

        /// <summary>
        /// Gets a block shape from the upcoming block shape pool. will handle empty state and fill upcoming shapes pool if detected empty.
        /// </summary>
        BlockShape GetBlockShape()
        {
            if (upcomingBlockShapes.Count <= 0)
            {
                PrepareUpcomingShapes();
            }

            // Takes a block shape instance from pool and instantiates it.
            GameObject upcomingShape = (GameObject)Instantiate(upcomingBlockShapes[0]);
            upcomingShape.name = upcomingShape.name.Replace("(Clone)", "");

            if(currentTutorialStep > 1) {
               upcomingShape.transform.localEulerAngles = new Vector3(0,0,90);
            }

            // Removes shapes from pool.
            upcomingBlockShapes.RemoveAt(0);
            return upcomingShape.GetComponent<BlockShape>();
        }

        /// <summary>
        /// Prepares a pool of upcoming shapes.
        /// </summary>
        void PrepareUpcomingShapes()
        {
            upcomingBlockShapes.AddRange(blockShapesPool);
            // upcomingBlockShapes.Shuffle();
        }

        /// <summary>
        /// Checks status of all block shape containers and fill or reorder based on gameplay settings and status.
        /// </summary>
        public void UpdateShapeContainers()
        {
            if (IsAllShapeContainerEmpty())
            {
                FillAllShapeContainers();
            }
            else
            {
                if (GamePlayUI.Instance.currentModeSettings.alwaysKeepFilled)
                {
                    ReorderShapeContainer();
                }
            }

            Invoke("CheckAllShapesCanbePlaced", 0.5F);
        }

        /// <summary>
        /// Reorders all shape containers after any block shape placed on the board.
        /// </summary>
        void ReorderShapeContainer()
        {
            ShapeContainer emptyShapeContainer = null;
            int emptyBlockIndex = 0;

            for (int i = 0; i < allShapeContainers.Count; i++)
            {

                if (allShapeContainers[i].blockShape != null)
                {
                    if (emptyShapeContainer != null)
                    {
                        BlockShape blockShape = allShapeContainers[i].blockShape;
                        blockShape.transform.SetParent(emptyShapeContainer.blockParent);
                        emptyShapeContainer.blockShape = blockShape;
                        allShapeContainers[i].blockShape = null;
                        blockShape.GetComponent<RectTransform>().AnchorX(0, 0.3F).SetDelay(0.1F).SetEase(Ease.EaseOut);
                        i = emptyBlockIndex;
                        emptyShapeContainer = null;
                    }
                }
                else
                {
                    if (emptyShapeContainer == null)
                    {
                        emptyShapeContainer = allShapeContainers[i];
                        emptyBlockIndex = i;
                    }
                }
            }
            FillLastShapeContainer();
        }

        /// <summary>
        /// Whether all block shape containers are empty or not.
        /// </summary>
        bool IsAllShapeContainerEmpty()
        {
            foreach (ShapeContainer rect in allShapeContainers)
            {
                if (rect.blockShape != null)
                {
                    return false;
                }
            }
            return true;
        }

        #region Registered Events Callback
        /// <summary>
        /// Callback when any block shape places on board.
        /// </summary>
        private void GamePlayUI_OnShapePlacedEvent()
        {
            Invoke("UpdateShapeContainers", 0.1F);
            totalShapesPlaced += 1;
        }
        #endregion

         /// <summary>
        /// Checks if any block shape from all containers can be placed on board. Game will go to rescue or gameover state upon returning false.
        /// </summary>
        void CheckAllShapesCanbePlaced()
        {
            bool canAnyShapePlaced = CheckBlockShapeCanPlaced();

            if (!canAnyShapePlaced)
            {
            }
        }

        /// <summary>
        /// Checks if any block shape from all containers can be placed on board. Shapes that can't placed will have lesser opacity.
        /// </summary>
        public bool CheckBlockShapeCanPlaced()
        {
            bool canAnyShapePlaced = false;
            foreach (ShapeContainer shapeContainer in allShapeContainers)
            {
                if (shapeContainer.blockShape != null)
                {
                    bool shapeCanbePlaced = blockShapePlacementChecker.CheckShapeCanbePlaced(shapeContainer.blockShape);
                    shapeContainer.blockShape.GetComponent<CanvasGroup>().alpha = (shapeCanbePlaced) ? 1F : 0.5F;

                    if (shapeCanbePlaced)
                    {
                        canAnyShapePlaced = true;
                    }
                }
            }
            return canAnyShapePlaced;
        }

        /// <summary>
        /// Returns status of all block shape containers. Typically called when saving board progress.
        /// </summary>
        public ShapeInfo[] GetCurrentShapesInfo()
        {
            ShapeInfo[] allShapesInfo = new ShapeInfo[allShapeContainers.Count];
            int shapeIndex = 0;

            foreach (ShapeContainer shapeContainer in allShapeContainers)
            {
                if (shapeContainer.blockShape != null)
                {
                    bool isAdvanceShape = shapeContainer.blockShape.isAdvanceShape;
                    allShapesInfo[shapeIndex] = new ShapeInfo(shapeContainer.blockShape.isAdvanceShape, shapeContainer.blockShape.name, shapeContainer.blockShape.transform.localEulerAngles.z);
                }
                else
                {
                    allShapesInfo[shapeIndex] = new ShapeInfo(false, null, 0);
                }
                shapeIndex++;
            }
            return allShapesInfo;
        }

        /// <summary>
        /// Returns total nuber of block shapes placed during gameplay.
        /// </summary>
        public int GetTotalShapesPlaced()
        {
            return totalShapesPlaced;
        }

        /// <summary>
        /// Resets all block shape containers.
        /// </summary>
        public void ResetGame()
        {
            upcomingBlockShapes.Clear();
            ClearAllShapeContainers();
        }

        void ClearAllShapeContainers() {
            foreach (ShapeContainer shapeContainer in allShapeContainers)
            {
                shapeContainer.Reset();
            }
        }
    }
}