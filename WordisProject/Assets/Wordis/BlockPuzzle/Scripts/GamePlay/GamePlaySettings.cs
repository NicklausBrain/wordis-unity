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

namespace Assets.Wordis.BlockPuzzle.Scripts.GamePlay
{
    // Game Modes.
    public enum GameMode
    {
        Tutorial,
        Classic,
        Timed,
        Advance
    }

    /// <summary>
    /// This scriptable object instance contains all the info regarding the game setting for each game mode.static
    /// This settings can be updated from Hyperbyte -> Gameplay Setting menu item.
    /// </summary>
    public class GamePlaySettings : ScriptableObject
    {
        // Classic mode settings.
        public GameModeSettings tutorialModeSettings;

        // Classic mode settings.
        public GameModeSettings classicModeSettings;

        // Time mode settings.
        public GameModeSettings timeModeSettings;

        // Advance mode settings.
        public GameModeSettings advancedModeSettings;

        #region TimeMode Additional Settings

        // Initial timer amount in seconds.
        public int initialTime = 60;

        // Bonus seconds on line break.
        public int addSecondsOnLineBreak = 10;

        #endregion

        // Should give reward on game over.
        public bool rewardOnGameOver = true;

        // Should give fixed reward on game over.
        public bool giveFixedReward = false;

        // Fixed Reward amount.
        public int fixedRewardAmount;

        // Amount of reward on game over.
        public float rewardPerLineCompleted = 1F;

        // Score per block clear.
        public int blockScore = 20;

        // Score on breaking line.
        public int singleLineBreakScore = 100;

        // Additional score multiplier on breaking more than 1 line.
        public int multiLineScoreMultiplier = 50;

        /// <summary>
        /// Game step duration in seconds.
        /// Lover -> faster game.
        /// </summary>
        public float gameSpeed = 1.0f;
    }

    /// <summary>
    /// Block shape info like prefab reference, sprite tag and probability of spawning it.
    /// </summary>
    [System.Serializable]
    public class BlockShapeInfo
    {
        public GameObject blockShape;
        public string blockSpriteTag;
        public int spawnProbability;
    }

    /// <summary>
    /// Game mode settings.
    /// </summary>
    [System.Serializable]
    public class GameModeSettings
    {
        // Game mode is should be active or likewise,
        public bool modeEnabled = true;

        // Size of block on board.
        public float blockSize;

        // Space between blocks on board.
        public float blockSpace;

        // Rotation allowed for block shape or not.
        public bool allowRotation = false;

        // Shape of block shape when inactive and inside shape container.
        public float shapeInactiveSize = 0.5F;

        // Offset from finger while dragging block shape.
        public float shapeDragPositionOffset = 1.0F;

        // Save Progress
        public bool saveProgress = true;
    }
}