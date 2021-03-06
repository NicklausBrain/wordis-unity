﻿// ©2019 - 2020 HYPERBYTE STUDIOS LLP
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

namespace Assets.Wordis.BlockPuzzle.Scripts.UI
{
    public class SelectRatingStars : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler,
        IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
#pragma warning disable 0649
        [SerializeField] ReviewAppScreen reviewAppScreen;

        [SerializeField] Sprite emptyLeftStarSprite;
        [SerializeField] Sprite emptyRightStarSprite;
        [SerializeField] Sprite filledLeftStarSprite;
        [SerializeField] Sprite filledRightStarSprite;

        [SerializeField] List<Image> allStars;
#pragma warning restore 0649

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnStarSelected(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnStarSelected(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        private void OnStarSelected(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0 && results[0].gameObject.name.Contains("img-star"))
            {
                int.TryParse(results[0].gameObject.name.Replace("img-star-", ""), out var starIndex);

                if (starIndex >= 0)
                {
                    ProcessStar(starIndex);
                }
            }
        }

        private void ProcessStar(int starIndex)
        {
            Image starImage = allStars[starIndex];

            if (starImage.sprite.name.Contains("empty"))
            {
                Fill(starIndex);
            }
            else
            {
                Empty(starIndex);
            }
        }

        private void Fill(int starIndex)
        {
            for (int i = 0; i <= starIndex; i++)
            {
                Image starImage = allStars[i];
                starImage.sprite = i % 2 == 0
                    ? filledLeftStarSprite
                    : filledRightStarSprite;
            }

            reviewAppScreen.currentRating = (starIndex + 1) * 0.5F;
        }

        private void Empty(int starIndex)
        {
            for (int i = allStars.Count - 1; i > starIndex; i--)
            {
                Image starImage = allStars[i];

                starImage.sprite = i % 2 == 0
                    ? emptyLeftStarSprite
                    : emptyRightStarSprite;
            }

            reviewAppScreen.currentRating = (starIndex + 1) * 0.5F;
        }
    }
}