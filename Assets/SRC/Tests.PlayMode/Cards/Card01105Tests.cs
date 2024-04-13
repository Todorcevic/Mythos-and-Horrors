//using MythosAndHorrors.GameRules;
//using System.Collections;
//using UnityEngine.TestTools;
//using Zenject;

//namespace MythosAndHorrors.PlayMode.Tests
//{
//    public class Card01105Tests : TestBase
//    {
//        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
//        [Inject] private readonly CardsProvider _cardsProvider;
//        [Inject] private readonly GameActionsProvider _gameActionsProvider;
//        [Inject] private readonly ChaptersProvider _chaptersProvider;

//        protected override bool DEBUG_MODE => true;

//        /*******************************************************************/
//        [UnityTest]
//        public IEnumerator DiscardAll()
//        {
//            CardPlot cardPlot = _cardsProvider.GetCard<CardPlot>("01105");
//            yield return PlayAllInvestigators();
//            Investigator investigator = _investigatorProvider.Leader;
//            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
//            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();
//            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine();

//        }

//        [UnityTest]
//        public IEnumerator TakeDamage()
//        {
//            CardPlot cardPlot = _cardsProvider.GetCard<CardPlot>("01105");
//            yield return PlayAllInvestigators();
//            Investigator investigator = _investigatorProvider.Leader;
//            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardPlot, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
//            yield return _gameActionsProvider.Create(new DecrementStatGameAction(cardPlot.Eldritch, cardPlot.Eldritch.Value)).AsCoroutine();
//            yield return _gameActionsProvider.Create(new CheckEldritchsPlotGameAction()).AsCoroutine();

//        }
//    }
//}
