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
using UnityEngine;

namespace Hyperbyte.Tutorial
{
    public class GamePlay : Singleton<GamePlay>
    {
        [Header("Public Class Members")]
        [Tooltip("BoardGenerator Script Reference")]
        public BoardGenerator boardGenerator;

        [Tooltip("BlockShapesController Script Reference")]
        public BlockShapesController blockShapeController;

        //List of all Blocks in Row X Column format.
        [System.NonSerialized] public List<List<Block>> allRows = new List<List<Block>>();
        [System.NonSerialized] public List<List<Block>> allColumns = new List<List<Block>>();

        //List of rows highlight while dragging shape. Will keep updating runtime. 
        [System.NonSerialized] public List<int> highlightingRows = new List<int>();

        //List of columns highlight while dragging shape. Will keep updating runtime. 
        [System.NonSerialized] public List<int> highlightingColumns = new List<int>();

        // Saves highlighting rows as cached to reduce iterations . Will keep updating runtime. 
        List<int> cachedHighlightingRows = new List<int>();

        // Saves highlighting columns as cached to reduce iterations . Will keep updating runtime. 
        List<int> cachedHighlightingColumns = new List<int>();

        /// <summary>
        /// Will get called when board grid gets initialized.
        /// </summary>
        public void OnBoardGridReady()
        {
            int totalRows = allRows.Count;

            for(int rowId = 0; rowId < allRows[0].Count; rowId++) {
                 List<Block> thisColumn = new List<Block>();
                 for (int columnId = 0; columnId < totalRows; columnId++) { 
                     thisColumn.Add(allRows[columnId][rowId]);
                 }
                 allColumns.Add(thisColumn);
            }
        }

        /// <summary>
        /// Clears all given rows from the grid.
        /// </summary>
        public void ClearRows(List<int> rowIds)
        {
            foreach (int rowId in rowIds)
            {
                StartCoroutine(ClearAllBlocks(GetEntireRow(rowId)));
            }
        }

        /// <summary>
        /// Clears all given columns from the grid.
        /// </summary>
        public void ClearColumns(List<int> columnIds)
        {
            foreach (int columnId in columnIds)
            {
                StartCoroutine(ClearAllBlocks(GetEntirColumn(columnId)));
            }
        }

        /// <summary>
        /// Clears all given blocks from the board. On Completion state of block will be empty.
        /// </summary>
        IEnumerator ClearAllBlocks(List<Block> allBlocks)
        {
            //Below calculation is done so blocks starts clearing from center to end on both sides.
            int middleIndex = (allBlocks.Count % 2 == 0) ? (allBlocks.Count / 2) : ((allBlocks.Count / 2) + 1);
            int leftIndex = (middleIndex - 1);
            int rightIndex = middleIndex;
            int totalBlocks = allBlocks.Count;

            for (int i = 0; i < middleIndex; i++, leftIndex--, rightIndex++)
            {
                if (leftIndex >= 0)
                {
                    allBlocks[leftIndex].Clear();
                }
                if (rightIndex < totalBlocks)
                {
                    allBlocks[rightIndex].Clear();
                }
                yield return new WaitForSeconds(0.03F);
            }
            yield return new WaitForSeconds(1);
            GamePlayUI.Instance.TutorialStepCompleted();
        }

        /// <summary>
        /// Returns all blocks from the given row.
        /// </summary>
        public List<Block> GetEntireRow(int rowId) {
            return allRows[rowId];
        }

        /// <summary>
        /// Returns all blocks from the given column.
        /// </summary>
        public List<Block> GetEntirColumn(int columnId)
        {
            return allColumns[columnId];
        }

        /// <summary>
        /// Returns true if row is about to complete on current block shape being placed otherwise false.
        /// </summary>
        public bool CanHighlightRow(int rowId) {
            return allRows[rowId].Find (o => o.isFilled == false) == null;
        }

        /// <summary>
        /// Returns true if given row if all blocks in given row are filled. Otherwise false.
        /// </summary>
        public bool IsRowCompleted(int rowId) {
             return allRows[rowId].Find (o => o.isFilled == false) == null;
        }

        /// <summary>
        /// Returns true if column is about to complete on current block shape being placed otherwise false.
        /// </summary>
        public bool CanHighlightColumn(int columnId)
        {
             return allColumns[columnId].Find (o => o.isFilled == false) == null;
        }

        /// <summary>
        /// Returns true if given column if all blocks in given row are filled. Otherwise false.
        /// </summary>
        public bool IsColumnCompleted(int columnId)
        {
            return allColumns[columnId].Find (o => o.isFilled == false) == null;
        }

        /// <summary>
        /// Highlights all block of from the row with given sprite.
        /// </summary>
        void HighlightRow(int rowId, Sprite sprite)
        {
            if (!cachedHighlightingRows.Contains(rowId))
            {
                for (int columnId = 0; columnId < allRows.Count; columnId++)
                {
                    allRows[rowId][columnId].Highlight(sprite);
                }
                cachedHighlightingRows.Add(rowId);
            }
        }

        /// <summary>
        /// Highlights all block of from the column with given sprite.
        /// </summary>
        void HighlightColumn(int columnId, Sprite sprite)
        {
            if (!cachedHighlightingColumns.Contains(columnId))
            {
                foreach(Block block in GetEntirColumn(columnId)) {
                    block.Highlight(sprite);
                }
                cachedHighlightingColumns.Add(columnId);
            }
        }

        /// <summary>
        /// Highlights all rows with given sprite.
        /// </summary>
        public void HighlightAllRows(List<int> hittingRows, Sprite sprite)
        {
            foreach (int row in hittingRows)
            {
                HighlightRow(row, sprite);
            }
        }

        /// <summary>
        /// Highlights all columns with given sprite.
        /// </summary>
        public void HighlightAllColmns(List<int> hittingColumns, Sprite sprite)
        {
            foreach (int column in hittingColumns)
            {
                HighlightColumn(column, sprite);
            }
        }

        /// <summary>
        /// Stops highlighting all rows and all columns that is being highlighted.
        /// </summary>
        public void StopHighlight()
        {
            foreach (int row in highlightingRows)
            {
                StopHighlightingRow(row);
            }

            foreach (int column in highlightingColumns)
            {
                StopHighlightingColumn(column);
            }

            highlightingRows.Clear();
            highlightingColumns.Clear();
        }

        /// <summary>
        /// Stops highlighting all rows and all columns that is being highlighted except for given rows and column ids.
        /// </summary>
        public void StopHighlight(List<int> excludingRows, List<int> excludingColumns)
        {
            foreach (int row in highlightingRows)
            {
                if (!excludingRows.Contains(row))
                {
                    StopHighlightingRow(row);
                }
            }

            foreach (int column in highlightingColumns)
            {
                if (!excludingColumns.Contains(column))
                {
                    StopHighlightingColumn(column);
                }
            }
            highlightingRows.Clear();
            highlightingColumns.Clear();
        }

        /// <summary>
        /// Stops highlighting the given row.
        /// </summary>
        void StopHighlightingRow(int rowId)
        {
            foreach(Block block in GetEntireRow(rowId)) {
                block.Reset();
            }

            if (cachedHighlightingRows.Contains(rowId)) {
                cachedHighlightingRows.Remove(rowId);
            }
        }

        /// <summary>
        /// Stops highlighting the given column.
        /// </summary>
        void StopHighlightingColumn(int columnId)
        {
            foreach(Block block in GetEntirColumn(columnId)) {
                 block.Reset();
            }
            if (cachedHighlightingColumns.Contains(columnId)) {
                cachedHighlightingColumns.Remove(columnId);
            }
        }
    }
}
