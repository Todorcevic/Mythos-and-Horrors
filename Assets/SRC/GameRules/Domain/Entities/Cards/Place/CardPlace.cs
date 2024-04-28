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
        public Reaction<MoveCardsGameAction> Reveal { get; private set; }

        /*******************************************************************/
        public int MaxHints => (Info.Hints ?? 0) * _investigatorsProvider.AllInvestigators.Count;
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public IEnumerable<CardPlace> ConnectedPlacesToMove =>
            _connectedPlacesToMove ??= ExtraInfo?.ConnectedPlaces?
            .Select(code => _cardsProvider.GetCardByCode(code)).Where(card => card.IsInPlay).OfType<CardPlace>();
        public IEnumerable<CardPlace> ConnectedPlacesFromMove => _cardsProvider.GetCardsThatCanMoveTo(this);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Hints = new Stat(MaxHints);
            Enigma = new Stat(Info.Enigma ?? 0);
            InvestigationTurnsCost = new Stat(1);
            MoveTurnsCost = new Stat(1);
            Revealed = new State(false);
            Reveal = new Reaction<MoveCardsGameAction>(RevealCondition, RevealLogic);
        }

        /*******************************************************************/
        public virtual async Task RevealEffect()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(RevealHistory, this));
        }

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await Reveal.Check(gameAction);
        }
        /*******************************************************************/

        private bool RevealCondition(MoveCardsGameAction gameAction)
        {
            if (Revealed.IsActive) return false;
            if (!OwnZone.Cards.Any(card => card is CardAvatar)) return false;
            return true;
        }

        private async Task RevealLogic(MoveCardsGameAction gameAction) => await _gameActionsProvider.Create(new RevealGameAction(this));
    }
}
