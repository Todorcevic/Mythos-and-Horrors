using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    [CreateAssetMenu(fileName = "Faction", menuName = "ScriptableObjects/InvestigatorElements")]
    public class FactionInvestigatorSO : ScriptableObject
    {
        [SerializeField, Required] public Faction _faction;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _badget;
    }
}
