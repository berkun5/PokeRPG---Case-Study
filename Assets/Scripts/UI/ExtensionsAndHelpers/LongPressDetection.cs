using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.ExtensionsAndHelpers
{
    public class LongPressDetection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<bool> LongPressToggled;
        [SerializeField] private float requiredHoldTime = 3f;
        private bool _isPointerDown;
        private bool _isLongPressing;
        private float _pressStartTime;

        public void OnPointerDown(PointerEventData eventData) => PressDown();
        public void OnPointerUp(PointerEventData eventData) => PressUp();
        private void OnMouseDown() => PressDown();
        private void OnMouseUp() => PressUp();
        
        private void Update()
        {
            if (!_isPointerDown)
            {
                return;
            }
            
            if (Time.time - _pressStartTime >= requiredHoldTime)
            {
                if (_isLongPressing)
                {
                    return;
                }
                
                _isLongPressing = true;
                LongPressToggled?.Invoke(true);
            }
        }

        private void PressDown()
        {
            _isPointerDown = true;
            _pressStartTime = Time.time;
        }

        private void PressUp()
        {
            StartCoroutine(EndLongPress());
        }

        private IEnumerator EndLongPress()
        {
            //must be completed one frame later to secure execution order with the regular press
            yield return new WaitForEndOfFrame();
            if (_isLongPressing)
            {
                LongPressToggled?.Invoke(false);
            }
            
            _isLongPressing = false;
            _isPointerDown = false;
        }
    }
}