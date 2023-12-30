using System;
using Characters.Base;
using Commands.CombatCommands;
using GameConfig.Enum;
using GameServices;
using GameServices.ServiceLocator;
using Managers.UI;
using UI.CharacterSelection.Popup;
using UI.ExtensionsAndHelpers;
using UI.Popup;
using UnityEngine;

namespace Characters
{
    public class PlayerCharacterEntity : CharacterEntityBase
    {
        private const CombatState RequiredCombatState = CombatState.PlayerTurn;
        [SerializeField] private LongPressDetection longPressDetection;
        private UIModelManager _uiModelManager;
        private bool _longPressToggled;
        
        private void OnDisable()
        {
            longPressDetection.LongPressToggled -= ToggleWorldSpacePopup;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _uiModelManager = GameServiceLocator.GetService<UIModelServiceProvider>().GetManager<UIModelManager>();
            
            longPressDetection.LongPressToggled -= ToggleWorldSpacePopup;
            longPressDetection.LongPressToggled += ToggleWorldSpacePopup;
        }

        private void OnMouseUpAsButton()
        {
            if (_longPressToggled)
            {
                return;
            }
            
            Attack();
        }
        
        protected override void Attack()
        {
            var bossCharacter= CombatManager.TryGetRandomBossAsTarget(out var noBossTarget);
           
            if (noBossTarget)
            {
                return;
            }

            var attackCommand = new MeleeAttackCommand(this, bossCharacter);
            CombatManager.TryAttack(RequiredCombatState, attackCommand);
        }

        private void ToggleWorldSpacePopup(bool show)
        {
            _longPressToggled = show;
            if (show)
            {
                // Convert the world space position to screen space
                var screenPosition = _uiModelManager.MainCamera.WorldToScreenPoint(characterSpriteRenderer.transform.position);
                TryShowStatsPopup(screenPosition);
            }
            else
            {
                TryHideStatsPopup();
            }
        }
        
        private void TryShowStatsPopup(Vector2 viewRectPos)
        {
            TryHideStatsPopup();
            _uiModelManager.Show<CharacterStatsPopupWindow>(window => 
                window.Init(new CharacterStatsPopupWindowViewModel(Config.CharacterId, viewRectPos)));
        }

        private void TryHideStatsPopup() => _uiModelManager.Hide<CharacterStatsPopupWindow>();
        
        protected override void SetGraphics()
        {
            base.SetGraphics();
            //add player specific particles and such
        }
    }
}
