using GameServices;
using GameServices.ServiceLocator;
using Managers.UI;
using UnityEngine;

namespace UI.FloatingText
{
    public class FloatingTextPanelViewModel : IFloatingTextPanelViewModel
    {
        private readonly Vector2 _receivedWorldPosition;
        private readonly string _updatedTextInformation;
        private readonly UIModelManager _uiModelManager;
        string IFloatingTextPanelViewModel.UpdatedTextInformation => _updatedTextInformation;
        
        public FloatingTextPanelViewModel(string textInfo, Vector3 worldPosition)
        {
            _uiModelManager = GameServiceLocator.GetService<UIModelServiceProvider>().GetManager<UIModelManager>();

            _updatedTextInformation = $"-{textInfo} HP";
            _receivedWorldPosition = worldPosition;
        }
        
        void IFloatingTextPanelViewModel.CompleteAnimation()
        {
            _uiModelManager.Hide<FloatingTextPanel>();
        }
        
        void IFloatingTextPanelViewModel.SetTextPosition(Transform sourceTransform)
        {
            var screenPos = _uiModelManager.MainCamera.WorldToScreenPoint(_receivedWorldPosition);
            sourceTransform.position = screenPos;
        }
    }
}
