using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class InvestigatorCardView : CardView
    {
        [SerializeField, Required, AssetsOnly] private Sprite _healthIcon;
        [SerializeField, Required, AssetsOnly] private Sprite _sanityIcon;
        [SerializeField, Required, ChildGameObjectsOnly] private CounterController _health;
        [SerializeField, Required, ChildGameObjectsOnly] private CounterController _sanity;

        public CardInvestigator CardInvestigator => (CardInvestigator)Card;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            _health.SetWith(_healthIcon, CardInvestigator.Health.Value);
            _sanity.SetWith(_sanityIcon, CardInvestigator.Sanity.Value);
        }
    }
}
