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
        public string Description { get; }
        public bool WithOpportunityAttack => !IsFreeActivation && (PlayActionType & PlayActionType.WithoutOpportunityAttack) == PlayActionType.None;
        public bool IsFreeActivation => ActivateTurnsCost.Value < 1;

        /*******************************************************************/
        public BaseEffect(Stat activateTurnCost, Func<Task> logic, PlayActionType playActionType, Investigator playedBy, string description)
        {
            ActivateTurnsCost = activateTurnCost;
            Logic = logic;
            PlayActionType = playActionType;
            Investigator = playedBy;
            Description = description;
        }

        /*******************************************************************/
        public bool IsThatActionType(PlayActionType actionType) => (PlayActionType & actionType) == actionType;
        public bool IsOneTheseActionType(PlayActionType actionType) => (PlayActionType & actionType) == 0;
    }
}
