using System.Collections;
using TMPro;
using UI.Character;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CombatResult
{
    public class CombatResultWindow : UIEntity
    {
        [SerializeField] private TextMeshProUGUI winLoseText;
        [SerializeField] private Button exitButton;
        [Header("CharacterViewGrid")]
        [SerializeField] private RectTransform characterViewContainer;
        [SerializeField] private GridLayoutGroup characterViewGrid;
        [SerializeField] private CharacterView characterViewPrefab;

        private ICombatResultWindowViewModel _viewModel;

        private void OnDisable()
        {
            exitButton.onClick.RemoveAllListeners();
        }

        public void Init(ICombatResultWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            winLoseText.text = viewModel.ResultHeader;
            
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(OnExitButtonTriggered);
            
            InitCharacterViews();
            StartCoroutine(DisableConstructedLayouts());
        }

        private void OnExitButtonTriggered() => _viewModel.HandleExitCombatButton();
        
        private void InitCharacterViews()
        {
            var viewModels = _viewModel.SelectedCharacterViewModels;
            foreach (var viewModel in viewModels)
            {
                var view = Instantiate(characterViewPrefab, characterViewContainer);
                view.gameObject.SetActive(true);
                view.Init(viewModel);
            }
        }
        
        private IEnumerator DisableConstructedLayouts()
        {
            yield return new WaitForEndOfFrame();
            characterViewGrid.enabled = false;
        }
    }
}
