using System;
using System.Collections.Generic;
using ReactiveProperty;
using UI.Character;

namespace UI.CharacterSelection
{
    public interface ICharacterSelectionWindowViewModel : IDisposable
    {
        void HandleStartCombatLogic();
        string SetNewCharacterCountText { get; }
        IReactiveProperty<bool> ToggleStartCombatButton { get; }
        IReactiveProperty<string> SetSelectionInformationText { get; }
        List<ICharacterViewModel> AllCharacterViewModels { get; }
    }
}
