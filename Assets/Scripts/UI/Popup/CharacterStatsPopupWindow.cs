using TMPro;
using UI.CharacterSelection.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popup
{
    public class CharacterStatsPopupWindow : UIEntity
    {
        [SerializeField] private RectTransform infoPanelRect;
        [SerializeField] private Image characterImage;
        
        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI attackText;
        [SerializeField] private TextMeshProUGUI experienceText;
        
        public void Init(ICharacterStatsPopupWindowViewModel viewModel)
        {
            infoPanelRect.position = viewModel.InfoPanelPosition(infoPanelRect);

            characterImage.sprite = viewModel.CharacterSprite;
            nameText.text = viewModel.CharacterName;
            healthText.text = viewModel.CharacterHealth;
            levelText.text = viewModel.CharacterLevel;
            attackText.text = viewModel.CharacterAttack;
            experienceText.text = viewModel.CharacterExperience;
        }
    }
}
