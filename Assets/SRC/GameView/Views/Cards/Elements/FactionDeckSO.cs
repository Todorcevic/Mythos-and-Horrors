using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "Token", menuName = "ScriptableObjects/DeckElements")]
    public class FactionDeckSO : ScriptableObject
    {
        [SerializeField, Required] public Faction _faction;
        [SerializeField, Required, AssetsOnly] public Sprite _templateDeckFront;
        [SerializeField, Required, AssetsOnly] public Sprite _badget;
        [SerializeField, Required, AssetsOnly] public Sprite _titleHolder;
        [SerializeField, Required, AssetsOnly] public Sprite _skillHolder;
    }
}
