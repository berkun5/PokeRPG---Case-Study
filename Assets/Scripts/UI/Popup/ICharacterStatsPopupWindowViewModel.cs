using ReactiveProperty;
using UnityEngine;

namespace UI.CharacterSelection.Popup
{
    public interface ICharacterStatsPopupWindowViewModel
    {
        Vector2 InfoPanelPosition(RectTransform targetRect);
        string CharacterName { get; }
        string CharacterLevel { get; }
        string CharacterHealth { get; }
        string CharacterAttack { get; }
        string CharacterExperience { get; }
        Sprite CharacterSprite { get; }
    }
}
