using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.Scripts.Controller;
using Assets.Wordis.BlockPuzzle.Scripts.GamePlay;
using Assets.Wordis.BlockPuzzle.Scripts.UI.Extensions;
using Assets.Wordis.Frameworks.InputManager.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Wordis.BlockPuzzle.Scripts.UI
{
    /// <summary>
    /// Selection on mode to be played.
    /// </summary>
    public class Statistics : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] GameObject _statItemTemplate;
        [SerializeField] GameObject _statsListContent;
        [SerializeField] GameObject _wordsUnlockedCounter;
#pragma warning restore 0649

        /// <summary>
        /// Close button listener.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void OnEnable()
        {
            PrepareStatsScreen();
        }

        private void PrepareStatsScreen()
        {
            var wordStats = GameProgressTracker.Instance.GetWordStats();

            SetWordsUnlockedCounter(wordStats.Count);

            foreach (var wordStat in wordStats)
            {
                CreateWordStatItem(wordStat.Key, wordStat.Value);
            }
        }

        private void SetWordsUnlockedCounter(int uniqueWordsUnlocked)
        {
            _wordsUnlockedCounter.GetComponent<Text>().text = $"{uniqueWordsUnlocked}";
        }

        /// <summary>
        /// Instantiates a button from template.
        /// </summary>
        /// <returns></returns>
        private void CreateWordStatItem(string word, int counter)
        {
            GameObject statItem = Instantiate(_statItemTemplate);
            statItem.transform.SetParent(_statsListContent.transform);
            statItem.name = $"itm-{word}";
            statItem.transform.localScale = Vector3.one;
            statItem.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            statItem.transform.SetAsLastSibling();
            statItem.GetComponentsInChildren<Text>().First().text = word;
            statItem.GetComponentsInChildren<Text>().Last().text = $"{counter}";
            statItem.SetActive(true);

            //// set level startup callback
            //statItem.GetComponent<Button>().onClick.AddListener(() => StartLevel(level));
        }

        private void CreateMoreToComeButton()
        {
            //GameObject statItem = Instantiate(_statItemTemplate);
            //statItem.transform.SetParent(_levelListContent.transform);
            //statItem.name = "moreToComeBtn";
            //statItem.transform.localScale = Vector3.one;
            //statItem.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            //statItem.transform.SetAsLastSibling();
            //statItem.GetComponentInChildren<Text>().text = "More to come...";
            //statItem.GetComponent<Button>().interactable = false;
            //statItem.SetActive(true);
        }

        /// <summary>
        /// Level button listener
        /// </summary>
        private void StartLevel(IWordisGameLevel level)
        {
            //if (InputManager.Instance.canInput())
            //{
            //    InputManager.Instance.DisableTouchForDelay();
            //    UIFeedback.Instance.PlayButtonPressEffect();
            //    UIController.Instance.LoadGamePlay(level);
            //    gameObject.Deactivate();
            //}
        }
    }
}