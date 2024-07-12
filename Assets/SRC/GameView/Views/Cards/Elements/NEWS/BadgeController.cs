using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{

    public class BadgeController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private List<FactionInvestigatorSO> _allFactions;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _badge;

        /*******************************************************************/
        public void SetBadge(Faction faction)
        {
            _badge.sprite = _allFactions.Find(factionDeckSO => factionDeckSO._faction == faction)._badget;
        }
    }
}
