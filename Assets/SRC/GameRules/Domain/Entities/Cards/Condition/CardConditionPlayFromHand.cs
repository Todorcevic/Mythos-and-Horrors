using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardConditionPlayFromHand : CardCondition, IPlayableFromHand
    {
        public Stat PlayFromHandTurnsCost { get; private set; }
        public GameConditionWith<Investigator> PlayFromHandCondition { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandCondition = new GameConditionWith<Investigator>(CanPlayFromHandWith);
            PlayFromHandTurnsCost = CreateStat(1);
        }

        /*******************************************************************/
        protected bool CanPlayFromHandWith(Investigator investigator)
        {
            //if (gameAction is not OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction) return false;
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner != investigator) return false;
            if (ResourceCost.Value > ControlOwner.Resources.Value) return false;
            if (PlayFromHandTurnsCost.Value > ControlOwner.CurrentTurns.Value) return false;
            return CanPlayFromHandSpecific(investigator);
        }

        protected abstract bool CanPlayFromHandSpecific(Investigator investigator);
    }
}
