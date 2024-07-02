using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01107 : CardPlot
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        IEnumerable<CardCreature> GhoulsToMove() => _cardsProvider.AllCards.OfType<CardCreature>()
              .Where(creature => creature.Tags.Contains(Tag.Ghoul) && creature.IsInPlay && !creature.IsConfronted);
        IEnumerable<Investigator> InvestigatorsUnresignes() => _investigatorsProvider.AllInvestigators
              .Where(investigator => !investigator.Resign.IsActive);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<CreaturePhaseGameAction>(MoveGhoulCondition, MoveGhoulLogic, GameActionTime.After);
            CreateForceReaction<RoundGameAction>(PlaceEldritchCondition, PlaceEldritchLogic, GameActionTime.After);
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            if (_chaptersProvider.CurrentScene.CurrentGoal is not Card01110)
                await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.FullResolutions[3]));
            else
            {
                await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigatorsUnresignes, SufferInjury).Start();
                await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.FullResolutions[0]));
            }
            /*******************************************************************/

            async Task SufferInjury(Investigator investigator) =>
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(investigator.Injury, 1).Start();
        }

        /*******************************************************************/
        private bool MoveGhoulCondition(CreaturePhaseGameAction gameAction)
        {
            if (!IsInPlay) return false;
            return true;
        }

        private async Task MoveGhoulLogic(CreaturePhaseGameAction gameAction)
        {
            Card01115 parlor = _cardsProvider.GetCard<Card01115>();
            await _gameActionsProvider.Create<SafeForeach<CardCreature>>().SetWith(GhoulsToMove, MoveCreature).Start();

            /*******************************************************************/
            async Task MoveCreature(CardCreature ghoul) =>
                await _gameActionsProvider.Create<MoveCreatureGameAction>().SetWith(ghoul, ghoul.CurrentPlace.DistanceTo(parlor).path).Start();
        }

        /*******************************************************************/
        private bool PlaceEldritchCondition(RoundGameAction gameAction)
        {
            if (!IsInPlay) return false;
            return true;
        }

        private async Task PlaceEldritchLogic(RoundGameAction gameAction)
        {
            CardPlace parlor = _cardsProvider.GetCard<Card01115>();
            CardPlace hallway = _cardsProvider.GetCard<Card01112>();
            int amountEldritch = _cardsProvider.AllCards.OfType<CardCreature>()
                  .Where(creature => creature.Tags.Contains(Tag.Ghoul) && (creature.CurrentPlace == parlor || creature.CurrentPlace == hallway)).Count();

            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Eldritch, amountEldritch).Start();
        }
    }
}
