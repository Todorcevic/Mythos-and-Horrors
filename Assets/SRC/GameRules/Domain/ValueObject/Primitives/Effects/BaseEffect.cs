using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record BaseEffect : IViewEffectDescription
    {
        public Stat ActivateTurnsCost { get; }
        public PlayActionType PlayActionType { get; }
        public Investigator Investigator { get; }
        public Func<Task> Logic { get; private set; }
        public bool WithOpportunityAttack => !IsFreeActivation && (PlayActionType & PlayActionType.WithoutOpportunityAttack) == PlayActionType.None;
        public bool IsFreeActivation => ActivateTurnsCost.Value < 1;
        public bool CanBePlayed => ActivateTurnsCost.Value <= Investigator.CurrentTurns.Value;
        public Localization Localization { get; private set; }

        /*******************************************************************/
        public BaseEffect(Stat activateTurnCost, Func<Task> logic, PlayActionType playActionType, Investigator playedBy, Localization localization)
        {
            ActivateTurnsCost = activateTurnCost;
            Logic = logic;
            PlayActionType = playActionType;
            Investigator = playedBy;
            Localization = localization;
        }

        /*******************************************************************/
        public bool IsThatActionType(PlayActionType actionType) => (PlayActionType & actionType) == actionType;
        public bool IsOneTheseActionType(PlayActionType actionType) => (PlayActionType & actionType) != 0;
    }
}
