using System;
using ReactiveProperty;
using UnityEngine;

namespace UI.Character
{
    public interface ICharacterViewModel : IDisposable
    {
        void HandleSelectionLogic();
        void HandleToggleStatsPopup(bool show, Vector2 viewRectPosition);
        Sprite CharacterIcon { get; }
        bool ShowStatIncrease { get; }
        string StatIncreaseInfo { get; }
        IReactiveProperty<Color> CharacterImageColor { get; }
        IReactiveProperty<bool> CharacterSelected { get; }
    }
}
