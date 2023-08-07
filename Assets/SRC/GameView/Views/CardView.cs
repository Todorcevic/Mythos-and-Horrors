using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GameView
{
    public class CardView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _title;
    }
}
