using TMPro;
using UI.ExtensionsAndHelpers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Character
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private LongPressDetection longPressDetection;
        
        [Header("RectTransform")]
        [SerializeField] private RectTransform viewRectTransform;
        [SerializeField] private RectTransform statIncreaseRect;
        
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI statIncreaseText;
        
        [Header("Images")]
        [SerializeField] private Image characterIcon;
        [SerializeField] private Image selectionHighlight;
        
        [Header("Buttons")]
        [SerializeField] private Button selectButton;

        private ICharacterViewModel _viewModel;
        
        private void OnDisable()
        {
            longPressDetection.LongPressToggled -= TogglePopup;
            _viewModel?.CharacterSelected.Unsubscribe(ToggleHighlightSelection);
            _viewModel?.CharacterImageColor.Unsubscribe(ToggleCharacterImageColor);
            selectButton.onClick.RemoveAllListeners();
        }

        public void Init(ICharacterViewModel viewModel)
        {
            _viewModel = viewModel;
            characterIcon.sprite = viewModel.CharacterIcon;

            statIncreaseRect.gameObject.SetActive(viewModel.ShowStatIncrease);
            statIncreaseText.text = viewModel.StatIncreaseInfo;
            
            longPressDetection.LongPressToggled -= TogglePopup;
            longPressDetection.LongPressToggled += TogglePopup;
            
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(viewModel.HandleSelectionLogic);
            selectButton.onClick.AddListener(AnimateSelection);
            
            viewModel.CharacterSelected.Subscribe(ToggleHighlightSelection);
            viewModel.CharacterImageColor.Subscribe(ToggleCharacterImageColor, false);
        }

        private void TogglePopup(bool show) => _viewModel.HandleToggleStatsPopup(show, viewRectTransform.position);

        private void AnimateSelection()
        {
           // _viewModel.Unlocked;
            //add dotween
        }

        private void ToggleHighlightSelection(bool selected)
        {
            selectionHighlight.gameObject.SetActive(selected);
        }
        
        private void ToggleCharacterImageColor(Color c)
        {
            characterIcon.color = c;
        }
    }
}
