using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameView
{
    public class CardView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _title;

        public string Id { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init(string id, string name)
        {
            Id = id;
            _title.text = name;
        }

        /*******************************************************************/
    }
}
