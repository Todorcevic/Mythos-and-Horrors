using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01171 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Hex };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            CardCreature nearestCultist = investigator.NearestCreatures.FirstOrDefault(card => card.HasThisTag(Tag.Cultist));
            if (nearestCultist != null)
            {
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(nearestCultist.Eldritch, 2).Start();
            }
            else
            {
                CardCreature searchedCultist = _chaptersProvider.CurrentScene.DangerDeckZone.Cards.Concat(_chaptersProvider.CurrentScene.DangerDiscardZone.Cards)
                    .OfType<CardCreature>().FirstOrDefault(card => card.HasThisTag(Tag.Cultist));
                await _gameActionsProvider.Create(new DrawGameAction(investigator, searchedCultist));
                await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(_chaptersProvider.CurrentScene.DangerDeckZone).Start();
            }
        }
    }
}
