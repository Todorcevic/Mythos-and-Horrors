using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardPlace : Card, IStartReactionable, IEndReactionable, IRevellable
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public Stat Hints { get; private set; }
        public Stat Enigma { get; private set; }
        public Stat InvestigationCost { get; private set; }
        public Stat MoveCost { get; private set; }
        public State IsRevealed { get; private set; }
        public History RevealHistory => ExtraInfo.Histories.ElementAtOrDefault(0);
        public List<CardPlace> ConnectedPlacesToMove => ExtraInfo?.ConnectedPlaces?.Select(code => _cardsProvider.GetCard<CardPlace>(code)).ToList();
        public List<CardPlace> ConnectedPlacesFromMove => _cardsProvider.GetCardsThatCanMoveTo(this);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0);
            Enigma = new Stat(Info.Enigma ?? 0);
            InvestigationCost = new Stat(1, 1);
            MoveCost = new Stat(1, 1);
            IsRevealed = new State(false);
        }

        /*******************************************************************/
        public virtual async Task WhenFinish(GameAction gameAction)
        {
            if (gameAction is MoveCardsGameAction) await CheckIfCanReveal();
        }

        public virtual async Task WhenBegin(GameAction gameAction)
        {
            if (gameAction is OneInvestigatorTurnGameAction investigatorTurnGA)
            {
                await CheckIfInvestigate(investigatorTurnGA);
                await CheckIfMove(investigatorTurnGA);
            }
        }

        /*******************************************************************/
        private async Task CheckIfCanReveal()
        {
            if (IsRevealed.Value) return;
            if (!OwnZone.Cards.Exists(card => card is CardAvatar)) return;

            await _gameActionFactory.Create(new RevealGameAction(this));
        }

        private async Task CheckIfInvestigate(OneInvestigatorTurnGameAction turnInvestigatorGA)
        {
            if (turnInvestigatorGA.ActiveInvestigator.CurrentPlace != this) return;
            if (turnInvestigatorGA.ActiveInvestigator.Turns.Value < InvestigationCost.Value) return;

            AddEffect(turnInvestigatorGA.ActiveInvestigator, _textsProvider.GameText.DEFAULT_VOID_TEXT + "Investigate", Investigate);
            await Task.CompletedTask;

            /*******************************************************************/
            Task Investigate() => _gameActionFactory.Create(new InvestigateGameAction(turnInvestigatorGA.ActiveInvestigator, this));
        }

        private async Task CheckIfMove(OneInvestigatorTurnGameAction turnInvestigatorGA)
        {
            if (!ConnectedPlacesFromMove.Contains(turnInvestigatorGA.ActiveInvestigator.CurrentPlace)) return;
            if (turnInvestigatorGA.ActiveInvestigator.Turns.Value < MoveCost.Value) return;

            AddEffect(turnInvestigatorGA.ActiveInvestigator, _textsProvider.GameText.DEFAULT_VOID_TEXT + "Move", Move);
            await Task.CompletedTask;

            /*******************************************************************/
            Task Move() => _gameActionFactory.Create(new MoveToPlaceGameAction(turnInvestigatorGA.ActiveInvestigator, this));
        }
    }
}
