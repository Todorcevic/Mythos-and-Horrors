using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01162 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Hazard };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            ChallengePhaseGameAction challengeGameAction = _gameActionsProvider.Create<ChallengePhaseGameAction>();
            await challengeGameAction.SetWith(investigator.Agility, 3, "Challenge_Card01162", this, failEffect: FailEffect, localizableArgs: Info.Name).Execute();

            /*******************************************************************/
            async Task FailEffect() =>
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, this, amountDamage: challengeGameAction.ResultChallenge.TotalDifferenceValue * -1).Execute();
        }
    }
}
