using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.FloatingText
{
    public class FloatingTextPanel : UIEntity
    {
        [SerializeField] private RectTransform panelRect;
        [SerializeField] private TextMeshProUGUI informationText;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private IFloatingTextPanelViewModel _viewModel;
        private Sequence _floatingSequence;
        
        private void OnDisable() => _floatingSequence?.Kill();
        
        public void Init(IFloatingTextPanelViewModel viewModel)
        {
            _viewModel = viewModel;
            
            informationText.text = _viewModel.UpdatedTextInformation;
            _viewModel.SetTextPosition(panelRect.transform);
            
            _floatingSequence = AnimateSequence(); 
            _floatingSequence.OnComplete(() => _viewModel.CompleteAnimation());
        }
        
        private Sequence AnimateSequence()
        {
            const int moveUpAmount = 175;
            const float sequenceInDuration = 1.5f;
            const float fadeDuration = sequenceInDuration / 3;
            
            canvasGroup.alpha = 0;
            var targetYPos = panelRect.transform.position.y + moveUpAmount;
            
            var seq = DOTween.Sequence();
            seq.Append(canvasGroup.DOFade(1, fadeDuration));
            seq.Join(panelRect.transform.DOScale(Vector3.zero, fadeDuration).From());
            
            seq.AppendInterval(-fadeDuration);
            seq.Append(panelRect.transform.DOMoveY(targetYPos, sequenceInDuration));
            
            seq.AppendInterval(-fadeDuration);
            seq.Append(canvasGroup.DOFade(0, fadeDuration));
            
            seq.SetEase(Ease.OutSine);
            return seq;
        }
    }
}