using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01515 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Madness };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, fromCard: this, amountFear: 2, isDirect: true).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(investigator.DiscardZone.Cards, _chaptersProvider.CurrentScene.OutZone).Execute();
        }
    }
}
