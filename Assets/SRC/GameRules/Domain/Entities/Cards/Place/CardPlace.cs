using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public interface IRevelable
    {
        void Reveal();
    }

    public class CardPlace : Card, IEndReactionable, IRevelable
    {
        public Stat Hints { get; private set; }
        public Stat Enigma { get; private set; }
        public bool IsReveled { get; private set; }

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
            if (gameAction is not MoveInvestigatorGameAction moveCardsGameAction) return;
            if (!OwnZone.Cards.Exists(card => card is CardAvatar)) return;

            await _gameActionFactory.Create(new RevealCardGameAction(this));
        }

        void IRevelable.Reveal() => IsReveled = true;

    }
}
