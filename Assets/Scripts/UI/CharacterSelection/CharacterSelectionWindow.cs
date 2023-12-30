using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Character;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CharacterSelection
{
    public class CharacterSelectionWindow : UIEntity
    {
        [Header("Container")]
        [SerializeField] private RectTransform characterViewContainer;
        [SerializeField] private GridLayoutGroup characterContainerGrid;

        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI selectionInformationText;

        [SerializeField] private TextMeshProUGUI remainingCombatCountText;
        
        [Header("Buttons")]
        [SerializeField] private Button startCombatButton;
        
        [Header("Prefabs")]
        [SerializeField] private CharacterView characterViewPrefab;
        
        private ICharacterSelectionWindowViewModel _viewModel;
        private readonly List<CharacterView> _characterViewInstances = new();

        private void OnDisable()
        {
            _viewModel?.ToggleStartCombatButton.Unsubscribe(OnToggleStartCombatButton);
            startCombatButton.onClick.RemoveAllListeners();
        }
        
        public void Init(ICharacterSelectionWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            
            viewModel.ToggleStartCombatButton.Subscribe(OnToggleStartCombatButton);
            viewModel.SetSelectionInformationText.Subscribe(OnselectionInformationTextChanged);
            
            startCombatButton.onClick.AddListener(viewModel.HandleStartCombatLogic);

            remainingCombatCountText.text = viewModel.SetNewCharacterCountText;
            InitCharacterViews();
            StartCoroutine(DisableConstructedLayouts());
        }
        
        private void InitCharacterViews()
        {
            var viewModels = _viewModel.AllCharacterViewModels;
            for (int i = 0; i < viewModels.Count; i++)
            {
                var viewModel = viewModels[i];
                var hasViewForThisIndex = _characterViewInstances.Count > i;
                var view = hasViewForThisIndex ? _characterViewInstances[i] : Instantiate(characterViewPrefab, characterViewContainer);
                
                view.gameObject.SetActive(true);
                view.Init(viewModel);

                if (!hasViewForThisIndex)
                {
                    _characterViewInstances.Add(view);
                }
            }
        }

        private IEnumerator DisableConstructedLayouts()
        {
            //this is how I usually handle disabling all layouts and force rebuild once they are constructed.
            //it helps a lot with the framerate especially on stacked UI, I would love to hear a better solution.
            yield return new WaitForEndOfFrame();
            characterContainerGrid.enabled = false;
        }
        
        private void OnToggleStartCombatButton(bool show)
        {
            startCombatButton.gameObject.SetActive(show);
        }
        
        private void OnselectionInformationTextChanged(string text)
        {
            selectionInformationText.text = text;
        }
    }
}
