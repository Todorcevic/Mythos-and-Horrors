using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardPlace : Card, IEndReactionable, IStartReactionable, IRevellable
    {
        private List<CardPlace> _connectedPlacesToMove;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public Stat Hints { get; private set; }
        public Stat Enigma { get; private set; }
        public Stat InvestigationTurnsCost { get; private set; }
        public Stat MoveTurnsCost { get; private set; }
        public State Revealed { get; private set; }
        public Reaction MustReveal { get; private set; }

        /*******************************************************************/
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public List<CardPlace> ConnectedPlacesToMove =>
            _connectedPlacesToMove ??= ExtraInfo?.ConnectedPlaces?.Select(code => _cardsProvider.GetCard<CardPlace>(code)).ToList();
        public List<CardPlace> ConnectedPlacesFromMove => _cardsProvider.GetCardsThatCanMoveTo(this);

        /*******************************************************************/
        [Inject]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0);
            Enigma = new Stat(Info.Enigma ?? 0);
            InvestigationTurnsCost = new Stat(1, 1);
            MoveTurnsCost = new Stat(1, 1);
            Revealed = new State(false);
            MustReveal = new Reaction(CheckReveal, Reveal);
        }

        /*******************************************************************/
        public virtual async Task WhenFinish(GameAction gameAction)
        {
            await MustReveal.Check(gameAction);
        }

        public virtual async Task WhenBegin(GameAction gameAction)
        {
            CheckInvestigate(gameAction);
            CheckMove(gameAction);
            await Task.CompletedTask;
        }

        /************************** REVEAL *****************************/
        protected bool CheckReveal(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction) return false;
            if (Revealed.IsActive) return false;
            if (!OwnZone.Cards.Exists(card => card is CardAvatar)) return false;
            return true;
        }

        protected async Task Reveal() => await _gameActionFactory.Create(new RevealGameAction(this));

        /************************** INVESTIGATE *****************************/
        protected void CheckInvestigate(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneTurnGA) return;

            _effectProvider.Create()
                .SetCard(this)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Investigate))
                .SetInvestigator(_investigatorProvider.ActiveInvestigator)
                .SetCanPlay(CanInvestigate)
                .SetLogic(Investigate);
        }

        protected bool CanInvestigate()
        {
            if (_investigatorProvider.ActiveInvestigator.CurrentPlace != this) return false;
            if (_investigatorProvider.ActiveInvestigator.Turns.Value < InvestigationTurnsCost.Value) return false;
            return true;
        }

        protected async Task Investigate()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(_investigatorProvider.ActiveInvestigator.Turns, InvestigationTurnsCost.Value));
            await _gameActionFactory.Create(new InvestigateGameAction(_investigatorProvider.ActiveInvestigator, this));
        }

        /************************** MOVE *****************************/
        protected void CheckMove(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneTurnGA) return;

            _effectProvider.Create()
                .SetCard(this)
                .SetInvestigator(_investigatorProvider.ActiveInvestigator)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Move))
                .SetCanPlay(CanMove)
                .SetLogic(Move);
        }

        protected virtual bool CanMove()
        {
            if (_investigatorProvider.ActiveInvestigator.CurrentPlace == null) return false;
            if (!ConnectedPlacesFromMove.Contains(_investigatorProvider.ActiveInvestigator.CurrentPlace)) return false;
            if (_investigatorProvider.ActiveInvestigator.Turns.Value < MoveTurnsCost.Value) return false;

            return true;
        }

        protected async Task Move()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(_investigatorProvider.ActiveInvestigator.Turns, MoveTurnsCost.Value));
            await _gameActionFactory.Create(new MoveToPlaceGameAction(_investigatorProvider.ActiveInvestigator, this));
        }
    }
}
