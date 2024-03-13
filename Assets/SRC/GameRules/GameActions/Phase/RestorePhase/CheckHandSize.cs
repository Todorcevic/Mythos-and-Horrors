using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckHandSize : GameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly EffectsProvider _effectProvider;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public CheckHandSize(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            while (Investigator.HandSize > GameValues.MAX_HAND_SIZE)
            {
                Create();
                await _gameActionProvider.Create(new InteractableGameAction());
            }
        }

        private void Create()
        {
            foreach (Card card in Investigator.HandZone.Cards)
            {
                _effectProvider.Create()
                    .SetCard(card)
                    .SetInvestigator(Investigator)
                    .SetCanPlay(CanChoose)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Discard))
                    .SetLogic(Discard);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionProvider.Create(new DiscardGameAction(card));
                    await _gameActionProvider.Create(new CheckHandSize(Investigator));
                };

                bool CanChoose()
                {
                    if (card is IFlaw) return false;
                    return true;
                }
            }
        }
    }
}
