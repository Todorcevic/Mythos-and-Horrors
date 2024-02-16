using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardPlace : Card, IEndReactionable, IRevellable
    {
        private bool _isReveled;
        [Inject] private readonly List<History> _histories;

        public Stat Hints { get; private set; }
        public Stat Enigma { get; private set; }
        public History RevealHistory => _histories[0];

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0, Info.Hints ?? 0);
            Enigma = new Stat(Info.Enigma ?? 0);
        }

        /*******************************************************************/
        void IRevellable.Reveal() => _isReveled = true;

        async Task IEndReactionable.WhenFinish(GameAction gameAction)
        {
            if (_isReveled) return;
            if (gameAction is not MoveInvestigatorGameAction moveCardsGameAction) return;
            if (!OwnZone.Cards.Exists(card => card is CardAvatar)) return;

            await _gameActionFactory.Create(new RevealGameAction(this));
        }
    }
}
