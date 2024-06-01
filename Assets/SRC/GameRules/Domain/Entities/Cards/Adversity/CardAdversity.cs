using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversity : Card
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public GameCommand<Investigator> PlayFromDraw { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromDraw = new(PlayAdversityFor);
        }

        /*******************************************************************/
        protected virtual async Task PlayAdversityFor(Investigator investigator)
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, investigator.DangerZone));
        }
    }
}
