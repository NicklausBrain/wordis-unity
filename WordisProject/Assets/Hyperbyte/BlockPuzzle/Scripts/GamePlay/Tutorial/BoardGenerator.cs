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

namespace Hyperbyte.Tutorial
{
    /// <summary>
    /// This script component will generte the board with given size and will also place blocks from previos session if there is progress.
    /// </summary>
	public class BoardGenerator : MonoBehaviour
    {
        #pragma warning disable 0649
        // Prefab template of block.
        [SerializeField] GameObject blockTemplate;

        // Parent inside which all blocks will be generated. Typically root of block grid.
        [SerializeField] GameObject blockRoot;
        #pragma warning restore 0649

        /// <summary>
        /// Generates the block grid based on game settings and will also set progress from previoius session if any.
        /// </summary>
        public void GenerateBoard()
        {
            BoardSize boardSize = GamePlayUI.Instance.GetBoardSize();

            int rowSize = (int)boardSize;
            int columnSize = (int)boardSize;

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

            GamePlayUI.Instance.gamePlay.allRows = new List<List<Block>>();
            Sprite blockBGSprite = ThemeManager.Instance.GetBlockSpriteWithTag(blockTemplate.GetComponent<Block>().defaultSpriteTag);

            // Iterates through all rows and columns to generate grid.
            for (int row = 0; row < rowSize; row++)
            {
                List<Block> blockRow = new List<Block>();

                for (int column = 0; column < columnSize; column++)
                {
                    // Spawn a block instance and prepares it.
                    RectTransform blockElement = GetBlockInsideGrid();
                    blockElement.localPosition = new Vector3(currentPositionX, currentPositionY, 0);
                    currentPositionX += (blockSize + blockSpace);
                    blockElement.sizeDelta = Vector3.one * blockSize;
                    blockElement.GetComponent<BoxCollider2D>().size = Vector3.one * blockSize;
                    blockElement.GetComponent<Image>().sprite = blockBGSprite;
                    blockElement.name = "block-" + row + "" + column;

                    // Sets blocks logical position inside grid and its default sprite.
                    Block block = blockElement.GetComponent<Block>();
                    block.gameObject.SetActive(true);
                    block.SetBlockLocation(row, column);
                    blockRow.Add(block);
                    block.assignedSpriteTag = block.defaultSpriteTag;
                    block.GetComponent<BoxCollider2D>().enabled = false;
                    block.GetComponent<Image>().color = new Color(1,1,1,0.3F);
                }
                currentPositionX = startPointX;
                currentPositionY -= (blockSize + blockSpace);

                GamePlayUI.Instance.gamePlay.allRows.Add(blockRow);
            }

            GamePlay.Instance.OnBoardGridReady();
        }

        public void UpdateBoardForTutorial(int step) 
        {
            if(step == 1) {
                List<Block> middleColumn = GamePlay.Instance.GetEntirColumn(2);

                foreach(Block block in middleColumn) {
                    block.GetComponent<BoxCollider2D>().enabled = true;
                    block.GetComponent<Image>().color = new Color(1,1,1,1F);
                }
                GamePlayUI.Instance.boardHighlightImage.SetActive(true);
                
            } 
            else if(step == 2)  {
                List<Block> middleColumn = GamePlay.Instance.GetEntirColumn(2);
                
                foreach(Block block in middleColumn) 
                {
                    block.GetComponent<BoxCollider2D>().enabled = true;
                    block.GetComponent<Image>().color = new Color(1,1,1,0.3F);
                    block.GetComponent<BoxCollider2D>().enabled = false;
                }

                List<Block> middleRow = GamePlay.Instance.GetEntireRow(2);

                 foreach(Block block in middleRow) {
                    block.GetComponent<BoxCollider2D>().enabled = true;
                    block.GetComponent<Image>().color = new Color(1,1,1,1F);
                }
                GamePlayUI.Instance.boardHighlightImage.transform.localEulerAngles = new Vector3(0,0,90);
            }
        }

        /// <summary>
        /// Will set block status if there is any from previos session progress.
        /// </summary>
        void SetBlockStatus(Block block, string statusData)
        {
            bool isAvailable = true;
            bool.TryParse(statusData.Split('-')[0], out isAvailable);
            string spriteTag = statusData.Split('-')[1];

            if (!isAvailable)
            {
                block.PlaceBlock(statusData.Split('-')[1]);
            }
        }

        /// <summary>
        /// Horizontal position from where block grid will start.
        /// </summary>
        public float GetStartPointX(float blockSize, int rowSize)
        {
            float totalWidth = (blockSize * rowSize) + ((rowSize - 1) * GamePlayUI.Instance.currentModeSettings.blockSpace);
            return -((totalWidth / 2) - (blockSize / 2));
        }

        /// <summary>
        /// Vertical position from where block grid will start.
        /// </summary>
        public float GetStartPointY(float blockSize, int columnSize)
        {
            float totalHeight = (blockSize * columnSize) + ((columnSize - 1) * GamePlayUI.Instance.currentModeSettings.blockSpace);
            return ((totalHeight / 2) - (blockSize / 2));
        }

        /// <summary>
        /// Spawn a new block instance and sets its block root as its parent.
        /// </summary>
        public RectTransform GetBlockInsideGrid()
        {
            GameObject block = (GameObject)(Instantiate(blockTemplate)) as GameObject;
            block.transform.SetParent(blockRoot.transform);
            block.transform.localScale = Vector3.one;
            return block.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Resets Grid and removes all blocks from it.
        /// </summary>
        public void ResetGame()
        {
            blockRoot.ClearAllChild();
        }
    }
}
