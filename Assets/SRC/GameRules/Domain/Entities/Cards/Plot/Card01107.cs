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

        public Reaction<CreaturePhaseGameAction> MoveGhoulReaction { get; private set; }
        public Reaction<RestorePhaseGameAction> PlaceEldritch { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            MoveGhoulReaction = new Reaction<CreaturePhaseGameAction>(MoveGhoulCondition, MoveGhoulLogic);
            PlaceEldritch = new Reaction<RestorePhaseGameAction>(PlaceEldritchCondition, PlaceEldritchLogic);
        }

        /*******************************************************************/
        public override async Task CompleteEffect()
        {
            if (_chaptersProvider.CurrentScene.CurrentGoal is not Card01110)
                await _chaptersProvider.CurrentScene.Resolution3();
            else await new SafeForeach<Investigator>(SufferInjury, GetInvestigators).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> GetInvestigators() => _investigatorsProvider.AllInvestigators
                   .Where(investigator => !investigator.Resign.IsActive);

            async Task SufferInjury(Investigator investigator) =>
                await _gameActionsProvider.Create(new IncrementStatGameAction(investigator.Injury, 1));
        }

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await MoveGhoulReaction.CheckToReact(gameAction);
            await PlaceEldritch.CheckToReact(gameAction);
        }

        /*******************************************************************/
        private bool MoveGhoulCondition(GameAction gameAction)
        {
            if (!IsInPlay) return false;
            return true;
        }

        private async Task MoveGhoulLogic(GameAction gameAction)
        {
            Card01115 parlor = _cardsProvider.GetCard<Card01115>();
            await new SafeForeach<CardCreature>(MoveCreature, GetGhouls).Execute();

            /*******************************************************************/
            async Task MoveCreature(CardCreature ghoul) =>
                await _gameActionsProvider.Create(new MoveCreatureGameAction(ghoul, parlor));

            IEnumerable<CardCreature> GetGhouls() => _cardsProvider.AllCards.OfType<CardCreature>()
                    .Where(creature => creature.Tags.Contains(Tag.Ghoul) && creature.IsInPlay && !creature.IsConfronted);
        }

        /*******************************************************************/
        private bool PlaceEldritchCondition(GameAction gameAction)
        {
            if (!IsInPlay) return false;
            return true;
        }

        private async Task PlaceEldritchLogic(GameAction gameAction)
        {
            CardPlace parlor = _cardsProvider.GetCard<Card01115>();
            CardPlace hallway = _cardsProvider.GetCard<Card01112>();
            int amountEldritch = _cardsProvider.AllCards.OfType<CardCreature>()
                  .Where(creature => creature.Tags.Contains(Tag.Ghoul) && (creature.CurrentPlace == parlor || creature.CurrentPlace == hallway)).Count();

            await _gameActionsProvider.Create(new DecrementStatGameAction(Eldritch, amountEldritch));
        }
    }
}
