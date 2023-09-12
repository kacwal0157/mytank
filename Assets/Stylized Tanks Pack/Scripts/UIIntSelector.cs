
using UnityEngine;
using UnityEngine.UI;

namespace ModularTanksPack
{
    public class UIIntSelector : MonoBehaviour
    {
        public event System.Action<int> SeletedOptionChanged;

        [SerializeField]
        private Text selectedOptionNumberLabel;
        [SerializeField]
        private Button previousOptionButton;
        [SerializeField]
        private Button nextOptionButton;

        private int selectedOptionNumber;
        private int minOptionNumber;
        private int maxOptionNumber;

        private void Awake()
        {
            previousOptionButton.onClick.AddListener(OnPreviousOptionButtonClicked);
            nextOptionButton.onClick.AddListener(OnNextOptionButtonClicked);
        }

        public void Init(int minOptionNumber, int maxOptionNumber, int selectedOptionNumber = 0)
        {
            this.minOptionNumber = minOptionNumber;
            this.maxOptionNumber = maxOptionNumber;
            SetSelectedOption(selectedOptionNumber);
        }

        public void SetSelectedOption(int selectedOptionNumber)
        {
            this.selectedOptionNumber = selectedOptionNumber;
            selectedOptionNumberLabel.text = selectedOptionNumber.ToString();
        }

        private void OnPreviousOptionButtonClicked()
        {
            ChangedOption(-1);
        }

        private void OnNextOptionButtonClicked()
        {
            ChangedOption(1);
        }

        private void ChangedOption(int changeBy)
        {
            selectedOptionNumber += changeBy;
            if (selectedOptionNumber < minOptionNumber)
            {
                selectedOptionNumber = maxOptionNumber;
            }
            else if (selectedOptionNumber > maxOptionNumber)
            {
                selectedOptionNumber = minOptionNumber;
            }
            selectedOptionNumberLabel.text = selectedOptionNumber.ToString();
            if (SeletedOptionChanged != null)
            {
                SeletedOptionChanged.Invoke(selectedOptionNumber);
            }
        }
    }
}
