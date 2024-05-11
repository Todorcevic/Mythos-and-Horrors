using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    public class PrepareSceneCORE1Tests : SetupAutoInject
    {
        [Inject] private readonly PrepareGameRulesUseCase _prepareGameRulesUseCase;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        //[Inject] private readonly PreparationSceneCORE1 _preparationSceneCORE1;

        SceneCORE1 Scene => (SceneCORE1)_chaptersProvider.CurrentScene;

        /*******************************************************************/
        [Test]
        public async void PrepareSceneCORE1()
        {
            _prepareGameRulesUseCase.Execute();
            await PlayAllInvestigators(withAvatar: false);
            await _gameActionsProvider.Create(new PrepareSceneGameAction(_chaptersProvider.CurrentScene));

            Assert.That(Scene.GoalZone.Cards.Unique(), Is.EqualTo(Scene.FirstGoal));
            Assert.That(Scene.PlotZone.Cards.Unique(), Is.EqualTo(Scene.FirstPlot));
            Assert.That(Scene.DangerDeckZone.Cards.Count(), Is.EqualTo(Scene.RealDangerCards.Count()));

        }

        public async Task PlayAllInvestigators(bool withCards = true, bool withResources = true, bool withAvatar = true)
        {
            //SceneCORE1 scene = (SceneCORE1)_chaptersProvider.CurrentScene;
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
            {
                if (withCards)
                    await _gameActionsProvider.Create(new MoveCardsGameAction(GetCardZonesInvestigator(investigator, true)));
                if (withResources)
                    await _gameActionsProvider.Create(new GainResourceGameAction(investigator, 5));
            }
            if (withAvatar)
                await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(_investigatorsProvider.AllInvestigators, Scene.Study));
        }

        private Dictionary<Card, (Zone zone, bool faceDown)> GetCardZonesInvestigator(Investigator investigator, bool withCards)
        {
            Dictionary<Card, (Zone zone, bool faceDown)> moveInvestigatorCards = new()
            {
                { investigator.InvestigatorCard, (investigator.InvestigatorZone, false) }
            };

            if (withCards)
            {
                investigator.FullDeck.Take(5).ForEach(card => moveInvestigatorCards.Add(card, (investigator.HandZone, false)));
                investigator.FullDeck.Skip(5).ForEach(card => moveInvestigatorCards.Add(card, (investigator.DeckZone, true)));
            }
            return moveInvestigatorCards;
        }
    }
}
