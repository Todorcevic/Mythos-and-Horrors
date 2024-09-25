using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01173 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        private SceneCORE2 SceneCORE2 => (SceneCORE2)_chaptersProvider.CurrentScene;

        public override IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<ChallengePhaseGameAction>().SetWith(investigator.Agility, 4, new Localization("Challenge_Card01173", CurrentName), this, failEffect: HarmAndMove).Execute();

            /*******************************************************************/
            async Task HarmAndMove()
            {
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, fromCard: this, amountDamage: 1, amountFear: 1).Execute();
                Dictionary<Card, Zone> moveAndDisconfront = investigator.BasicCreaturesConfronted.Where(creature => !creature.HasThisTag(Tag.Nightgaunt))
                    .ToDictionary(creature => (Card)creature, creature => creature.CurrentPlace.OwnZone);
                moveAndDisconfront.Add(investigator.AvatarCard, SceneCORE2.Center.OwnZone);
                await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(moveAndDisconfront).Execute();
            }
        }
    }
}
