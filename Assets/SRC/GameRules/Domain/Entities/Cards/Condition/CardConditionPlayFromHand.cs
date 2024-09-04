using System;
using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardConditionPlayFromHand : CardCondition, IPlayableFromHandInTurn
    {
        public Stat PlayFromHandTurnsCost { get; protected set; }
        public GameConditionWith<Investigator> PlayFromHandCondition { get; private set; }

        public virtual Func<Card> CardAffected => null;

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
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner != investigator) return false;
            if (ResourceCost.Value > ControlOwner.Resources.Value) return false;
            if (PlayFromHandTurnsCost.Value > ControlOwner.CurrentTurns.Value) return false;
            return CanPlayFromHandSpecific(investigator);
        }

        protected abstract bool CanPlayFromHandSpecific(Investigator investigator);
    }
}
