using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardPlace : Card, IRevealable
    {
        private IEnumerable<CardPlace> _connectedPlacesToMove;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Stat Hints { get; private set; }
        public Stat Enigma { get; private set; }
        public Stat InvestigationTurnsCost { get; private set; }
        public Stat MoveTurnsCost { get; private set; }
        public State Revealed { get; private set; }
        public IReaction RevealReaction { get; private set; }

        /*******************************************************************/
        public int MaxHints => (Info.Hints ?? 0) * _investigatorsProvider.AllInvestigators.Count();
        public bool IsAlone => !OwnZone.Cards.Any(card => card is CardAvatar || card is CardCreature);
        public History RevealHistory => ExtraInfo?.Histories.ElementAtOrDefault(0) ?? new History();
        public IEnumerable<CardCreature> CreaturesInThisPlace => _cardsProvider.GetCardsInPlay().OfType<CardCreature>()
            .Where(creature => creature.CurrentPlace == this);
        public IEnumerable<Investigator> InvestigatorsInThisPlace => _investigatorsProvider.AllInvestigatorsInPlay
            .Where(investigator => investigator.CurrentPlace == this);
        public IEnumerable<CardPlace> ConnectedPlacesToMove =>
            _connectedPlacesToMove ??= ExtraInfo?.ConnectedPlaces?
            .Select(code => _cardsProvider.GetCardByCode(code)).Where(card => card.IsInPlay).OfType<CardPlace>();
        public IEnumerable<CardPlace> ConnectedPlacesFromMove => _cardsProvider.GetCardsThatCanMoveTo(this);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Hints = CreateStat(MaxHints);
            Enigma = CreateStat(Info.Enigma ?? 0);
            InvestigationTurnsCost = CreateStat(1);
            MoveTurnsCost = CreateStat(1);
            Revealed = CreateState(false);
            CreateReaction<MoveCardsGameAction>(RevealCondition, RevealLogic, false);
        }

        /*******************************************************************/
        public virtual async Task RevealEffect()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(RevealHistory, this));
        }

        /*******************************************************************/
        private bool RevealCondition(MoveCardsGameAction moveCardsGameAction)
        {
            if (Revealed.IsActive) return false;
            if (!OwnZone.Cards.Any(card => card is CardAvatar)) return false;
            return true;
        }

        private async Task RevealLogic(MoveCardsGameAction moveCardsGameAction) =>
            await _gameActionsProvider.Create(new RevealGameAction(this));

        public int DistanceTo(CardPlace cardPlace)
        {
            List<CardPlace> locationsCheck = new();
            int distance = 0;
            return FindPath(new[] { this }, cardPlace);

            int FindPath(IEnumerable<CardPlace> listLocation, CardPlace moveToLocation)
            {
                List<CardPlace> listToCheck = new();
                foreach (CardPlace location in listLocation)
                {
                    if (location == moveToLocation) return distance;
                    locationsCheck.Add(location);
                    listToCheck.AddRange(location.ConnectedPlacesToMove
                        .Where(cardPlace => !locationsCheck.Contains(cardPlace) && !listToCheck.Contains(cardPlace)));
                }
                distance++;
                if (listToCheck.Count > 0) return FindPath(listToCheck, moveToLocation);
                return int.MaxValue;
            }
        }
    }
}
