using System;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record BaseEffect : IViewEffectDescription
    {
        private readonly string _description;
        public Stat ActivateTurnsCost { get; }
        public PlayActionType PlayActionType { get; }
        public Investigator Investigator { get; }
        public Func<Task> Logic { get; private set; }
        public string Description => _description ?? Logic.GetInvocationList().First().Method.Name;
        public bool WithOpportunityAttack => !IsFreeActivation && (PlayActionType & PlayActionType.WithoutOpportunityAttack) == PlayActionType.None;
        public bool IsFreeActivation => ActivateTurnsCost.Value < 1;

        /*******************************************************************/
        public BaseEffect(Stat activateTurnCost, Func<Task> logic, PlayActionType playActionType, Investigator playedBy, string description = null)
        {
            ActivateTurnsCost = activateTurnCost;
            Logic = logic;
            PlayActionType = playActionType;
            Investigator = playedBy;
            _description = description;
        }

        /*******************************************************************/

        public bool IsActionType(PlayActionType actionType) => (PlayActionType & actionType) == actionType;
    }
}
