using System.Text;
using GameConfig;
using GameConfig.Enum;
using GameServices;
using GameServices.ServiceLocator;
using Managers;
using ReactiveProperty;
using UI.ExtensionsAndHelpers;
using UnityEngine;

namespace UI.CharacterSelection.Popup
{
    public class CharacterStatsPopupWindowViewModel : ICharacterStatsPopupWindowViewModel
    {
        public string CharacterName { get; }
        public string CharacterLevel { get; }
        public string CharacterHealth { get; }
        public string CharacterAttack { get; }
        public string CharacterExperience { get; }
        public Sprite CharacterSprite { get; }

        private readonly Vector2 _targetPosition;
        public CharacterStatsPopupWindowViewModel(CharacterId characterId, Vector2 targetRectPosition)
        {
            _targetPosition = targetRectPosition;
            var builder = new StringBuilder();

            var settingsManager = GameServiceLocator.GetService<PersistentServiceProvider>().GetManager<SettingsManager>();
            var remoteCharacterData = settingsManager.RemoteData.CharacterData;
            CharacterSprite = settingsManager.LocalData.CharacterConfigs.GetConfig(characterId).Visual;
            
            builder.Append("Name: <size=120%>").Append(characterId);
            CharacterName = builder.ToString();
            builder.Clear();
            
            builder.AppendFormat("Health: <size=120%>{0:F1}", remoteCharacterData.GetCharacterTotalHealth(characterId));
            CharacterHealth = builder.ToString();
            builder.Clear();
            
            builder.Append("Level: <size=120%>").Append(remoteCharacterData.GetCharacterLevel(characterId));
            CharacterLevel = builder.ToString();
            builder.Clear();
            
            builder.AppendFormat("Attack Power: <size=120%>{0:F1}", remoteCharacterData.GetCharacterAttackPower(characterId));
            CharacterAttack = builder.ToString();
            builder.Clear();
            
            builder.Append("Experience: <size=120%>").Append(remoteCharacterData.GetCharacterExperience(characterId));
            CharacterExperience = builder.ToString();
            builder.Clear();
        }
        
        Vector2 ICharacterStatsPopupWindowViewModel.InfoPanelPosition(RectTransform targetRect)
        {
            return RectTransformExtensions.GetInScreenRectPosition(_targetPosition, targetRect);
        }
    }
}
