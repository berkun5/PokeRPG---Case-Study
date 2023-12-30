using UnityEngine;

namespace UI.FloatingText
{
    public interface IFloatingTextPanelViewModel 
    {
        string UpdatedTextInformation { get; }
        void CompleteAnimation();
        void SetTextPosition(Transform sourceTransform);
    }
}
