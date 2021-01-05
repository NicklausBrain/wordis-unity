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

using System;
using UnityEngine;
using UnityEngine.UI;
using Hyperbyte.Localization;

namespace Hyperbyte
{
    /// <summary>
    /// This script component is attched to common dialogue popup. This script will prepare a delegate based button click event callbacks
    /// which can be programmer as per the requirement.
    /// </summary>
    public class CommonMessageScreen : MonoBehaviour
    {
        [Header("To be assigned from inspector")]

        #pragma warning disable 0649
        [Tooltip("Popup Title")]
        [SerializeField] Text txtTitle;

        [Tooltip("Message on the popup")]
        [SerializeField] Text txtDescription;

        [Tooltip("Confirm Button")]
        [SerializeField] Button btnConfirm;

        [Tooltip("Positiove Button, Acts as yes button")]
        [SerializeField] Button btnPositive;

        [Tooltip("Negative Button, Acts as no button")]
        [SerializeField] Button btnNegative;

        [Tooltip("Text on confirm buttom")]
        [SerializeField] Text btnConfirmText;

        [Tooltip("Text on positiove buttom")]
        [SerializeField] Text btnPositiveText;

        [Tooltip("Text on negative buttom")]
        [SerializeField] Text btnNegativeText;
        #pragma warning restore 0649

        // Action delegate for Positive button press callback.
        public Action OnPositiveButtonPressedAction;
        
        // Action delegate for negative button press callback.
        public Action OnNegativeButtonPressedAction;

        // Action delegate for confirm button press callback.
        public Action OnConfirmButtonPressedAction;

        /// <summary>
        /// Create a dialogue with required info and assigns delegate from the info.
        /// </summary>
        public void SetDialogueInfo(CommonDialogueInfo info)
        {
            txtTitle.text = info.GetTitle();
            txtDescription.text = info.GetMessage();

            if (info.GetOnPositiveButtonClickListener() != null)
            {
                OnPositiveButtonPressedAction = info.GetOnPositiveButtonClickListener();
                btnPositiveText.text = info.GetPositiveButtonText();
            }

            if (info.GetOnNegativeButtonClickListener() != null)
            {
                OnNegativeButtonPressedAction = info.GetOnNegativeButtonClickListener();
                btnNegativeText.text = info.GetNegativeButtonText();
            }

            if (info.GetOnConfirmButtonClickListener() != null)
            {
                OnConfirmButtonPressedAction = info.GetOnConfirmButtonClickListener();
                btnConfirmText.text = info.GetConfirmButtonText();
            }

            switch (info.GetMessageType())
            {
                case CommonDialogueMessageType.Info:
                    btnNegative.gameObject.SetActive(false);
                    btnPositive.gameObject.SetActive(false);
                    btnConfirm.gameObject.SetActive(true);
                    break;

                case CommonDialogueMessageType.Confirmation:
                    btnNegative.gameObject.SetActive(true);
                    btnPositive.gameObject.SetActive(true);
                    btnConfirm.gameObject.SetActive(false);
                    break;
            }
        }


        /// <summary>
        /// Closes the dialogue.
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
        /// Forward Positive Button Press callback.
        /// </summary>
        public void OnPositiveButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();

                if (OnPositiveButtonPressedAction != null)
                {
                    OnPositiveButtonPressedAction.Invoke();
                }
            }
        }

        /// <summary>
        /// Forward Negative Button Press callback.
        /// </summary>
        public void OnNegativeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();

                if (OnNegativeButtonPressedAction != null)
                {
                    OnNegativeButtonPressedAction.Invoke();
                }
            }
        }

        /// <summary>
        /// Forward Confirm Button Press callback.
        /// </summary>
        public void OnConfirmButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();

                if (OnConfirmButtonPressedAction != null)
                {
                    OnConfirmButtonPressedAction.Invoke();
                }
            }
        }
    }
    
    /// <summary>
    /// Common dialogue info with constructor.
    /// </summary>
    public class CommonDialogueInfo
    {
        CommonDialogueMessageType messageType;
        string title;
        string message;

        string txtConfirmButtonText = LocalizationManager.Instance.GetTextWithTag("txtOk");
        string txtPositiveButtonText = LocalizationManager.Instance.GetTextWithTag("txtYes");
        string txtNegativeButtonText = LocalizationManager.Instance.GetTextWithTag("txtNo");

        Action onPositiveButtonPressedAction;
        Action onNegativeButtonPressedAction;
        Action onConfirmButtonPressedAction;

        public CommonDialogueInfo SetTitle(string text)
        {
            title = text;
            return this;
        }

        public CommonDialogueMessageType GetMessageType() { return messageType; }
        public string GetTitle() { return title; }
        public string GetMessage() { return message; }

        public string GetConfirmButtonText() { return txtConfirmButtonText; }
        public string GetPositiveButtonText() { return txtPositiveButtonText; }
        public string GetNegativeButtonText() { return txtNegativeButtonText; }

        public Action GetOnConfirmButtonClickListener() { return onConfirmButtonPressedAction; }
        public Action GetOnPositiveButtonClickListener() { return onPositiveButtonPressedAction; }
        public Action GetOnNegativeButtonClickListener() { return onNegativeButtonPressedAction; }

        public CommonDialogueInfo SetMessageType(CommonDialogueMessageType _messageType) { messageType = _messageType; return this; }
        public CommonDialogueInfo SetMessage(string text) { message = text; return this; }
        public CommonDialogueInfo SetOnConfirmButtonClickListener(Action listener) { onConfirmButtonPressedAction = listener; return this; }
        public CommonDialogueInfo SetOnPositiveButtonClickListener(Action listener) { onPositiveButtonPressedAction = listener; return this; }
        public CommonDialogueInfo SetOnNegativeButtonClickListener(Action listener) { onNegativeButtonPressedAction = listener; return this; }
        public CommonDialogueInfo SetConfirmButtonText(string text) { txtConfirmButtonText = text; return this; }
        public CommonDialogueInfo SetPositiveButtomText(string text) { txtPositiveButtonText = text; return this; }
        public CommonDialogueInfo SetNegativeButtonText(string text) { txtNegativeButtonText = text; return this; }

        public void Show()
        {
            UIController.Instance.commonMessageScreen.Activate();
            UIController.Instance.commonMessageScreen.GetComponent<CommonMessageScreen>().SetDialogueInfo(this);
        }
    }

    /// <summary>
    /// Dialogue Type. 
    /// </summary>
    public enum CommonDialogueMessageType
    {
        Info = 0,           
        Confirmation = 1
    }
}