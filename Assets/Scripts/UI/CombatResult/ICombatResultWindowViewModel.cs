using System.Collections.Generic;
using UI.Character;

namespace UI.CombatResult
{
    public interface ICombatResultWindowViewModel
    {
        string ResultHeader { get; }
        List<ICharacterViewModel> SelectedCharacterViewModels { get; }
        void HandleExitCombatButton();
    }
}
