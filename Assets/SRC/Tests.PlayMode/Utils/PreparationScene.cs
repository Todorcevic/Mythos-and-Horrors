using MythosAndHorrors.GameRules;
using Zenject;
using System.Collections;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PreparationScene
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public IEnumerator PlaceAllPlaceCards()
        {
            CardPlace place1 = _cardsProvider.GetCard<Card01111>();
            CardPlace place2 = _cardsProvider.GetCard<Card01112>();
            CardPlace place3 = _cardsProvider.GetCard<Card01113>();
            CardPlace place4 = _cardsProvider.GetCard<Card01114>();
            CardPlace place5 = _cardsProvider.GetCard<Card01115>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place1, _chaptersProvider.CurrentScene.PlaceZone[0, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place2, _chaptersProvider.CurrentScene.PlaceZone[1, 3])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place3, _chaptersProvider.CurrentScene.PlaceZone[2, 4])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place4, _chaptersProvider.CurrentScene.PlaceZone[0, 4])).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(place5, _chaptersProvider.CurrentScene.PlaceZone[1, 4])).AsCoroutine();
        }

        public IEnumerator PlayLeadInvestigator()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.Leader);
        }

        public IEnumerator PlayThisInvestigator(Investigator investigator)
        {
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.InvestigatorCard, investigator.InvestigatorZone)).AsCoroutine();
            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator.FullDeck, investigator.DeckZone, isFaceDown: true)).AsCoroutine();
        }

        public IEnumerator PlayAllInvestigators()
        {
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
                yield return PlayThisInvestigator(investigator);
        }
    }
}
