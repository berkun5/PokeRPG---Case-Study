using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class UIEntity : MonoBehaviour
    {
        public bool PrepareOnInit => prepareOnInit;
        [SerializeField] private bool prepareOnInit;
    }
}
