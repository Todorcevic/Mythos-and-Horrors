using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CommitableCard : Card
    {
        public Investigator InvestigatorCommiter { get; private set; }
        public Stat Strength { get; private set; }
        public Stat Agility { get; private set; }
        public Stat Intelligence { get; private set; }
        public Stat Power { get; private set; }
        public Stat Wild { get; private set; }
        public State Commited { get; private set; }

        public override Investigator ControlOwner => InvestigatorCommiter ?? base.ControlOwner;
        public override bool IsInPlay => !Commited.IsActive && base.IsInPlay;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Strength = CreateStat(Info.Strength ?? 0);
            Agility = CreateStat(Info.Agility ?? 0);
            Intelligence = CreateStat(Info.Intelligence ?? 0);
            Power = CreateStat(Info.Power ?? 0);
            Wild = CreateStat(Info.Wild ?? 0);
            Commited = CreateState(false, OnCommited);
        }

        /*******************************************************************/
        public int GetChallengeValue(ChallengeType challengeType)
        {
            return challengeType switch
            {
                ChallengeType.Strength => Wild.Value + Strength.Value,
                ChallengeType.Agility => Wild.Value + Agility.Value,
                ChallengeType.Intelligence => Wild.Value + Intelligence.Value,
                ChallengeType.Power => Wild.Value + Power.Value,
                _ => Wild.Value
            };
        }

        private void OnCommited(bool isActive)
        {
            if (isActive) InvestigatorCommiter = ControlOwner;
            else InvestigatorCommiter = null;
        }
    }
}
