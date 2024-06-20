using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardConditionPlayFromHand : CardCondition, IPlayableFromHand
    {
        public Stat PlayFromHandTurnsCost { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandTurnsCost = CreateStat(1);
        }

        /*******************************************************************/
        protected sealed override bool CanPlayFromHandWith(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction) return false;
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner != oneInvestigatorTurnGameAction.ActiveInvestigator) return false;
            if (ResourceCost.Value > ControlOwner.Resources.Value) return false;
            if (PlayFromHandTurnsCost.Value > ControlOwner.CurrentTurns.Value) return false;
            return CanPlayFromHandSpecific(gameAction);
        }

        protected abstract bool CanPlayFromHandSpecific(GameAction gameAction);
    }
}
