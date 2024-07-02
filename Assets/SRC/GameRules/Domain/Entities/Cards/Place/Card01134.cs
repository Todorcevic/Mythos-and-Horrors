using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01134 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public State Played { get; private set; }
        public Activation<Investigator> GainHints { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Arkham };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Played = CreateState(false);
            GainHints = CreateActivation(1, Logic, Condition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private async Task Logic(Investigator investigator)
        {
            await _gameActionsProvider.Create<PayResourceGameAction>().SetWith(investigator, 5).Start();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(Hints, 2).Start();
            await _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, Hints, 2).Start();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Played, true).Start();
        }

        private bool Condition(Investigator investigator)
        {
            if (Played.IsActive) return false;
            if (investigator.CurrentPlace != this) return false;
            if (investigator.Resources.Value < 5) return false;
            return true;
        }

        /*******************************************************************/
    }
}
