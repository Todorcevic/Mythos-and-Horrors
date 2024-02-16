using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardPlace : Card, IEndReactionable
    {
        private bool isReveled;

        public Stat Hints { get; private set; }
        public Stat Enigma { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0, Info.Hints ?? 0);
            Enigma = new Stat(Info.Enigma ?? 0);
        }
        /*******************************************************************/

        async Task IEndReactionable.WhenFinish(GameAction gameAction)
        {
            if (isReveled) return;
            if (gameAction is not MoveInvestigatorGameAction moveCardsGameAction) return;
            if (!OwnZone.Cards.Exists(card => card is CardAvatar)) return;

            isReveled = true;
            await _gameActionFactory.Create(new ShowHistoryGameAction(Histories.First()));
            await _gameActionFactory.Create(new RevealPlaceGameAction(this));
        }
    }
}
