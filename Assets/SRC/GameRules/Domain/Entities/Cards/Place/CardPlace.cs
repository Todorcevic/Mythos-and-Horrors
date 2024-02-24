using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardPlace : Card, IStartReactionable, IEndReactionable, IRevellable
    {
        [Inject] private readonly List<History> _histories;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly TextsProvider _textsProvider;

        public Stat Hints { get; private set; }
        public Stat Enigma { get; private set; }
        public Stat InvestigationCost { get; private set; }
        public State Revealed { get; private set; }
        public History RevealHistory => _histories[0];



        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0);
            Enigma = new Stat(Info.Enigma ?? 0);
            InvestigationCost = new Stat(1, 1);
            Revealed = new State(false);
        }

        /*******************************************************************/
        public virtual async Task WhenFinish(GameAction gameAction)
        {
            await CheckIfCanReveal(gameAction);
        }

        private async Task CheckIfCanReveal(GameAction gameAction)
        {
            if (Revealed.Value) return;
            if (gameAction is not MoveInvestigatorGameAction) return;
            if (!OwnZone.Cards.Exists(card => card is CardAvatar)) return;

            await _gameActionFactory.Create(new RevealGameAction(this));
        }

        public virtual async Task WhenBegin(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction turnInvestigatorGA) return;
            if (turnInvestigatorGA.ActiveInvestigator.CurrentPlace != this) return;
            if (turnInvestigatorGA.ActiveInvestigator.Turns.Value < InvestigationCost.Value) return;

            AddEffect(new Effect(turnInvestigatorGA.ActiveInvestigator, _textsProvider.GameText.DEFAULT_VOID_TEXT + "Investigate", Investigate));
            Task Investigate() => _gameActionFactory.Create(new InvestigateGameAction(turnInvestigatorGA.ActiveInvestigator, this));

            await Task.CompletedTask;
        }
    }
}
