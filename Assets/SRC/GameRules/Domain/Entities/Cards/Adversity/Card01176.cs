using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01176 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Omen };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<ChallengePhaseGameAction>().SetWith(investigator.Power, 4, "Yellow Sign", this, failEffect: HarmAndWeakness).Start();

            /*******************************************************************/
            async Task HarmAndWeakness()
            {
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, fromCard: this, amountFear: 2).Start();
                Card madness = investigator.DeckZone.Cards.FirstOrDefault(card => card.HasThisTag(Tag.Weakness) && card.HasThisTag(Tag.Madness));
                if (madness != null) await _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, madness).Start();
            }
        }
    }
}
