using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardPlace : Card, IEndReactionable, IRevellable
    {
        private List<CardPlace> _connectedPlacesToMove;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;

        public Stat Hints { get; private set; }
        public Stat Enigma { get; private set; }
        public Stat InvestigationCost { get; private set; }
        public Stat MoveCost { get; private set; }
        public State Revealed { get; private set; }
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public List<CardPlace> ConnectedPlacesToMove => _connectedPlacesToMove ??= ExtraInfo?.ConnectedPlaces?.Select(code => _cardsProvider.GetCard<CardPlace>(code)).ToList();
        public List<CardPlace> ConnectedPlacesFromMove => _cardsProvider.GetCardsThatCanMoveTo(this);
        public Reaction MustReveal { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0);
            Enigma = new Stat(Info.Enigma ?? 0);
            InvestigationCost = new Stat(1, 1);
            MoveCost = new Stat(1, 1);
            Revealed = new State(false);
            MustReveal = new Reaction(CheckIfMustReveal, Reveal);
        }

        /*******************************************************************/
        public virtual async Task WhenFinish(GameAction gameAction)
        {
            await MustReveal.Check(gameAction);
        }

        /************************** REACTIONS *****************************/
        private bool CheckIfMustReveal(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction) return false;
            if (Revealed.IsActive) return false;
            if (!OwnZone.Cards.Exists(card => card is CardAvatar)) return false;
            return true;
        }

        private async Task Reveal() => await _gameActionFactory.Create(new RevealGameAction(this));

        /************************** CONDITIONS *****************************/
        public virtual bool CanMoveWithThis(Investigator investigator)
        {
            if (!ConnectedPlacesFromMove.Contains(investigator.CurrentPlace)) return false;
            if (investigator.Turns.Value < MoveCost.Value) return false;
            return true;
        }
    }
}
